using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetify
{
    /// <summary>
    /// Represents the region in which a toplist can be received.
    /// </summary>
    public enum ToplistRegion
    {
        /// <summary>
        /// The toplist applies globally.
        /// </summary>
        Everywhere = 0,

        /// <summary>
        /// The toplist applies only for a user.
        /// </summary>
        User = 1
    }
}
