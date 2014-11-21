using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetify
{
    /// <summary>
    /// Represents a playlist.
    /// </summary>
    /// <remarks>
    /// This class wraps sp_playlist.
    /// </remarks>
    public class Playlist : SessionObject
    {
        public override void AddRef()
        {
            lock (NativeMethods.LibraryLock)
            {
                NativeMethods.sp_playlist_add_ref(this.Handle).ThrowIfError();
            }
        }

        protected override void Dispose(bool disposing)
        {
            lock (NativeMethods.LibraryLock)
            {
                NativeMethods.sp_playlist_release(this.Handle).ThrowIfError();
            }
            base.Dispose(disposing);
        }

        private void LoadMetadata()
        {
            throw new NotImplementedException();
            this.RaiseInitializationComplete();
        }
    }
}
