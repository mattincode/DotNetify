using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetify
{
    /// <summary>
    /// Describes the type of album.
    /// </summary>
    public enum AlbumType
    {
        /// <summary>
        /// The album is a regular album.
        /// </summary>
        Regular = 0,

        /// <summary>
        /// The album is a single.
        /// </summary>
        Single = 1,

        /// <summary>
        /// The album is a compilation.
        /// </summary>
        Compilation,

        /// <summary>
        /// The album is unknown.
        /// </summary>
        Unknown
    }
}
