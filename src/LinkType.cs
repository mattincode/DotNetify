using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetify
{
    /// <summary>
    /// Describes the type of a link.
    /// </summary>
    public enum LinkType
    {
        /// <summary>
        /// Link type not valid - default until the library has parsed the link, or when parsing failed.
        /// </summary>
        Invalid = 0,

        /// <summary>
        /// The link is a track.
        /// </summary>
        Track = 1,

        /// <summary>
        /// The link is an album.
        /// </summary>
        Album = 2,

        /// <summary>
        /// The link is an artist.
        /// </summary>
        Artist = 3,

        /// <summary>
        /// The link is a search.
        /// </summary>
        Search = 4,

        /// <summary>
        /// The link is a playlist.
        /// </summary>
        Playlist = 5,

        /// <summary>
        /// The link is a profile.
        /// </summary>
        Profile = 6,

        /// <summary>
        /// The link is a starred item.
        /// </summary>
        Starred = 7,

        /// <summary>
        /// The link is a local file.
        /// </summary>
        LocalTrack = 8,

        /// <summary>
        /// The link is an image.
        /// </summary>
        Image = 9
    }
}
