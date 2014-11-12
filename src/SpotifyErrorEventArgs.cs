using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetify
{
    public class SpotifyErrorEventArgs : SpotifyEventArgs
    {
        /// <summary>
        /// The error that happened during the operation.
        /// </summary>
        public Error Error { get; private set; }

        /// <summary>
        /// Initializes a new <see cref="SpotifyErrorEventArgs"/>.
        /// </summary>
        /// <param name="session"></param>
        /// <param name="error">The error that happened during the operation.</param>
        public SpotifyErrorEventArgs(Session session, Error error)
            : base(session)
        {
            Contract.Requires<ArgumentNullException>(session != null);

            this.Error = error;
        }
    }
}
