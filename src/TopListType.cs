using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetify
{
    /// <summary>
    /// Descibes the type of the items of a toplist.
    /// </summary>
    public enum ToplistType
    {
        /// <summary>
        /// The list contains the top artists.
        /// </summary>
        Artists = 0,

        /// <summary>
        /// The list contains the top albums.
        /// </summary>
        Albums = 1,

        /// <summary>
        /// The list contains the top tracks.
        /// </summary>
        Tracks = 2
    }
}
