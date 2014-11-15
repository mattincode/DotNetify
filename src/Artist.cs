﻿using System;
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

            throw new NotImplementedException();
        }

        public override void AddRef()
        {
            lock (NativeMethods.LibraryLock)
            {
                NativeMethods.sp_artist_add_ref(this.Handle);
            }
        }

        private void LoadMetadata()
        {
            lock (NativeMethods.LibraryLock)
            {

            }
        }
    }
}
