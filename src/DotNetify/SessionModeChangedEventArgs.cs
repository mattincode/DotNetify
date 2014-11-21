using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetify
{
    public class SessionModeChangedEventArgs : SpotifyEventArgs
    {
        public bool IsPrivate { get; private set; }

        public SessionModeChangedEventArgs(Session session, bool isPrivate)
            : base(session)
        {
            Contract.Requires<ArgumentNullException>(session != null);

            this.IsPrivate = isPrivate;
        }
    }
}
