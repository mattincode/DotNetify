using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetify
{
    public class LoggedInEventArgs : SpotifyResultEventArgs
    {
        public User User { get; private set; }

        public LoggedInEventArgs(Session session, Result error, User user)
            : base(session, error)
        {
            this.User = user;
        }
    }
}
