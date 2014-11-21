using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetify
{
    /// <summary>
    /// Current connection type set using sp_session_set_connection_type().
    /// </summary>
    public enum ConnectionType
    {
        /// <summary>
        /// Connection type unknown (Default).
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// No connection.
        /// </summary>
        None = 1,

        /// <summary>
        /// Mobile data (EDGE, 3G, etc.).
        /// </summary>
        Mobile = 2,

        /// <summary>
        /// Roamed mobile data (EDGE, 3G, etc.).
        /// </summary>
        MobileRoaming = 3,

        /// <summary>
        /// Wireless connection.
        /// </summary>
        Wifi = 4,

        /// <summary>
        /// Ethernet cable, etc.
        /// </summary>
        Wired = 5
    }
}
