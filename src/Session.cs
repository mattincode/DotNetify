using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetify
{
    /// <summary>
    /// Represents a spotify session.
    /// </summary>
    /// <remarks>
    /// Currently (because of how libspotify is implemented) only one session can exist per process (which is a fugly shame!).
    /// </remarks>
    public class Session : SpotifyObject
    {
        /// <summary>
        /// The <see cref="CallbackHandler"/> representing the main event processor.
        /// </summary>
        private readonly CallbackHandler callbackHandler;

        /// <summary>
        /// Called when login has been processed.
        /// </summary>
        public event EventHandler<SpotifyErrorEventArgs> LoggedIn;

        /// <summary>
        /// Called when logout has been processed. Either called explicitly
        /// if you initialize a logout operation, or implicitly if there
        /// is a permanent connection error.
        /// </summary>
        public event EventHandler<SpotifyEventArgs> LoggedOut;

        /// <summary>
        /// Called whenever metadata has been updated.
        /// </summary>
        /// <remarks>If you have metadata cached outside of DotNetify, you should purge your caches and fetch new versions.</remarks>
        public event EventHandler<SpotifyEventArgs> MetadataUpdateReceived;

        /// <summary>
        /// Called when there is a connection error, and the library has problems
        /// reconnecting to the Spotify service. Could be called multiple times (as
        /// long as the problem is present).
        /// </summary>
        public event EventHandler<SpotifyErrorEventArgs> ConnectionError;

        /// <summary>
        /// Called when the access point wants to display a message to the user.
        /// </summary>
        public event EventHandler<TextEventArgs> MessageToUserReceived;

        /// <summary>
        /// Called when processing needs to take place on the main thread.
        /// </summary>
        /// <remarks>
        /// You need to call <see cref="ProcessEvents"/> in the main thread to get
        /// libspotify to do more work. Failure to do so may cause request timeouts,
        /// or a lost connection.
        /// </remarks>
        public event EventHandler<SpotifyEventArgs> MainThreadProcessingRequest;

        /// <summary>
        /// Called when there is decompressed audio data available.
        /// </summary>
        public event EventHandler<MusicDeliveryEventArgs> MusicDelivery;

        /// <summary>
        /// Music has been paused because an account only allows music
        /// to be played from one location simultaneously.
        /// </summary>
        public event EventHandler<SpotifyEventArgs> PlayTokenLost;

        /// <summary>
        /// Logging callback.
        /// </summary>
        public event EventHandler<TextEventArgs> LogMessageArrived;

        /// <summary>
        /// End of track. Called when the currently played track has reached its end.
        /// </summary>
        public event EventHandler<SpotifyEventArgs> EndOfTrack;

        /// <summary>
        /// Streaming error. Called when streaming cannot start or continue.
        /// </summary>
        public event EventHandler<SpotifyErrorEventArgs> StreamingError;

        /// <summary>
        /// Called after user info (anything related to <see cref="User"/>-objects) have been updated.
        /// </summary>
        public event EventHandler<SpotifyEventArgs> UserInfoUpdated;

        /// <summary>
        /// Called when audio playback should start.
        /// </summary>
        public event EventHandler<SpotifyEventArgs> PlaybackStarted;

        /// <summary>
        /// Called when audio playback should stop.
        /// </summary>
        public event EventHandler<SpotifyEventArgs> PlaybackStopped;

        /// <summary>
        /// Called when offline synchronization status is updated.
        /// </summary>
        public event EventHandler<SpotifyEventArgs> OfflineSyncStatusChanged;

        /// <summary>
        /// Callen when there was an offline error.
        /// </summary>
        public event EventHandler<SpotifyErrorEventArgs> OfflineError;

        /// <summary>
        /// Called when storable credentials have been updated, usually called when we have connected to the API.
        /// </summary>
        /// <remarks>
        /// The credentials blob is a string which contains an encrypted token that can be stored safely on disk 
        /// instead of storing plaintext passwords.
        /// </remarks>
        public event EventHandler<TextEventArgs> CredentialsBlobUpdated;

        /// <summary>
        /// Called when the connection state has updated - such as when logging in, going offline, etc.
        /// </summary>
        public event EventHandler<SpotifyEventArgs> ConnectionStateUpdated;

        /// <summary>
        /// Called when there is a scrobble error event.
        /// </summary>
        public event EventHandler<SpotifyErrorEventArgs> ScrobbleError;

        /// <summary>
        /// Called when there is a change in the private session mode.
        /// </summary>
        public event EventHandler<SessionModeChangedEventArgs> SessionModeChanged;

        /// <summary>
        /// Backing field.
        /// </summary>
        private bool _NeedsProcessing;

        /// <summary>
        /// Indicates whether the application needs to call <see cref="ProcessEvents"/> for libspotify to perform work
        /// that needs to be done on the main thread.
        /// </summary>
        public bool NeedsProcessing
        {
            get
            {
                return _NeedsProcessing;
            }
            set
            {
                this.SetProperty(ref _NeedsProcessing, value);
            }
        }

        /// <summary>
        /// Backing field.
        /// </summary>
        private Player _Player;

        /// <summary>
        /// The <see cref="Player"/> controlling the audio streaming.
        /// </summary>
        public Player Player
        {
            get
            {
                return _Player;
            }
            private set
            {
                this.SetProperty(ref _Player, value);
            }
        }

        /// <summary>
        /// Initializes a new <see cref="Session"/>.
        /// </summary>
        /// <param name="handler">
        /// A <see cref="CallbackHandler"/> that is responsible for all the processing and the return values of the libspotify callbacks.
        /// </param>
        public Session(CallbackHandler handler)
        {
            Contract.Requires<ArgumentNullException>(handler != null);

            this.callbackHandler = handler;
        }

        /// <summary>
        /// Causes libspotify to process work that needs to be done on the main thread.
        /// </summary>
        public void ProcessEvents()
        {

        }

        /// <summary>
        /// Disposes the <see cref="Session"/> releasing it from the memory.
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
