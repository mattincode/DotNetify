using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetify
{
    public class ConnectionStateUpdatedEventArgs : SpotifyEventArgs
    {
        public ConnectionState ConnectionState { get; private set; }

        public ConnectionStateUpdatedEventArgs(Session session, ConnectionState state)
            : base(session)
        {
            this.ConnectionState = state;
        }
    }
}
