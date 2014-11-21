using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetify
{
    /// <summary>
    /// Represents an object that wraps a libspotify object.
    /// </summary>
    public abstract class SpotifyObject : ObservableObject, IDisposable
    {
        /// <summary>
        /// Backing field.
        /// </summary>
        private IntPtr _Handle;

        /// <summary>
        /// The handle to the underlying libspotify object.
        /// </summary>
        public IntPtr Handle
        {
            get
            {
                return _Handle;
            }
            protected set
            {
                this.SetProperty(ref _Handle, value);
            }
        }

        /// <summary>
        /// Initializes a new <see cref="SpotifyObject"/>.
        /// </summary>
        protected SpotifyObject() { }

        /// <summary>
        /// Initializes a new <see cref="SpotifyObject"/> and sets the handle.
        /// </summary>
        /// <param name="handle">The handle to the underlying libspotify object.</param>
        protected SpotifyObject(IntPtr handle)
        {
            this.Handle = handle;
        }

        /// <summary>
        /// Finalizes the object releasing all resources.
        /// </summary>
        ~SpotifyObject()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// Disposes the object releasing all resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
        }

        /// <summary>
        /// Disposes the object releasing all resources.
        /// </summary>
        /// <param name="disposing">Indicates whether to dispose managed resources as well.</param>
        protected virtual void Dispose(bool disposing)
        {
            this.Handle = IntPtr.Zero;
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the specified <paramref name="disposable"/>, if it is not <c>null</c>.
        /// </summary>
        /// <param name="disposable">The <see cref="IDisposable"/> to be disposed. May be null.</param>
        protected void Dispose(IDisposable disposable)
        {
            if (disposable != null)
            {
                disposable.Dispose();
            }
        }
    }
}
