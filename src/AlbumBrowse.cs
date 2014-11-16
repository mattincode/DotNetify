using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetify
{
    /// <summary>
    /// Represents a query for additional album metadata.
    /// </summary>
    /// <remarks>
    /// This class destroys itself once the query has been performed. The queried tracks' reference count
    /// will be incremented before the albumbrowse object is destroyed, so they won't be released together.
    /// </remarks>
    internal class AlbumBrowse : SessionObject
    {
        /// <summary>
        /// Backing field.
        /// </summary>
        private Album _Album;

        /// <summary>
        /// The <see cref="Album"/> whose properties will be populated when the albumbrowse is complete.
        /// </summary>
        public Album Album
        {
            get
            {
                return _Album;
            }
            private set
            {
                this.SetProperty(ref _Album, value);
            }
        }

        /// <summary>
        /// Initializes a new <see cref="AlbumBrowse"/>.
        /// </summary>
        /// <param name="session">The <see cref="Session"/>.</param>
        /// <param name="album">The <see cref="Album"/> to get additional information about.</param>
        public AlbumBrowse(Session session, Album album)
            : base(session)
        {
            Contract.Requires<ArgumentNullException>(session != null);
            Contract.Requires<ArgumentNullException>(album != null);

            lock (NativeMethods.LibraryLock)
            {
                this.Handle = NativeMethods.sp_albumbrowse_create(session.Handle, album.Handle, this.AlbumbrowseCompleteCallback, IntPtr.Zero);
            }
        }

        /// <summary>
        /// Adds a reference to the underlying libspotify albumbrowse object.
        /// </summary>
        public override void AddRef()
        {
            lock (NativeMethods.LibraryLock)
            {
                NativeMethods.sp_albumbrowse_add_ref(this.Handle);
            }
        }

        /// <summary>
        /// Disposes the <see cref="AlbumBrowse"/> decrementing the reference counter of the underlying libspotify albumbrowse object.
        /// </summary>
        /// <param name="disposing">Indicates whether to dispose managed resources as well.</param>
        protected override void Dispose(bool disposing)
        {
            lock (NativeMethods.LibraryLock)
            {
                NativeMethods.sp_albumbrowse_release(this.Handle);
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// The callback called when the albumbrowse completed.
        /// </summary>
        /// <param name="albumBrowse">The handle to the underlying libspotify albumbrowse object.</param>
        /// <param name="userData">Custom userdata. Not used in DotNetify.</param>
        private void AlbumbrowseCompleteCallback(IntPtr albumBrowse, IntPtr userData)
        {
            Session s = this.Session;
            if (s == null)
            {
                throw new InvalidOperationException("Albumbrowse cannot be completed. Session is null. This is a programming bug, and SHOULD NOT happen at runtime. Please contact the developers.");
            }
            Album a = this.Album;
            if (a == null)
            {
                throw new InvalidOperationException("Albumbrowse cannot be completed. Album is null. This is a programming bug, and SHOULD NOT happen at runtime. Please contact the developers.");
            }

            lock (NativeMethods.LibraryLock)
            {
                IntPtr handle = this.Handle;
                if (this.IsLoaded = NativeMethods.sp_albumbrowse_is_loaded(handle) && NativeMethods.sp_albumbrowse_error(handle) == Result.Ok)
                {
                    a.Copyrights = Enumerable.Range(0, NativeMethods.sp_albumbrowse_num_copyrights(handle))
                                             .Select(i => NativeMethods.sp_albumbrowse_copyright(handle, i).AsString())
                                             .ToArray();
                    a.Review = NativeMethods.sp_albumbrowse_review(handle).AsString();
                    Track[] tracks  = Enumerable.Range(0, NativeMethods.sp_albumbrowse_num_tracks(handle))
                                                .Select(i => new Track(s, NativeMethods.sp_albumbrowse_track(handle, i)))
                                                .ToArray();
                    foreach (Track t in tracks)
                    {
                        t.AddRef();
                    }
                    a.Tracks = tracks;
                }

                this.Dispose();
            }
        }
    }
}
