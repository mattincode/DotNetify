using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetify
{
    /// <summary>
    /// Represents generic event arguments associated with a spotify session.
    /// </summary>
    public class SpotifyEventArgs : EventArgs
    {
        /// <summary>
        /// The <see cref="Session"/>.
        /// </summary>
        public Session Session { get; protected set; }

        /// <summary>
        /// Initializes a new <see cref="SpotifyEventArgs"/>.
        /// </summary>
        protected SpotifyEventArgs() { }

        /// <summary>
        /// Initializes a new <see cref="SpotifyEventArgs"/> and sets the session.
        /// </summary>
        /// <param name="session">The <see cref="Session"/>.</param>
        public SpotifyEventArgs(Session session)
        {
            Contract.Requires<ArgumentNullException>(session != null);

            this.Session = session;
        }
    }
}
