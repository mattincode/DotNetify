using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetify
{
    public class Player : ObservableObject
    {
        private bool _IsPlaying;

        public bool IsPlaying
        {
            get
            {
                return _IsPlaying;
            }
            private set
            {
                this.SetProperty(ref _IsPlaying, value);
            }
        }

        private Session _Session;

        public Session Session
        {
            get
            {
                return _Session;
            }
            private set
            {
                Contract.Requires<ArgumentNullException>(value != null);

                this.SetProperty(ref _Session, value);
            }
        }

        public Player(Session s)
        {
            Contract.Requires<ArgumentNullException>(s != null);

            this.Session = s;
        }

        public void Load(Track track)
        {
            Contract.Requires<ArgumentNullException>(track != null);

            Session s = this.GetSession();
            lock (s.LibraryLock)
            {
                Spotify.CheckForError(NativeMethods.sp_session_player_load(s.Handle, track.Handle));
            }
        }

        public void Play()
        {
            Session s = this.GetSession();
            lock (s.LibraryLock)
            {
                Spotify.CheckForError(NativeMethods.sp_session_player_play(s.Handle, true));
                this.IsPlaying = true;
            }
        }

        public void Pause()
        {
            Session s = this.GetSession();
            lock (s.LibraryLock)
            {
                Spotify.CheckForError(NativeMethods.sp_session_player_play(s.Handle, false));
                this.IsPlaying = false;
            }
        }

        public void Seek(int offset)
        {
            Contract.Requires<ArgumentOutOfRangeException>(offset >= 0);

            Session s = this.GetSession();
            lock (s.LibraryLock)
            {
                Spotify.CheckForError(NativeMethods.sp_session_player_seek(s.Handle, offset));
            }
        }

        public void Stop()
        {
            Session s = this.GetSession();
            lock (s.LibraryLock)
            {
                Spotify.CheckForError(NativeMethods.sp_session_player_unload(s.Handle));
                this.IsPlaying = false;
            }
        }

        private Session GetSession()
        {
            Session s = this.Session;
            if (s == null)
            {
                throw new InvalidOperationException("Session is null, operation cannot be performed.");
            }
            return s;
        }
    }
}
