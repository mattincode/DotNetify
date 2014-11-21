using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetify
{
    /// <summary>
    /// Represents a Spotify query.
    /// </summary>
    /// <remarks>
    /// This class wraps sp_search.
    /// </remarks>
    public class Search : SessionObject
    {
        public override void AddRef()
        {
            lock (NativeMethods.LibraryLock)
            {
                NativeMethods.sp_search_add_ref(this.Handle).ThrowIfError();
            }
        }

        protected override void Dispose(bool disposing)
        {
            lock (NativeMethods.LibraryLock)
            {
                NativeMethods.sp_search_release(this.Handle).ThrowIfError();
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
