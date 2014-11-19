using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetify
{
    public class Playlist : SessionObject
    {
        public override void AddRef()
        {
            lock (NativeMethods.LibraryLock)
            {
                NativeMethods.sp_playlist_add_ref(this.Handle);
            }
        }
    }
}
