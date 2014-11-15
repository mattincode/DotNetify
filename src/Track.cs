using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetify
{
    /// <summary>
    /// Represents a piece of music.
    /// </summary>
    public class Track : SessionObject
    {
        /// <summary>
        /// Backing field.
        /// </summary>
        private Album _Album;

        /// <summary>
        /// The <see cref="Album"/> the <see cref="Track"/> was released on. See remarks.
        /// </summary>
        /// <remarks>
        /// This instance will be released when the <see cref="Track"/> is disposed.
        /// </remarks>
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
        /// Backing field.
        /// </summary>
        private Artist[] _Artists;

        /// <summary>
        /// The <see cref="Artist"/>s that have written the song. See remarks.
        /// </summary>
        /// <remarks>
        /// The <see cref="Artist"/>s will be released when the <see cref="Track"/> is disposed.
        /// </remarks>
        public Artist[] Artists
        {
            get
            {
                Artist[] artists = _Artists;
                return (artists != null) ? artists.ToArray() : null;
            }
            private set
            {
                this.SetProperty(ref _Artists, value);
            }
        }

        /// <summary>
        /// Backing field.
        /// </summary>
        private TrackAvailability _Availability;

        /// <summary>
        /// The <see cref="Track"/>s availability.
        /// </summary>
        /// <seealso cref="TrackAvailability"/>
        public TrackAvailability Availability
        {
            get
            {
                return _Availability;
            }
            private set
            {
                this.SetProperty(ref _Availability, value);
            }
        }

        /// <summary>
        /// Backing field.
        /// </summary>
        private int _Disc;

        /// <summary>
        /// The disc number the track is on.
        /// </summary>
        public int Disc
        {
            get
            {
                return _Disc;
            }
            private set
            {
                this.SetProperty(ref _Disc, value);
            }
        }

        /// <summary>
        /// Backing field.
        /// </summary>
        private TimeSpan _Duration;

        /// <summary>
        /// The length of the track.
        /// </summary>
        public TimeSpan Duration
        {
            get
            {
                return _Duration;
            }
            private set
            {
                this.SetProperty(ref _Duration, value);
            }
        }

        /// <summary>
        /// Backing field.
        /// </summary>
        private int _Index;

        /// <summary>
        /// The <see cref="Track"/>s index on the <see cref="P:Album"/>.
        /// </summary>
        public int Index
        {
            get
            {
                return _Index;
            }
            private set
            {
                this.SetProperty(ref _Index, value);
            }
        }

        /// <summary>
        /// Backing field.
        /// </summary>
        private bool _IsLoaded;

        /// <summary>
        /// Indicates whether the <see cref="Track"/>s metadata has loaded.
        /// </summary>
        public bool IsLoaded
        {
            get
            {
                return _IsLoaded;
            }
            private set
            {
                this.SetProperty(ref _IsLoaded, value);
            }
        }

        /// <summary>
        /// Backing field.
        /// </summary>
        private bool _IsLocal;

        /// <summary>
        /// Indicates whether the track is a local file.
        /// </summary>
        public bool IsLocal
        {
            get
            {
                return _IsLocal;
            }
            private set
            {
                this.SetProperty(ref _IsLocal, value);
            }
        }

        /// <summary>
        /// Backing field.
        /// </summary>
        private bool _IsPlaceholder;

        /// <summary>
        /// Indicates whether the <see cref="Track"/> is a placeholder. See remarks.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Placeholder tracks are used to store other objects than tracks in the playlist. Currently this is
        /// used in the inbox to store artists, albums and playlists.
        /// </para>
        /// <para>
        /// The metadata does not need to be loaded for this property to work.
        /// </para>
        /// </remarks>
        public bool IsPlaceholder
        {
            get
            {
                return _IsPlaceholder;
            }
            set
            {
                this.SetProperty(ref _IsPlaceholder, value);
            }
        }

        /// <summary>
        /// Backing field.
        /// </summary>
        private bool _IsStarred;

        /// <summary>
        /// Indicates whethe the user has starred the specified <see cref="Track"/>.
        /// </summary>
        public bool IsStarred
        {
            get
            {
                return _IsStarred;
            }
            set
            {
                Session s = this.Session;
                if (s == null)
                {
                    throw new InvalidOperationException("The session was null. Starring cannot be set.");
                }
                lock (NativeMethods.LibraryLock)
                {
                    using (NativeIntPtrArray nativeTracks = new NativeIntPtrArray(new[] { this.Handle }))
                    {
                        Spotify.CheckForError(NativeMethods.sp_track_set_starred(s.Handle, nativeTracks, 1, value));
                    }
                }
                this.SetProperty(ref _IsStarred, value);
            }
        }

        /// <summary>
        /// Backing field.
        /// </summary>
        private TrackOfflineStatus _OfflineStatus;

        /// <summary>
        /// The <see cref="Track"/>s offline status.
        /// </summary>
        /// <seealso cref="TrackOfflineStatus"/>
        public TrackOfflineStatus OfflineStatus
        {
            get
            {
                return _OfflineStatus;
            }
            private set
            {
                this.SetProperty(ref _OfflineStatus, value);
            }
        }

        /// <summary>
        /// Backing field.
        /// </summary>
        private int _Popularity;

        /// <summary>
        /// The <see cref="Track"/>s popularity, from 0-100. If this is 0, popularity is undefined.
        /// </summary>
        public int Popularity
        {
            get
            {
                return _Popularity;
            }
            private set
            {
                Contract.Requires<ArgumentOutOfRangeException>(value >= 0 && value <= 100);

                this.SetProperty(ref _Popularity, value);
            }
        }

        /// <summary>
        /// Initializes a new <see cref="Track"/>.
        /// </summary>
        /// <param name="session">The <see cref="Session"/> that created the <see cref="Track"/>.</param>
        /// <param name="handle">The handle to the libspotify track object.</param>
        public Track(Session session, IntPtr handle)
            : base(session, handle)
        {
            Contract.Requires<ArgumentNullException>(session != null);
            Contract.Requires<ArgumentNullException>(handle != IntPtr.Zero);

            session.MetadataUpdateReceived += (s, e) => this.LoadMetadata();
            this.LoadMetadata();
        }

        /// <summary>
        /// Increments the <see cref="Track"/>s reference count.
        /// </summary>
        public override void AddRef()
        {
            lock (NativeMethods.LibraryLock)
            {
                Spotify.CheckForError(NativeMethods.sp_track_add_ref(this.Handle));
            }
        }

        /// <summary>
        /// Plays the <see cref="Track"/>. See remarks.
        /// </summary>
        /// <remarks>
        /// This will only load the <see cref="Track"/> into the <see cref="Player"/>. It will not change
        /// the play / pause-status of the <see cref="Player"/>. So if the <see cref="Player.IsPlaying"/> is not
        /// <c>true</c>, the track will not be hearable.
        /// </remarks>
        public void Play()
        {
            Session session = this.Session;
            if (session == null)
            {
                throw new InvalidOperationException("Track cannot be played, Session is null.");
            }
            Player player = session.Player;
            if (player == null)
            {
                throw new InvalidOperationException("Track cannot be played, player is null.");
            }

            player.Load(this);
        }
        
        /// <summary>
        /// Disposes the <see cref="Track"/> releasing the underlying libspotify track object.
        /// </summary>
        /// <param name="disposing">Indicates whether to release managed resources as well.</param>
        protected override void Dispose(bool disposing)
        {
            lock (NativeMethods.LibraryLock)
            {
                NativeMethods.sp_track_release(this.Handle);
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Loads the <see cref="Track"/>s metadata.
        /// </summary>
        private void LoadMetadata()
        {
            Session session = this.Session;
            if (session == null)
            {
                throw new InvalidOperationException("Metadata cannot be fetched, Session is null. This is a programming bug, and SHOULD NOT happen at runtime. Please contact the developers.");
            }

            lock (NativeMethods.LibraryLock)
            {
                IntPtr handle = this.Handle;
                this.IsPlaceholder = NativeMethods.sp_track_is_placeholder(this.Handle);
                if (this.IsLoaded = NativeMethods.sp_track_is_loaded(handle))
                {
                    this.Album = new Album(session, NativeMethods.sp_track_album(handle));
                    this.Artists = Enumerable.Range(0, NativeMethods.sp_track_num_artists(handle))
                                             .Select(i => new Artist(session, NativeMethods.sp_track_artist(handle, i))).ToArray();
                    this.Availability = NativeMethods.sp_track_get_availability(session.Handle, handle);
                    this.Disc = NativeMethods.sp_track_disc(handle);
                    this.Duration = TimeSpan.FromMilliseconds(NativeMethods.sp_track_duration(handle));
                    this.Index = NativeMethods.sp_track_index(handle);
                    this.IsLocal = NativeMethods.sp_track_is_local(session.Handle, handle);
                    this.IsStarred = NativeMethods.sp_track_is_starred(session.Handle, handle);
                    this.Name = NativeMethods.sp_track_name(handle).AsString();
                    this.OfflineStatus = NativeMethods.sp_track_offline_get_status(handle);
                    this.Popularity = NativeMethods.sp_track_popularity(handle);
                }
            }
        }
    }
}
