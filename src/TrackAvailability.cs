using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetify
{
    /// <summary>
    /// Track availability.
    /// </summary>
    public enum TrackAvailability
    {
        /// <summary>
        /// Track is not available.
        /// </summary>
        Unavailable = 0,

        /// <summary>
        /// Track is available and can be played.
        /// </summary>
        Available = 1,

        /// <summary>
        /// Track can not be streamed using this account.
        /// </summary>
        NotStreamable = 2,

        /// <summary>
        /// Track not available on artist's reqeust.
        /// </summary>
        BannedByArtist = 3
    }
}
