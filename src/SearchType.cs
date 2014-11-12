using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetify
{
    /// <summary>
    /// Search types.
    /// </summary>
    public enum SearchType
    {
        /// <summary>
        /// A regular search.
        /// </summary>
        Standard = 0,

        /// <summary>
        /// Search with suggestions.
        /// </summary>
        Suggest = 1
    }
}
