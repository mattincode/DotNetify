using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetify
{
    /// <summary>
    /// Track offline status.
    /// </summary>
    public enum TrackOfflineStatus
    {
        /// <summary>
        /// Not marked for offline.
        /// </summary>
        OnlineOnly = 0,

        /// <summary>
        /// Waiting for download.
        /// </summary>
        Waiting = 1,

        /// <summary>
        /// Currently downloading.
        /// </summary>
        Downloading = 2,

        /// <summary>
        /// Downloaded OK and can be played.
        /// </summary>
        OfflineEnabled = 3,

        /// <summary>
        /// Error during download.
        /// </summary>
        Error = 4,

        /// <summary>
        /// Downloaded OK but not playable due to expiery.
        /// </summary>
        Expired = 5,

        /// <summary>
        /// Waiting because device have reached max number of allowed tracks.
        /// </summary>
        LimitExceeded = 6,

        /// <summary>
        /// Downloaded OK and available but scheduled for re-download.
        /// </summary>
        ScheduledForResync = 7
    }
}
