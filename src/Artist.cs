using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetify
{
    /// <summary>
    /// Represents an artist.
    /// </summary>
    public class Artist : SessionObject
    {
        /// <summary>
        /// Backing field.
        /// </summary>
        private Image _Portrait;

        /// <summary>
        /// Gets a portrait image of the artist.
        /// </summary>
        public Image Portrait
        {
            get
            {
                return _Portrait;
            }
            private set
            {
                this.SetProperty(ref _Portrait, value);
            }
        }

        /// <summary>
        /// Initializes a new <see cref="Artist"/>.
        /// </summary>
        /// <param name="session">The <see cref="Session"/>.</param>
        /// <param name="handle">The handle to the underlying libspotify artist object.</param>
        public Artist(Session session, IntPtr handle)
            : base(session, handle)
        {
            Contract.Requires<ArgumentNullException>(session != null);
            Contract.Requires<ArgumentNullException>(handle != IntPtr.Zero);

            session.MetadataUpdateReceived += (s, e) => this.LoadMetadata();
            this.LoadMetadata();
        }

        /// <summary>
        /// Increases the reference count to the underlying libspotify artist object preventing it from being
        /// released.
        /// </summary>
        public override void AddRef()
        {
            lock (NativeMethods.LibraryLock)
            {
                NativeMethods.sp_artist_add_ref(this.Handle);
            }
        }

        /// <summary>
        /// Disposes the <see cref="Artist"/> decreasing the reference count of the underlying libspotify artist object.
        /// </summary>
        /// <param name="disposing">Indicates whether to dispose managed resources as well.</param>
        protected override void Dispose(bool disposing)
        {
            lock (NativeMethods.LibraryLock)
            {
                NativeMethods.sp_artist_release(this.Handle);
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Loads the artists metadata.
        /// </summary>
        private void LoadMetadata()
        {
            Session s = this.Session;
            if (s == null)
            {
                throw new InvalidOperationException("Metadata cannot be loaded, session was null. This is a programming bug, and SHOULD NOT happen at runtime. Please contact the developers.");
            }

            lock (NativeMethods.LibraryLock)
            {
                IntPtr handle = this.Handle;
                if (this.IsLoaded = NativeMethods.sp_artist_is_loaded(handle))
                {
                    this.Name = NativeMethods.sp_artist_name(handle).AsString();
                    this.Portrait = new Image(s, NativeMethods.sp_artist_portrait(this.Handle, ImageSize.Large));
                }
            }
        }
    }
}
