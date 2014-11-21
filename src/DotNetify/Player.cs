using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetify
{
    /// <summary>
    /// Represents the controls
    /// </summary>
    public class Player : ObservableObject
    {
        /// <summary>
        /// Backing field.
        /// </summary>
        private bool _IsPlaying;

        /// <summary>
        /// Indicates whether the player is playing.
        /// </summary>
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

        /// <summary>
        /// Backing field.
        /// </summary>
        private Session _Session;

        /// <summary>
        /// The <see cref="Session"/> the <see cref="Player"/> is associated with.
        /// </summary>
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

        /// <summary>
        /// Initializes a new <see cref="Player"/> from the specified <see cref="Session"/>.
        /// </summary>
        /// <param name="s">The <see cref="Session"/> to initialize from.</param>
        public Player(Session s)
        {
            Contract.Requires<ArgumentNullException>(s != null);

            this.Session = s;
        }

        /// <summary>
        /// Loads the specified <see cref="Track"/> into the <see cref="Player"/>. See remarks.
        /// </summary>
        /// <remarks>
        /// It will be played immediately, if <see cref="P:IsPlaying"/>is set to <c>true</c>.
        /// </remarks>
        /// <param name="track">The <see cref="Track"/> to play.</param>
        public void Load(Track track)
        {
            Contract.Requires<ArgumentNullException>(track != null);

            lock (NativeMethods.LibraryLock)
            {
                NativeMethods.sp_session_player_load(this.Session.Handle, track.Handle).ThrowIfError();
            }
        }

        /// <summary>
        /// Starts the playback.
        /// </summary>
        public void Play()
        {
            lock (NativeMethods.LibraryLock)
            {
                NativeMethods.sp_session_player_play(this.Session.Handle, true).ThrowIfError();
                this.IsPlaying = true;
            }
        }

        /// <summary>
        /// Pauses playback.
        /// </summary>
        public void Pause()
        {
            lock (NativeMethods.LibraryLock)
            {
                NativeMethods.sp_session_player_play(this.Session.Handle, false).ThrowIfError();
                this.IsPlaying = false;
            }
        }

        /// <summary>
        /// Preloads the specified <see cref="Track"/> in the background.
        /// </summary>
        /// <remarks>
        /// This is especially useful for playlists to enable seamless playback of multiple tracks. Prefetch
        /// the next track while the current track plays and playback won't halt.
        /// </remarks>
        /// <param name="track">The <see cref="Track"/> to prefetch.</param>
        public void Prefetch(Track track)
        {
            Contract.Requires<ArgumentNullException>(track != null);

            lock (NativeMethods.LibraryLock)
            {
                NativeMethods.sp_session_player_prefetch(this.Session.Handle, track.Handle).ThrowIfError();
            }
        }

        /// <summary>
        /// Scrolls to a position inside the current track.
        /// </summary>
        /// <param name="offset">The offset inside the <see cref="Track"/>.</param>
        public void Seek(TimeSpan offset)
        {
            Contract.Requires<ArgumentOutOfRangeException>(offset >= TimeSpan.Zero);

            lock (NativeMethods.LibraryLock)
            {
                NativeMethods.sp_session_player_seek(this.Session.Handle, offset.Milliseconds).ThrowIfError();
            }
        }

        /// <summary>
        /// Stops playback.
        /// </summary>
        public void Stop()
        {
            lock (NativeMethods.LibraryLock)
            {
                NativeMethods.sp_session_player_unload(this.Session.Handle).ThrowIfError();
                this.IsPlaying = false;
            }
        }
    }
}
