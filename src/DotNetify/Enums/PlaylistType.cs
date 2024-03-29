﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetify
{
    /// <summary>
    /// Playlist types.
    /// </summary>
    public enum PlaylistType
    {
        /// <summary>
        /// A normal playlist.
        /// </summary>
        Playlist = 0,

        /// <summary>
        /// Marks a folder starting point.
        /// </summary>
        StartFolder = 1,

        /// <summary>
        /// Marks a folder ending point.
        /// </summary>
        EndFolder = 2,

        /// <summary>
        /// Unknown entry.
        /// </summary>
        Placeholder = 3
    }
}
