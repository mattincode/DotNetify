using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetify
{
    public abstract class SessionObject : SpotifyObject
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

        /// <summary>
        /// Increases the reference count to the specified instance preventing it from being released on
        /// <see cref="Dispose"/>. See remarks.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Calling <see cref="M:Dispose"/> decreases the reference count once.
        /// </para>
        /// <para>
        /// You usually WON'T need to call this manually. As long as everything is being disposed only when
        /// no other instance needs the instance anymore, this does not need to be called.</para>
        /// </remarks>
        public abstract void AddRef();
    }
}
