using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetify
{
    /// <summary>
    /// Represents a Spotify user.
    /// </summary>
    /// <remarks>
    /// This class wraps sp_user.
    /// </remarks>
    public class User : SessionObject
    {
        /// <summary>
        /// Backing field.
        /// </summary>
        private string _CanonicalName;

        /// <summary>
        /// The users canonical name.
        /// </summary>
        public string CanonicalName
        {
            get
            {
                return _CanonicalName;
            }
            private set
            {
                this.SetProperty(ref _CanonicalName, value);
            }
        }

        /// <summary>
        /// Initializes a new <see cref="User"/>.
        /// </summary>
        /// <param name="session">The <see cref="Session"/>.</param>
        /// <param name="handle">The handle to the underlying libspotify user object.</param>
        public User(Session session, IntPtr handle)
            : base(session, handle)
        {
            Contract.Requires<ArgumentNullException>(session != null);
            Contract.Requires<ArgumentException>(handle != IntPtr.Zero);

            session.MetadataUpdateReceived += (s, e) => this.LoadMetadata();
            this.LoadMetadata();
        }

        /// <summary>
        /// Increments the reference count of the <see cref="User"/> preventing the underlying libspotify user
        /// object from being collected when this class is disposed.
        /// </summary>
        public override void AddRef()
        {
            lock (NativeMethods.LibraryLock)
            {
                NativeMethods.sp_user_add_ref(this.Handle).ThrowIfError();
            }
        }

        /// <summary>
        /// Disposes the user decrementing the reference count of the underlying libspotify user object.
        /// </summary>
        /// <param name="disposing">Indicates whether to release managed resources as well.</param>
        protected override void Dispose(bool disposing)
        {
            lock (NativeMethods.LibraryLock)
            {
                NativeMethods.sp_user_release(this.Handle);
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Loads the <see cref="User"/>s metadata.
        /// </summary>
        private void LoadMetadata()
        {
            Session session = this.Session;
            if (session == null)
            {
                throw new InvalidOperationException("User metadata cannot be loaded, session was null. This is a programming bug, and SHOULD NOT happen at runtime. Please contact the developers.");
            }

            lock (NativeMethods.LibraryLock)
            {
                IntPtr handle = this.Handle;
                if (this.IsLoaded = NativeMethods.sp_user_is_loaded(handle))
                {
                    this.CanonicalName = NativeMethods.sp_user_canonical_name(handle).AsString();
                    this.Name = NativeMethods.sp_user_display_name(handle).AsString();
                    this.RaiseInitializationComplete();
                }
            }
        }
    }
}
