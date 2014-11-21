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
    /// Currently (because of how libspotify is implemented) only one session can exist per process. Should libspotify be able
    /// to deal with multiple sessions per process this library will be adapted. (Probably doesn't even need any changes, it does
    /// not care about how many sessions are present at a time)
    /// </remarks>
    public class Session : SpotifyObject
    {
        /// <summary>
        /// The <see cref="CallbackHandler"/> handling the libspotify callbacks.
        /// </summary>
        private readonly CallbackHandler callbackHandler;

        /// <summary>
        /// Called when there is a connection error, and the library has problems
        /// reconnecting to the Spotify service. Could be called multiple times (as
        /// long as the problem is present).
        /// </summary>
        public event EventHandler<SpotifyResultEventArgs> ConnectionError;

        /// <summary>
        /// Called when the connection state has updated - such as when logging in, going offline, etc.
        /// </summary>
        public event EventHandler<ConnectionStateUpdatedEventArgs> ConnectionStateUpdated;

        /// <summary>
        /// Called when storable credentials have been updated, usually called when we have connected to the API.
        /// </summary>
        /// <remarks>
        /// The credentials blob is a string which contains an encrypted token that can be stored safely on disk 
        /// instead of storing plaintext passwords.
        /// </remarks>
        public event EventHandler<TextEventArgs> CredentialsBlobUpdated;

        /// <summary>
        /// Called when login has been processed.
        /// </summary>
        public event EventHandler<LoggedInEventArgs> LoggedIn;

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
        public event EventHandler<SpotifyResultEventArgs> StreamingError;

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
        public event EventHandler<OfflineSyncStatusChangedEventArgs> OfflineSyncStatusChanged;

        /// <summary>
        /// Called when there was an offline error.
        /// </summary>
        public event EventHandler<SpotifyResultEventArgs> OfflineError;

        /// <summary>
        /// Called when there is a scrobble error event.
        /// </summary>
        public event EventHandler<SpotifyResultEventArgs> ScrobbleError;

        /// <summary>
        /// Called when there is a change in the private session mode.
        /// </summary>
        public event EventHandler<SessionModeChangedEventArgs> SessionModeChanged;

        /// <summary>
        /// Backing field.
        /// </summary>
        private int _CacheSize;

        /// <summary>
        /// The maximum cache size in megabytes.
        /// </summary>
        public int CacheSize
        {
            get
            {
                return _CacheSize;
            }
            private set
            {
                Contract.Requires<ArgumentOutOfRangeException>(value >= 0);

                this.SetProperty(ref _CacheSize, value);
            }
        }

        /// <summary>
        /// Backing field.
        /// </summary>
        private ConnectionState _ConnectionState;

        /// <summary>
        /// The connection state of the specified session.
        /// </summary>
        public ConnectionState ConnectionState
        {
            get
            {
                return _ConnectionState;
            }
            private set
            {
                this.SetProperty(ref _ConnectionState, value);
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
        /// Backing field.
        /// </summary>
        private User _User;

        /// <summary>
        /// Fetches the currently logged in user.
        /// </summary>
        public User User // Will be set in LoggedIn callback
        {
            get
            {
                return _User;
            }
            private set
            {
                this.SetProperty(ref _User, value);
            }
        }

        /// <summary>
        /// Initializes a new <see cref="Session"/>.
        /// </summary>
        public Session(SessionConfig sessionConfig)
        {
            Contract.Requires<ArgumentNullException>(sessionConfig != null);
            Contract.Requires<ArgumentException>(sessionConfig.CallbackHandler != null);

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
            {
                NativeMethods.sp_session_config config = new NativeMethods.sp_session_config();

                config.api_version = Spotify.ApiVersion;
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

                IntPtr sessionHandle = IntPtr.Zero;
                lock (NativeMethods.LibraryLock)
                {
                    NativeMethods.sp_session_create(ref config, ref sessionHandle).ThrowIfError();
                }
                this.Handle = sessionHandle;
            }

            this.CacheSize = sessionConfig.CacheSize;
            this.callbackHandler = sessionConfig.CallbackHandler;
            this.Player = new Player(this);
        }

        /// <summary>
        /// Flushes the caches.
        /// </summary>
        /// <remarks>
        /// This will make libspotify write all data that is meant to be stored on disk to the disk immediately. libspotify does 
        /// this periodically by itself and also on logout. So under normal conditions this should never need to be used.
        /// </remarks>
        public void Flush()
        {
            lock (NativeMethods.LibraryLock)
            {
                NativeMethods.sp_session_flush_caches(this.Handle);
            }
        }

        /// <summary>
        /// Removes stored credentials in libspotify.
        /// </summary>
        public void ForgetMe()
        {
            lock (NativeMethods.LibraryLock)
            {
                NativeMethods.sp_session_forget_me(this.Handle);
            }
        }

        /// <summary>
        /// Logs in the specified username/password combo. This initiates the login in the background.
        /// A callback is called when login is complete.
        /// </summary>
        /// <param name="username">The username to log in.</param>
        /// <param name="password">The password for the specified username.</param>
        /// <param name="rememberMe">If set, the username / password will be remembered by libspotify.</param>
        /// <param name="blob">
        /// If you have received a blob in the <see cref="E:CredentialsBlobUpdated"/>-event you can pass this here instead of password.
        /// </param>
        public void Login(string username, string password, bool rememberMe, string blob = null)
        {
            Contract.Requires<ArgumentNullException>(username != null);
            Contract.Requires<ArgumentNullException>(password != null || blob != null);

            using (NativeString nativeUsername = new NativeString(username))
            using (NativeString nativePassword = new NativeString(password))
            {
                this.Login(nativeUsername, nativePassword, rememberMe, blob);
            }
        }

        /// <summary>
        /// Logs in the specified username/password combo. This initiates the login in the background.
        /// A callback is called when login is complete.
        /// </summary>
        /// <remarks>
        /// When using this overload instead of the other one accepting strings you are able to pass in native arrays.
        /// The benefit of those arrays is that they can be freed / cleared out directly after their usage. Thus, the password
        /// will not be left in application memory any longer than needed.
        /// </remarks>
        /// <param name="utf8Password">A pointer to the UTF-8 encoded password.</param>
        /// <param name="utf8Username">A pointer to the UTF-8 encoded username.</param>
        /// <param name="rememberMe">If set, the username / password will be remembered by libspotify.</param>
        /// <param name="blob">
        /// If you have received a blob in the <see cref="E:CredentialsBlobUpdated"/>-event you can pass this here instead of password.
        /// </param>
        public void Login(IntPtr utf8Username, IntPtr utf8Password, bool rememberMe, string blob = null)
        {
            Contract.Requires<ArgumentException>(utf8Username != IntPtr.Zero);
            Contract.Requires<ArgumentException>(utf8Password != IntPtr.Zero);

            using (NativeString nativeBlob = new NativeString(blob))
            {
                lock (NativeMethods.LibraryLock)
                {
                    NativeMethods.sp_session_login(this.Handle, utf8Username, utf8Password, rememberMe, nativeBlob).ThrowIfError();
                }
            }
        }

        /// <summary>
        /// Logs out the currently logged in user.
        /// </summary>
        /// <remarks>
        /// Always call this before terminating the application and libspotify is currently
        /// logged in. Otherwise, the settings and cache may be lost.
        /// 
        /// <u>Will automatically be called on <see cref="M:Dispose"/>.</u> If you dispose the <see cref="Session"/>
        /// correctly, this will be called and everything will be safe.
        /// </remarks>
        public void Logout()
        {
            lock (NativeMethods.LibraryLock)
            {
                NativeMethods.sp_session_logout(this.Handle).ThrowIfError();
            }
        }

        /// <summary>
        /// Causes libspotify to process work that needs to be done on the main thread.
        /// </summary>
        public int ProcessEvents()
        {
            lock (NativeMethods.LibraryLock)
            {
                int nextRequest = 0;
                NativeMethods.sp_session_process_events(this.Handle, ref nextRequest).ThrowIfError();
                return nextRequest;
            }
        }

        /// <summary>
        /// Log in the remembered user if last user that logged in logged in with remember me-flag set.
        /// </summary>
        /// <exception cref="InvalidOperationException">No stored credentials found to relogin.</exception>
        public void Relogin()
        {
            if (this.TryRelogin())
            {
                throw new InvalidOperationException("There were no credentials to relogin from, log in manually.");
            }
        }

        /// <summary>
        /// Attempts to log in the remembered user and indicates success or failure.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the relogin was successfull or <c>false</c> if not. In those cases request the username/password-combination
        /// from the user and log in regularly via <see cref="M:Login"/>.
        /// </returns>
        public bool TryRelogin()
        {
            lock (NativeMethods.LibraryLock)
            {
                return (NativeMethods.sp_session_relogin(this.Handle) == Result.Ok);
            }
        }

        /// <summary>
        /// Disposes the <see cref="Session"/> releasing it from the memory.
        /// </summary>
        /// <param name="disposing">Indicates whether to dispose managed resources as well.</param>
        protected override void Dispose(bool disposing)
        {
            this.Logout();
            lock (NativeMethods.LibraryLock)
            {
                NativeMethods.sp_session_release(this.Handle).ThrowIfError();
            }
            base.Dispose(disposing);
        }

        #region Callbacks

        private void ConnectionErrorCallback(IntPtr session, Result error)
        {
            if (this.Handle == session)
            {
                this.callbackHandler.ConnectionError(this, error);
                this.Raise(this.ConnectionError, error);
            }
        }

        private void ConnectionStateUpdatedCallback(IntPtr session)
        {
            if (this.Handle == session)
            {
                ConnectionState state;
                lock (NativeMethods.LibraryLock)
                {
                    state = NativeMethods.sp_session_connectionstate(this.Handle);
                }

                this.callbackHandler.ConnectionStateUpdated(this, state);
                this.ConnectionState = state;
                EventHandler<ConnectionStateUpdatedEventArgs> handler = this.ConnectionStateUpdated;
                if (handler != null)
                {
                    handler(this, new ConnectionStateUpdatedEventArgs(this, state));
                }
            }
        }

        private void CredentialsBlobUpdatedCallback(IntPtr session, IntPtr blob)
        {
            if (this.Handle == session)
            {
                string blobString = blob.AsString();
                this.callbackHandler.CredentialsBlobUpdated(this, blobString);
                this.Raise(this.CredentialsBlobUpdated, blobString);
            }
        }

        private void EndOfTrackCallback(IntPtr session)
        {
            if (this.Handle == session)
            {
                this.callbackHandler.EndOfTrack(this);
                this.Raise(this.EndOfTrack);
            }
        }

        private void GetAudioBufferStatsCallback(IntPtr session, ref AudioBufferStatistics stats)
        {
            if (this.Handle == session)
            {
                stats = this.callbackHandler.GetAudioBufferStats(this);
                // No event here, since its more of a request from the library than a push of new information
            }
        }

        private void LoggedInCallback(IntPtr session, Result error)
        {
            if (this.Handle == session)
            {
                User user;
                lock (NativeMethods.LibraryLock)
                {
                    user = new User(this, NativeMethods.sp_session_user(this.Handle));
                }
                this.callbackHandler.LoggedIn(this, error, user);
                this.User = user;
                EventHandler<LoggedInEventArgs> handler = this.LoggedIn;
                if (handler != null)
                {
                    handler(this, new LoggedInEventArgs(this, error, user));
                }
            }
        }

        private void LoggedOutCallback(IntPtr session)
        {
            if (this.Handle == session)
            {
                this.callbackHandler.LoggedOut(this);
                this.User = null;
                this.Raise(this.LoggedOut);
            }
        }

        private void LogMessageCallback(IntPtr session, IntPtr data)
        {
            if (this.Handle == session)
            {
                string logData = data.AsString();
                this.callbackHandler.LogMessage(this, logData);
                this.Raise(this.LogMessageArrived, logData);
            }
        }

        private void MessageToUserCallback(IntPtr session, IntPtr message)
        {
            if (this.Handle == session)
            {
                string messageString = message.AsString();
                this.callbackHandler.MessageToUser(this, messageString);
                this.Raise(this.MessageToUserReceived, messageString);
            }
        }

        private void MetadataUpdatedCallback(IntPtr session)
        {
            if (this.Handle == session)
            {
                this.callbackHandler.MetadataUpdateReceived(this);
                this.Raise(this.MetadataUpdateReceived);
            }
        }

        private int MusicDeliveryCallback(IntPtr session, ref AudioFormat format, IntPtr frames, int frameCount)
        {
            if (this.Handle == session)
            {
                MusicPacket packet = new MusicPacket(format, frames, frameCount);
                int result = this.callbackHandler.MusicDelivery(this, packet);
                EventHandler<MusicDeliveryEventArgs> handler = this.MusicDelivery;
                if (handler != null)
                {
                    handler(this, new MusicDeliveryEventArgs(this, packet));
                }
                return result;
            }
            return frameCount;
        }

        private void NotifyMainThreadCallback(IntPtr session)
        {
            if (this.Handle == session)
            {
                this.callbackHandler.NotifyMainThread(this);
                this.Raise(this.MainThreadProcessingRequest);
                this.NeedsProcessing = true;
            }
        }

        private void OfflineErrorCallback(IntPtr session, Result error)
        {
            if (this.Handle == session)
            {
                this.callbackHandler.OfflineError(this, error);
                this.Raise(this.OfflineError, error);
            }
        }

        private void OfflineStatusUpdatedCallback(IntPtr session)
        {
            if (this.Handle == session)
            {
                IntPtr statusPtr = IntPtr.Zero;
                try
                {
                    statusPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(NativeMethods.sp_offline_sync_status)));
                    bool isSyncing;
                    lock (NativeMethods.LibraryLock)
                    {
                        isSyncing = NativeMethods.sp_offline_sync_get_status(this.Handle, statusPtr);
                    }
                    if (isSyncing)
                    {
                        NativeMethods.sp_offline_sync_status nativeSyncStatus = (NativeMethods.sp_offline_sync_status)Marshal.PtrToStructure(statusPtr, typeof(NativeMethods.sp_offline_sync_status));
                        OfflineSyncStatus syncStatus = new OfflineSyncStatus(
                            nativeSyncStatus.queued_tracks, nativeSyncStatus.queued_bytes, nativeSyncStatus.done_tracks,
                            nativeSyncStatus.done_bytes, nativeSyncStatus.copied_tracks, nativeSyncStatus.copied_bytes,
                            nativeSyncStatus.willnotcopy_tracks, nativeSyncStatus.error_tracks,
                            nativeSyncStatus.syncing
                        );

                        this.callbackHandler.OfflineSynchronizationStatusChanged(this, syncStatus);
                        EventHandler<OfflineSyncStatusChangedEventArgs> handler = this.OfflineSyncStatusChanged;
                        if (handler != null)
                        {
                            handler(this, new OfflineSyncStatusChangedEventArgs(this, syncStatus));
                        }
                    }
                }
                finally
                {
                    Marshal.FreeHGlobal(statusPtr);
                }
            }
        }

        private void PlayTokenLostCallback(IntPtr session)
        {
            if (this.Handle == session)
            {
                this.callbackHandler.PlayTokenLost(this);
                this.Raise(this.PlayTokenLost);
            }
        }

        private void PrivateSessionModeChangedCallback(IntPtr session, [MarshalAs(UnmanagedType.I1)] bool isPrivate)
        {
            if (this.Handle == session)
            {
                this.callbackHandler.SessionModeChanged(this, isPrivate);
                EventHandler<SessionModeChangedEventArgs> handler = this.SessionModeChanged;
                if (handler != null)
                {
                    handler(this, new SessionModeChangedEventArgs(this, isPrivate));
                }
            }
        }

        private void ScrobbleErrorCallback(IntPtr session, Result error)
        {
            if (this.Handle == session)
            {
                this.callbackHandler.ScrobbleError(this, error);
                this.Raise(this.ScrobbleError, error);
            }
        }

        private void StartPlaybackCallback(IntPtr session)
        {
            if (this.Handle == session)
            {
                this.callbackHandler.StartPlayback(this);
                this.Raise(this.PlaybackStarted);
            }
        }

        private void StopPlaybackCallback(IntPtr session)
        {
            if (this.Handle == session)
            {
                this.callbackHandler.StopPlayback(this);
                this.Raise(this.PlaybackStopped);
            }
        }

        private void StreamingErrorCallback(IntPtr session, Result error)
        {
            if (this.Handle == session)
            {
                this.callbackHandler.StreamingError(this, error);
                this.Raise(this.StreamingError, error);
            }
        }

        private void UserinfoUpdatedCallback(IntPtr session)
        {
            if (this.Handle == session)
            {
                this.callbackHandler.UserinfoUpdated(this);
                this.Raise(this.UserInfoUpdated);
            }
        }

        #endregion

        private void Raise(EventHandler<SpotifyEventArgs> handler)
        {
            if (handler != null)
            {
                handler(this, new SpotifyEventArgs(this));
            }
        }

        private void Raise(EventHandler<SpotifyResultEventArgs> handler, Result error)
        {
            if (handler != null)
            {
                handler(this, new SpotifyResultEventArgs(this, error));
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
