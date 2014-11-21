using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetify
{
    /// <summary>
    /// Event arguments with a text message.
    /// </summary>
    public class TextEventArgs : SpotifyEventArgs
    {
        /// <summary>
        /// The text.
        /// </summary>
        public string Text { get; private set; }

        /// <summary>
        /// Initializes new <see cref="TextEventArgs"/>.
        /// </summary>
        /// <param name="session">The <see cref="Session"/>.</param>
        /// <param name="text">The text.</param>
        public TextEventArgs(Session session, string text)
            : base(session)
        {
            Contract.Requires<ArgumentNullException>(session != null);

            this.Text = text;
        }
    }
}
