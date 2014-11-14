using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetify
{
    public class Artist : SessionObject
    {
        public Artist(Session session, IntPtr handle)
            : base(session, handle)
        {
            Contract.Requires<ArgumentNullException>(session != null);
            Contract.Requires<ArgumentNullException>(handle != IntPtr.Zero);

            
        }

        private void LoadMetadata()
        {
            lock (this.Session.LibraryLock)
            {

            }
        }
    }
}
