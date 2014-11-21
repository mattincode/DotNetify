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
    /// <remarks>
    /// This class wraps sp_artist and sp_artistbrowse.
    /// </remarks>
    public class Artist : SessionObject
    {
        /// <summary>
        /// Backing field.
        /// </summary>
        private Album[] _Albums;

        /// <summary>
        /// The albums written by the artist.
        /// </summary>
        public Album[] Albums
        {
            get
            {
                return _Albums;
            }
            private set
            {
                this.SetProperty(ref _Albums, value);
            }
        }

        /// <summary>
        /// Backing field.
        /// </summary>
        private string _Biography;

        /// <summary>
        /// The artists bio.
        /// </summary>
        public string Biography
        {
            get
            {
                return _Biography;
            }
            private set
            {
                this.SetProperty(ref _Biography, value);
            }
        }

        /// <summary>
        /// Backing field.
        /// </summary>
        private Image[] _Portraits;

        /// <summary>
        /// Gets a portrait image of the artist.
        /// </summary>
        public Image[] Portraits
        {
            get
            {
                return _Portraits;
            }
            private set
            {
                this.SetProperty(ref _Portraits, value);
            }
        }

        /// <summary>
        /// Backing field.
        /// </summary>
        private Artist[] _SimilarArtists;

        /// <summary>
        /// Gets other <see cref="Artist"/>s that are like the current artist.
        /// </summary>
        public Artist[] SimilarArtists
        {
            get
            {
                return _SimilarArtists;
            }
            private set
            {
                this.SetProperty(ref _SimilarArtists, value);
            }
        }

        /// <summary>
        /// Backing field.
        /// </summary>
        private Track[] _TophitTracks;

        /// <summary>
        /// The artists top tracks.
        /// </summary>
        public Track[] TophitTracks
        {
            get
            {
                return _TophitTracks;
            }
            private set
            {
                this.SetProperty(ref _TophitTracks, value);
            }
        }

        /// <summary>
        /// Initializes a new <see cref="Artist"/>.
        /// </summary>
        /// <param name="session">The <see cref="Session"/>.</param>
        /// <param name="handle">The handle to the underlying libspotify artist object.</param>
        public Artist(Session session, IntPtr handle)
            : this(session, handle, false)
        {
            Contract.Requires<ArgumentNullException>(session != null);
            Contract.Requires<ArgumentNullException>(handle != IntPtr.Zero);
        }

        /// <summary>
        /// Initializes a new <see cref="Artist"/>.
        /// </summary>
        /// <param name="session">The <see cref="Session"/>.</param>
        /// <param name="handle">The handle to the underlying libspotify artist object.</param>
        /// <param name="loadAdditionalMetadata">Indicates whether to load additional metadata that can be obtained through the API.</param>
        public Artist(Session session, IntPtr handle, bool loadAdditionalMetadata)
            : base(session, handle)
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
        /// Increases the reference count to the underlying libspotify artist object preventing it from being
        /// released.
        /// </summary>
        public override void AddRef()
        {
            lock (NativeMethods.LibraryLock)
            {
                NativeMethods.sp_artist_add_ref(this.Handle).ThrowIfError();
            }
        }

        /// <summary>
        /// Loads additional metadata about the artist that will not be loaded normally.
        /// </summary>
        /// <remarks>
        /// Specifically, the additional metadata will be the biography, all available portraits,
        /// similar artists, the albums and the artists most popular tracks.
        /// </remarks>
        public void LoadAdditionalMetadata()
        {
            Session s = this.Session;
            if (s == null)
            {
                throw new InvalidOperationException("Metadata cannot be loaded, session was null. This is a programming bug, and SHOULD NOT happen at runtime. Please contact the developers.");
            }

            lock (NativeMethods.LibraryLock)
            {
                IntPtr handle = this.Handle;
                NativeMethods.sp_artistbrowse_create(s.Handle, handle, ArtistBrowseType.NoTracks, (ab, u) =>
                {
                    lock (NativeMethods.LibraryLock)
                    {
                        try
                        {
                            if (NativeMethods.sp_artistbrowse_error(ab) == Result.Ok)
                            {
                                this.Albums = Enumerable.Range(0, NativeMethods.sp_artistbrowse_num_albums(ab))
                                                        .Select(i => new Album(s, NativeMethods.sp_artistbrowse_album(ab, i)))
                                                        .ToArray();
                                this.Biography = NativeMethods.sp_artistbrowse_biography(ab).AsString();
                                this.Portraits = Enumerable.Range(0, NativeMethods.sp_artistbrowse_num_portraits(ab))
                                                           .Select(i => new Image(s, NativeMethods.sp_artistbrowse_portrait(ab, i)))
                                                           .ToArray();
                                this.TophitTracks = Enumerable.Range(0, NativeMethods.sp_artistbrowse_num_tophit_tracks(ab))
                                                              .Select(i => new Track(s, NativeMethods.sp_artistbrowse_tophit_track(ab, i)))
                                                              .ToArray();
                            }
                        }
                        finally
                        {
                            NativeMethods.sp_artistbrowse_release(ab);
                        }
                    }
                }, IntPtr.Zero);
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
                NativeMethods.sp_artist_release(this.Handle).ThrowIfError();
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
                    this.Portraits = new[] { new Image(s, NativeMethods.sp_artist_portrait(handle, ImageSize.Large)) };

                    this.RaiseInitializationComplete();
                }
            }
        }
    }
}
