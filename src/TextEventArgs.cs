using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetify
{
    public class TextEventArgs : SpotifyEventArgs
    {
        public string Text { get; private set; }

        public TextEventArgs(Session session, string text)
            : base(session)
        {
            Contract.Requires<ArgumentNullException>(session != null);

            this.Text = text;
        }
    }
}
