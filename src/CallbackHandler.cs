using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetify
{
    /// <summary>
    /// Session callbacks.
    /// </summary>
    public abstract class CallbackHandler
    {
        /// <summary>
        /// Backing field.
        /// </summary>
        private CallbackHandler _Null = new NullCallbackHandler();

        /// <summary>
        /// A callback handler with all calls ending in the void. Returns default values for callbacks
        /// that require a return value.
        /// </summary>
        public CallbackHandler Null
        {
            get
            {
                return _Null;
            }
        }

        /// <summary>
        /// Called when login has been processed.
        /// </summary>
        /// <param name="session">The <see cref="Session"/> that was logged into.</param>
        /// <param name="error">The error during login.</param>
        public virtual void LoggedIn(Session session, Error error) { }

        /// <summary>
        /// Called when logout has been processed. Either called explicitly
        /// if you initialize a logout operation, or implicitly if there
        /// is a permanent connection error.
        /// </summary>
        /// <param name="session">The <see cref="Session"/> that was logged out of.</param>
        public virtual void LoggedOut(Session session) { }

        /// <summary>
        /// Called whenever metadata has been updated.
        /// </summary>
        /// <remarks>
        /// If you have metadata cached outside of DotNetify, you should purge your caches and fetch new versions.
        /// </remarks>
        /// <param name="session">The <see cref="Session"/> whose metadata was updated.</param>
        public virtual void MetadataUpdateReceived(Session session) { }

        /// <summary>
        /// Called when there is a connection error, and the library has problems
        /// reconnecting to the Spotify service. Could be called multiple times (as
        /// long as the problem is present),
        /// </summary>
        /// <param name="session">The <see cref="Session"/> with connection errors.</param>
        /// <param name="error">The error that happened.</param>
        public virtual void ConnectionError(Session session, Error error) { }

        /// <summary>
        /// Called when the access point wants to display a message to the user.
        /// </summary>
        /// <param name="session">The <see cref="Session"/>.</param>
        /// <param name="message">The user message.</param>
        public virtual void MessageToUser(Session session, string message) { }

        /// <summary>
        /// Called when processing needs to take place on the main thread.
        /// </summary>
        /// <remarks>
        /// You need to call <see cref="Session.ProcessEvents"/> in the main thread to get
        /// libspotify to do more work. Failure to do so may cause request timeouts,
        /// or a lost connection.
        /// </remarks>
        /// <param name="session">The <see cref="Session"/> which needs to process work.</param>
        public virtual void NotifyMainThread(Session session) { }

        /// <summary>
        /// Called when there is decompressed audio data available..
        /// </summary>
        /// <param name="session">The <see cref="Session"/>.</param>
        /// <param name="format">The audio format descriptor.</param>
        /// <param name="frames">The raw PCM data as described by <paramref name="format"/>.</param>
        /// <param name="frameCount">
        /// The frame count. If this is 0, a discontinuity has occurred (such as after a seek). 
        /// The application should flush its audio fifos, etc.
        /// </param>
        /// <returns>
        /// The number of frames consumed. This value can be used to rate limit the output 
        /// from the library if your output buffers are saturated. The library will retry delivery in about 100ms.
        /// </returns>
        public virtual int MusicDelivery(Session session, AudioFormat format, IntPtr frames, int frameCount) 
        {
            return frameCount;
        }

        /// <summary>
        /// Music has been paused because an account only allows music
        /// to be played from one location simultaneously.
        /// </summary>
        /// <remarks>
        /// When this callback is invoked the application should behave just as if the user pressed the pause button. 
        /// The application should also display a message to the user indicating the playback has been paused because another 
        /// application is playing using the same account.
        /// </remarks>
        /// <param name="session"></param>
        public virtual void PlayTokenLost(Session session) { }

        public virtual void LogMessage(Session session, string entry) { }

        public virtual void EndOfTrack(Session session) { }

        public virtual void StreamingError(Session session, Error error) { }

        public virtual void UserinfoUpdated(Session session) { }

        public virtual void StartPlayback(Session session) { }

        public virtual void StopPlayback(Session session) { }

        public virtual void GetAudioBufferStats(Session session, out AudioBufferStatistics stats)
        {
            stats = default(AudioBufferStatistics);
        }

        public virtual void OfflineSynchronizationStatusChanged(Session session) { }

        public virtual void OfflineError(Session session, Error error) { }

        public virtual void CredentialsBlobUpdated(Session session, string blob) { }

        public virtual void ConnectionStateUpdated(Session session) { }

        public virtual void ScrobbleError(Session session, Error error) { }

        public virtual void SessionModeChanged(Session session, bool isPrivate) { }

        private class NullCallbackHandler : CallbackHandler { }
    }
}
