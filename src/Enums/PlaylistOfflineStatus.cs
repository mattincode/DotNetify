using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetify
{
    /// <summary>
    /// Playlist offline status.
    /// </summary>
    public enum PlaylistOfflineStatus
    {
        /// <summary>
        /// Playlist is not offline enabled.
        /// </summary>
        OnlineOnly = 0,

        /// <summary>
        /// Playlist is synchronized to local storage.
        /// </summary>
        OfflineEnabled = 1,

        /// <summary>
        /// This playlist is currently downloading. Only one playlist can be in this state any given time.
        /// </summary>
        Downloading = 2,

        /// <summary>
        /// Playlist is queued for download.
        /// </summary>
        Waiting = 3,
    }
}
