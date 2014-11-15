using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetify
{
    public class SpotifyResultEventArgs : SpotifyEventArgs
    {
        /// <summary>
        /// The function result.
        /// </summary>
        public Result Result { get; private set; }

        /// <summary>
        /// Initializes a new <see cref="SpotifyResultEventArgs"/>.
        /// </summary>
        /// <param name="session">The <see cref="Session"/>.</param>
        /// <param name="result">The function result.</param>
        public SpotifyResultEventArgs(Session session, Result result)
            : base(session)
        {
            Contract.Requires<ArgumentNullException>(session != null);

            this.Result = result;
        }
    }
}
