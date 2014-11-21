using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetify
{
    /// <summary>
    /// Represents an object that is associated with a session.
    /// </summary>
    public abstract class SessionObject : SpotifyObject
    {
        /// <summary>
        /// Raised when the initialization of the <see cref="SessionObject"/> was complete and all properties are available.
        /// </summary>
        public event EventHandler InitializationComplete;

        /// <summary>
        /// Backing field.
        /// </summary>
        private bool _IsLoaded;

        /// <summary>
        /// Indicates whether metadata was already loaded.
        /// </summary>
        public bool IsLoaded
        {
            get
            {
                return _IsLoaded;
            }
            protected set
            {
                this.SetProperty(ref _IsLoaded, value);
            }
        }

        /// <summary>
        /// Backing field.
        /// </summary>
        private Session _Session;

        /// <summary>
        /// The <see cref="Session"/> the <see cref="SessionObject"/> is associated with.
        /// </summary>
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

        /// <summary>
        /// Initializes a new <see cref="SessionObject"/>.
        /// </summary>
        protected SessionObject() { }

        /// <summary>
        /// Initializes a new <see cref="SessionObject"/>.
        /// </summary>
        /// <param name="session">The <see cref="Session"/> the <see cref="SessionObject"/> is associated with.</param>
        protected SessionObject(Session session)
        {
            this.Session = session;
        }

        /// <summary>
        /// Initializes a new <see cref="SessionObject"/>.
        /// </summary>
        /// <param name="handle">The objects handle</param>
        /// <param name="session">The <see cref="Session"/> the <see cref="SessionObject"/> is associated with.</param>
        protected SessionObject(Session session, IntPtr handle)
            : base(handle)
        {
            this.Session = session;
        }

        /// <summary>
        /// Initializes a new <see cref="SessionObject"/>.
        /// </summary>
        /// <param name="handle">The objects handle</param>
        /// <param name="isLoaded">Indicates whether metadata was already loaded.</param>
        /// <param name="session">The <see cref="Session"/> the <see cref="SessionObject"/> is associated with.</param>
        protected SessionObject(Session session, IntPtr handle, bool isLoaded)
            : this(session, handle)
        {
            this.IsLoaded = isLoaded;
        }

        /// <summary>
        /// Increases the reference count to the specified instance preventing it from being released on
        /// <see cref="M:Dispose"/>. See remarks.
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

        /// <summary>
        /// Raises the <see cref="E:InitializationComplete"/>-event.
        /// </summary>
        protected void RaiseInitializationComplete()
        {
            this.Raise(this.InitializationComplete);
        }
    }
}
