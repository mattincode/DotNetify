using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetify
{
    /// <summary>
    /// Connection rules.
    /// </summary>
    [Flags]
    public enum ConnectionRules
    {
        /// <summary>
        /// The default connection rule (Network | AllowSyncOverWifi)
        /// </summary>
        Default = Network | AllowSyncOverWifi,

        /// <summary>
        /// Allow network traffic. When not set libspotify will force itself into offline mode.
        /// </summary>
        Network = 1,

        /// <summary>
        /// Allow network traffic even if roaming.
        /// </summary>
        NetworkIfRoaming = 2,

        /// <summary>
        /// Set to allow syncing of offline content over mobile connections.
        /// </summary>
        AllowSyncOverMobile = 4,

        /// <summary>
        /// Set to allow syncing of offline content over WiFi.
        /// </summary>
        AllowSyncOverWifi = 8
    }
}
