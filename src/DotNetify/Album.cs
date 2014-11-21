using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetify
{
    /// <summary>
    /// Represents an album, a fixed collection of music.
    /// </summary>
    /// <remarks>
    /// This class wraps sp_album and sp_albumbrowse.
    /// </remarks>
    public class Album : SessionObject
    {
        /// <summary>
        /// Backing field.
        /// </summary>
        private Artist _Artist;

        /// <summary>
        /// The <see cref="Artist"/> that created the <see cref="Album"/>.
        /// </summary>
        public Artist Artist
        {
            get
            {
                return _Artist;
            }
            private set
            {
                this.SetProperty(ref _Artist, value);
            }
        }

        /// <summary>
        /// Backing field.
        /// </summary>
        private string[] _Copyrights;

        /// <summary>
        /// Copyright information shipped with the album.
        /// </summary>
        public string[] Copyrights
        {
            get
            {
                return _Copyrights;
            }
            private set
            {
                this.SetProperty(ref _Copyrights, value);
            }
        }

        /// <summary>
        /// Backing field.
        /// </summary>
        private Image _Cover;

        /// <summary>
        /// Gets the <see cref="Album"/>s cover.
        /// </summary>
        public Image Cover
        {
            get
            {
                return _Cover;
            }
            private set
            {
                this.SetProperty(ref _Cover, value);
            }
        }

        /// <summary>
        /// Backing field.
        /// </summary>
        private bool _IsAvailable;

        /// <summary>
        /// Indicates whether the <see cref="Album"/> is available in the current region.
        /// </summary>
        public bool IsAvailable
        {
            get
            {
                return _IsAvailable;
            }
            private set
            {
                this.SetProperty(ref _IsAvailable, value);
            }
        }

        /// <summary>
        /// Backing field.
        /// </summary>
        private string _Review;

        /// <summary>
        /// A review of the album.
        /// </summary>
        public string Review
        {
            get
            {
                return _Review;
            }
            private set
            {
                this.SetProperty(ref _Review, value);
            }
        }

        /// <summary>
        /// Backing field.
        /// </summary>
        private Track[] _Tracks;

        /// <summary>
        /// All the tracks on the album.
        /// </summary>
        public Track[] Tracks
        {
            get
            {
                return _Tracks;
            }
            private set
            {
                this.SetProperty(ref _Tracks, value);
            }
        }

        /// <summary>
        /// Backing field.
        /// </summary>
        private AlbumType _Type;

        /// <summary>
        /// The type of the <see cref="Album"/>.
        /// </summary>
        /// <seealso cref="AlbumType"/>
        public AlbumType Type
        {
            get
            {
                return _Type;
            }
            private set
            {
                this.SetProperty(ref _Type, value);
            }
        }

        /// <summary>
        /// Backing field.
        /// </summary>
        private int _Year;

        /// <summary>
        /// The year the <see cref="Album"/> was released in.
        /// </summary>
        public int Year
        {
            get
            {
                return _Year;
            }
            private set
            {
                Contract.Requires<ArgumentOutOfRangeException>(value >= 0);

                this.SetProperty(ref _Year, value);
            }
        }

        /// <summary>
        /// Initializes a new <see cref="Album"/> without loading additional metadata.
        /// </summary>
        /// <param name="session">The <see cref="Session"/> the <see cref="Album"/> is associated with.</param>
        /// <param name="handle">The handle to the underlying libspotify album object.</param>
        public Album(Session session, IntPtr handle)
            : this(session, handle, false)
        {
            Contract.Requires<ArgumentNullException>(session != null);
            Contract.Requires<ArgumentNullException>(handle != IntPtr.Zero);
        }

        /// <summary>
        /// Initializes a new <see cref="Album"/> without loading additional metadata.
        /// </summary>
        /// <param name="session">The <see cref="Session"/> the <see cref="Album"/> is associated with.</param>
        /// <param name="handle">The handle to the underlying libspotify album object.</param>
        /// <param name="loadAdditionalMetadata">Indicates whether to load additional metadata that can be obtained through the API.</param>
        public Album(Session session, IntPtr handle, bool loadAdditionalMetadata)
            : base(session)
        {
            Contract.Requires<ArgumentNullException>(session != null);
            Contract.Requires<ArgumentNullException>(handle != IntPtr.Zero);

            session.MetadataUpdateReceived += (s, e) => this.LoadMetadata();
            this.LoadMetadata();
            if (loadAdditionalMetadata)
            {
                this.LoadAdditionalMetadata();
            }
        }

        /// <summary>
        /// Increases the reference count to the underlying libspotify album object preventing it from
        /// being released on <see cref="M:Dispose"/>.
        /// </summary>
        public override void AddRef()
        {
            lock (NativeMethods.LibraryLock)
            {
                NativeMethods.sp_album_add_ref(this.Handle).ThrowIfError();
            }
        }

        /// <summary>
        /// Loads additional metadata about the album, such as its tracks, copyright information and a review.
        /// </summary>
        public void LoadAdditionalMetadata()
        {
            Session s = this.Session;
            if (s == null)
            {
                throw new InvalidOperationException("Session was null, cannot load the metadata. This is a programming bug, and SHOULD NOT happen at runtime. Please contact the developers.");
            }

            lock (NativeMethods.LibraryLock)
            {
                IntPtr handle = this.Handle;
                NativeMethods.sp_albumbrowse_create(s.Handle, handle, (h, u) =>
                {
                    lock (NativeMethods.LibraryLock)
                    {
                        try
                        {
                            if (NativeMethods.sp_albumbrowse_error(h) == Result.Ok)
                            {
                                this.Copyrights = Enumerable.Range(0, NativeMethods.sp_albumbrowse_num_copyrights(handle))
                                                            .Select(i => NativeMethods.sp_albumbrowse_copyright(handle, i).AsString())
                                                            .ToArray();
                                this.Review = NativeMethods.sp_albumbrowse_review(handle).AsString();
                                Track[] tracks = Enumerable.Range(0, NativeMethods.sp_albumbrowse_num_tracks(handle))
                                                           .Select(i => new Track(s, NativeMethods.sp_albumbrowse_track(handle, i)))
                                                           .ToArray();
                                foreach (Track t in tracks) // Increment refcount because albumbrowse-object will be disposed and we don't want the tracks to die
                                {
                                    t.AddRef();
                                }
                                this.Tracks = tracks;
                            }
                        }
                        finally
                        {
                            NativeMethods.sp_albumbrowse_release(h);
                        }
                    }
                    this.RaiseInitializationComplete();
                }, IntPtr.Zero);
            }
        }

        /// <summary>
        /// Disposes the album decrementing the reference count of the underlying libspotify album object.
        /// </summary>
        /// <param name="disposing">Indicates whether to dispose managed resources as well.</param>
        protected override void Dispose(bool disposing)
        {
            lock (NativeMethods.LibraryLock)
            {
                NativeMethods.sp_album_release(this.Handle).ThrowIfError();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Loads all the metadata.
        /// </summary>
        private void LoadMetadata()
        {
            Session s = this.Session;
            if (s == null)
            {
                throw new InvalidOperationException("Session was null, cannot load the metadata. This is a programming bug, and SHOULD NOT happen at runtime. Please contact the developers.");
            }

            lock (NativeMethods.LibraryLock)
            {
                IntPtr handle = this.Handle;
                if (this.IsLoaded = NativeMethods.sp_album_is_loaded(handle))
                {
                    this.Artist = new Artist(s, NativeMethods.sp_album_artist(handle));
                    this.Cover = new Image(s, NativeMethods.sp_album_cover(handle, ImageSize.Large));
                    this.IsAvailable = NativeMethods.sp_album_is_available(handle);
                    this.Name = NativeMethods.sp_album_name(this.Handle).AsString();
                    this.Type = NativeMethods.sp_album_type(this.Handle);
                    this.Year = NativeMethods.sp_album_year(this.Handle);

                    this.RaiseInitializationComplete();
                }
            }
        }
    }
}
