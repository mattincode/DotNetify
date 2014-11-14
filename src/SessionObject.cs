using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetify
{
    public class SessionObject : SpotifyObject
    {
        private Session _Session;

        public Session Session
        {
            get
            {
                return _Session;
            }
            protected set
            {
                this.SetProperty(ref _Session, value);
            }
        }

        protected SessionObject() { }

        protected SessionObject(Session session, IntPtr handle)
            : base(handle)
        {
            this.Session = session;
        }
    }
}
