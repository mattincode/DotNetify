using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetify
{
    public abstract class SpotifyObject : ObservableObject, IDisposable
    {
        private IntPtr _Handle;

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

        protected SpotifyObject() { }

        protected SpotifyObject(IntPtr handle)
        {
            this.Handle = handle;
        }

        ~SpotifyObject()
        {
            this.Dispose(false);
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

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
