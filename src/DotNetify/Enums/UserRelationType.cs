using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetify
{
    /// <summary>
    /// Describes the relationship between two users.
    /// </summary>
    public enum UserRelationType
    {
        /// <summary>
        /// Not yet known.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// No relation.
        /// </summary>
        None = 1,

        /// <summary>
        /// The currently logged in user is following this user.
        /// </summary>
        Unidirectional = 2,

        /// <summary>
        /// Bidirectional friendship established.
        /// </summary>
        Bidirectional = 3
    }
}
