using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetify
{
    /// <summary>
    /// Controls the type of data that will be included in artist browse queries.
    /// </summary>
    public enum ArtistBrowseType
    {
        /// <summary>
        /// All information except tophit tracks.
        /// </summary>
        [Obsolete("This mode is deprecated and will be removed in a future release.")]
        Full,

        /// <summary>
        /// Only albums and data about artists, no tracks.
        /// </summary>
        NoTracks,

        /// <summary>
        /// Only return data about the artist (artist name, similar artist biography, etc.).
        /// </summary>
        NoAlbums
    }
}
