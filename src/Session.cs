using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.InteropServices;
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
        /// The <see cref="SessionConfig"/> used to initialize the <see cref="Session"/>.
        /// </summary>
        private readonly SessionConfig config;

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
        /// Called when there was an offline error.
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
        private object _LibraryLock = new object();

        /// <summary>
        /// The lock object used to lock access to the library functions.
        /// </summary>
        internal object LibraryLock
        {
            get
            {
                return _LibraryLock;
            }
            private set
            {
                this.SetProperty(ref _LibraryLock, value);
            }
        }

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
            private set
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
        public Session(SessionConfig sessionConfig)
        {
            Contract.Requires<ArgumentException>(sessionConfig.CallbackHandler != null);

            this.config = sessionConfig;

            NativeMethods.sp_session_callbacks callbacks = new NativeMethods.sp_session_callbacks();
            callbacks.connection_error = new connection_error(this.ConnectionErrorCallback);
            callbacks.connectionstate_updated = new connectionstate_updated(this.ConnectionStateUpdatedCallback);
            callbacks.credentials_blob_updated = new credentials_blob_updated(this.CredentialsBlobUpdatedCallback);
            callbacks.end_of_track = new end_of_track(this.EndOfTrackCallback);
            callbacks.get_audio_buffer_stats = new get_audio_buffer_stats(this.GetAudioBufferStatsCallback);
            callbacks.log_message = new log_message(this.LogMessageCallback);
            callbacks.logged_in = new logged_in(this.LoggedInCallback);
            callbacks.logged_out = new logged_out(this.LoggedOutCallback);
            callbacks.message_to_user = new message_to_user(this.MessageToUserCallback);
            callbacks.metadata_updated = new metadata_updated(this.MetadataUpdatedCallback);
            callbacks.music_delivery = new music_delivery(this.MusicDeliveryCallback);
            callbacks.notify_main_thread = new notify_main_thread(this.NotifyMainThreadCallback);
            callbacks.offline_error = new offline_error(this.OfflineErrorCallback);
            callbacks.offline_status_updated = new offline_status_updated(this.OfflineStatusUpdatedCallback);
            callbacks.play_token_lost = new play_token_lost(this.PlayTokenLostCallback);
            callbacks.private_session_mode_changed = new private_session_mode_changed(this.PrivateSessionModeChangedCallback);
            callbacks.scrobble_error = new scrobble_error(this.ScrobbleErrorCallback);
            callbacks.start_playback = new start_playback(this.StartPlaybackCallback);
            callbacks.stop_playback = new stop_playback(this.StopPlaybackCallback);
            callbacks.streaming_error = new streaming_error(this.StreamingErrorCallback);
            callbacks.userinfo_updated = new userinfo_updated(this.UserinfoUpdatedCallback);

            using (NativeByteArray nativeKey = new NativeByteArray(sessionConfig.ApplicationKey))
            using (NativeStruct<NativeMethods.sp_session_callbacks> nativeCallbacks = new NativeStruct<NativeMethods.sp_session_callbacks>(callbacks))
            using (NativeString nativeCacheLocation = new NativeString(sessionConfig.CacheLocation))
            using (NativeString nativeDeviceId = new NativeString(sessionConfig.DeviceId))
            using (NativeString nativeProxy = new NativeString(sessionConfig.ProxyConfig.Url))
            using (NativeString nativeProxyPassword = new NativeString(sessionConfig.ProxyConfig.Password))
            using (NativeString nativeProxyUsername = new NativeString(sessionConfig.ProxyConfig.Username))
            using (NativeString nativeSettingsLocation = new NativeString(sessionConfig.SettingsLocation))
            using (NativeString nativeTracefile = new NativeString(sessionConfig.TraceFile))
            using (NativeString nativeUserAgent = new NativeString(sessionConfig.UserAgent))
            using (NativeByteArray nativeUserData = new NativeByteArray(sessionConfig.UserData))
            {
                NativeMethods.sp_session_config config = new NativeMethods.sp_session_config();

                config.api_version = sessionConfig.ApiVersion;
                config.application_key = nativeKey;
                config.application_key_size = (UIntPtr)nativeKey.Length;
                config.cache_location = nativeCacheLocation;
                config.callbacks = nativeCallbacks;
                config.compress_playlists = sessionConfig.CompressPlaylists;
                config.device_id = nativeDeviceId;
                config.dont_save_metadata_for_playlists = sessionConfig.DontSaveMetadataForPlaylists;
                config.initially_unload_playlists = sessionConfig.DontLoadPlaylistsOnStartup;
                config.proxy = nativeProxy;
                config.proxy_password = nativeProxyPassword;
                config.proxy_username = nativeProxyUsername;
                config.settings_location = nativeSettingsLocation;
                config.tracefile = nativeTracefile;
                config.user_agent = nativeUserAgent;
                config.userdata = nativeUserData;

                IntPtr sessionHandle = IntPtr.Zero;
                NativeMethods.sp_session_create(ref config, ref sessionHandle);
                this.Handle = sessionHandle;
            }

            this.Player = new Player(this);
        }

        /// <summary>
        /// Causes libspotify to process work that needs to be done on the main thread.
        /// </summary>
        public int ProcessEvents()
        {
            int nextRequest = 0;
            Spotify.CheckForError(NativeMethods.sp_session_process_events(this.Handle, ref nextRequest));
            return nextRequest;
        }

        /// <summary>
        /// Disposes the <see cref="Session"/> releasing it from the memory.
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        private void ConnectionErrorCallback(IntPtr session, Error error)
        {
            if (this.Handle == session)
            {
                this.config.CallbackHandler.ConnectionError(this, error);
                this.Raise(this.ConnectionError, error);
            }
        }

        private void ConnectionStateUpdatedCallback(IntPtr session)
        {
            if (this.Handle == session)
            {
                this.config.CallbackHandler.ConnectionStateUpdated(this);
                this.Raise(this.ConnectionStateUpdated);
            }
        }

        private void CredentialsBlobUpdatedCallback(IntPtr session, IntPtr blob)
        {
            if (this.Handle == session)
            {
                string blobString = blob.AsString();
                this.config.CallbackHandler.CredentialsBlobUpdated(this, blobString);
                this.Raise(this.CredentialsBlobUpdated, blobString);
            }
        }

        private void EndOfTrackCallback(IntPtr session)
        {
            if (this.Handle == session)
            {
                this.config.CallbackHandler.EndOfTrack(this);
                this.Raise(this.EndOfTrack);
            }
        }

        private void GetAudioBufferStatsCallback(IntPtr session, ref AudioBufferStatistics stats)
        {
            if (this.Handle == session)
            {
                this.config.CallbackHandler.GetAudioBufferStats(this, ref stats);
                // No event here, since its more of a request from the library than a push of new information
            }
        }

        private void LoggedInCallback(IntPtr session, Error error)
        {
            if (this.Handle == session)
            {
                this.config.CallbackHandler.LoggedIn(this, error);
                this.Raise(this.LoggedIn, error);
            }
        }

        private void LoggedOutCallback(IntPtr session)
        {
            if (this.Handle == session)
            {
                this.config.CallbackHandler.LoggedOut(this);
                this.Raise(this.LoggedOut);
            }
        }

        private void LogMessageCallback(IntPtr session, IntPtr data)
        {
            if (this.Handle == session)
            {
                string logData = data.AsString();
                this.config.CallbackHandler.LogMessage(this, logData);
                this.Raise(this.LogMessageArrived, logData);
            }
        }

        private void MessageToUserCallback(IntPtr session, IntPtr message)
        {
            if (this.Handle == session)
            {
                string messageString = message.AsString();
                this.config.CallbackHandler.MessageToUser(this, messageString);
                this.Raise(this.MessageToUserReceived, messageString);
            }
        }

        private void MetadataUpdatedCallback(IntPtr session)
        {
            if (this.Handle == session)
            {
                this.config.CallbackHandler.MetadataUpdateReceived(this);
                this.Raise(this.MetadataUpdateReceived);
            }
        }

        private int MusicDeliveryCallback(IntPtr session, ref AudioFormat format, IntPtr frames, int frameCount)
        {
            if (this.Handle == session)
            {
                int result = this.config.CallbackHandler.MusicDelivery(this, format, frames, frameCount);
                EventHandler<MusicDeliveryEventArgs> handler = this.MusicDelivery;
                if (handler != null)
                {
                    handler(this, new MusicDeliveryEventArgs(this, new MusicPackage(format, frames, frameCount)));
                }
                return result;
            }
            return frameCount;
        }

        private void NotifyMainThreadCallback(IntPtr session)
        {
            if (this.Handle == session)
            {
                this.config.CallbackHandler.NotifyMainThread(this);
                this.Raise(this.MainThreadProcessingRequest);
                this.NeedsProcessing = true;
            }
        }

        private void OfflineErrorCallback(IntPtr session, Error error)
        {
            if (this.Handle == session)
            {
                this.config.CallbackHandler.OfflineError(this, error);
                this.Raise(this.OfflineError, error);
            }
        }

        private void OfflineStatusUpdatedCallback(IntPtr session)
        {
            if (this.Handle == session)
            {
                this.config.CallbackHandler.OfflineSynchronizationStatusChanged(this);
                this.Raise(this.OfflineSyncStatusChanged);
            }
        }

        private void PlayTokenLostCallback(IntPtr session)
        {
            if (this.Handle == session)
            {
                this.config.CallbackHandler.PlayTokenLost(this);
                this.Raise(this.PlayTokenLost);
            }
        }

        private void PrivateSessionModeChangedCallback(IntPtr session, [MarshalAs(UnmanagedType.I1)] bool isPrivate)
        {
            if (this.Handle == session)
            {
                this.config.CallbackHandler.SessionModeChanged(this, isPrivate);
                EventHandler<SessionModeChangedEventArgs> handler = this.SessionModeChanged;
                if (handler != null)
                {
                    handler(this, new SessionModeChangedEventArgs(this, isPrivate));
                }
            }
        }

        private void ScrobbleErrorCallback(IntPtr session, Error error)
        {
            if (this.Handle == session)
            {
                this.config.CallbackHandler.ScrobbleError(this, error);
                this.Raise(this.ScrobbleError, error);
            }
        }

        private void StartPlaybackCallback(IntPtr session)
        {
            if (this.Handle == session)
            {
                this.config.CallbackHandler.StartPlayback(this);
                this.Raise(this.PlaybackStarted);
            }
        }

        private void StopPlaybackCallback(IntPtr session)
        {
            if (this.Handle == session)
            {
                this.config.CallbackHandler.StopPlayback(this);
                this.Raise(this.PlaybackStopped);
            }
        }

        private void StreamingErrorCallback(IntPtr session, Error error)
        {
            if (this.Handle == session)
            {
                this.config.CallbackHandler.StreamingError(this, error);
                this.Raise(this.StreamingError, error);
            }
        }

        private void UserinfoUpdatedCallback(IntPtr session)
        {
            if (this.Handle == session)
            {
                this.config.CallbackHandler.UserinfoUpdated(this);
                this.Raise(this.UserInfoUpdated);
            }
        }

        private void Raise(EventHandler<SpotifyEventArgs> handler)
        {
            if (handler != null)
            {
                handler(this, new SpotifyEventArgs(this));
            }
        }

        private void Raise(EventHandler<SpotifyErrorEventArgs> handler, Error error)
        {
            if (handler != null)
            {
                handler(this, new SpotifyErrorEventArgs(this, error));
            }
        }

        private void Raise(EventHandler<TextEventArgs> handler, string text)
        {
            if (handler != null)
            {
                handler(this, new TextEventArgs(this, text));
            }
        }
    }
}
