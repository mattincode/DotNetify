using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetify
{
    public class Album : SessionObject
    {
        private AlbumType _Type;

        public AlbumType Type
        {
            get
            {
                return _Type;
            }
            set
            {
                this.SetProperty(ref _Type, value);
            }
        }

        public Album(Session session, IntPtr handle)
            : base(session, handle)
        {
            Contract.Requires<ArgumentNullException>(session != null);
            Contract.Requires<ArgumentNullException>(handle != IntPtr.Zero);


        }

        private void LoadMetadata()
        {

        }
    }
}
