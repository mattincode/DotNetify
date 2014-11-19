using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetify
{
    public class Search : SessionObject
    {
        public override void AddRef()
        {
            lock (NativeMethods.LibraryLock)
            {
                NativeMethods.sp_search_add_ref(this.Handle);
            }
        }
    }
}
