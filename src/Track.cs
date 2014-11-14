using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetify
{
    public class Track : SessionObject
    {
        private Album _Album;

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

        private Artist[] _Artists;

        public Artist[] Artists
        {
            get
            {
                return _Artists;
            }
            private set
            {
                this.SetProperty(ref _Artists, value);
            }
        }

        private TrackAvailability _Availability;

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

        private int _Disc;

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

        private TimeSpan _Duration;

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

        private int _Index;

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

        private bool _IsLoaded;

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

        private bool _IsLocal;

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

        private bool _IsStarred;

        public bool IsStarred
        {
            get
            {
                return _IsStarred;
            }
            private set
            {
                this.SetProperty(ref _IsStarred, value);
            }
        }

        private TrackOfflineStatus _OfflineStatus;

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

        public Track(Session session, IntPtr handle)
            : base(session, handle)
        {
            Contract.Requires<ArgumentNullException>(session != null);
            Contract.Requires<ArgumentNullException>(handle != IntPtr.Zero);

            session.MetadataUpdateReceived += (s, e) => this.LoadMetadata();
            this.LoadMetadata();
        }

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

        protected override void Dispose(bool disposing)
        {
            lock (this.Session.LibraryLock)
            {
                NativeMethods.sp_track_release(this.Handle);
            }
            base.Dispose(disposing);
        }

        private void LoadMetadata()
        {
            Session session = this.Session;
            if (session == null)
            {
                throw new InvalidOperationException("Metadata cannot be fetched, Session is null.");
            }

            lock (session.LibraryLock)
            {
                IntPtr handle = this.Handle;
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
                }
            }
        }
    }
}
