using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetify
{
    /// <summary>
    /// Describes the current state of the connection.
    /// </summary>
    public enum ConnectionState
    {
        /// <summary>
        /// User not yet logged in.
        /// </summary>
        LoggedOut = 0,

        /// <summary>
        /// Logged in against a Spotify access point.
        /// </summary>
        LoggedIn = 1,

        /// <summary>
        /// Was logged in, but has now been disconnected.
        /// </summary>
        Disconnected = 2,

        /// <summary>
        /// The connection state is undefined.
        /// </summary>
        Undefined = 3,

        /// <summary>
        /// Logged in in offline mode.
        /// </summary>
        Offline = 4
    }
}
