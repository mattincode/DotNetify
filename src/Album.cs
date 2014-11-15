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
        private Artist _Artist;

        public Artist Artist
        {
            get
            {
                return _Artist;
            }
            private set
            {
                this.SetProperty(ref _Artist, value);
            }
        }

        private bool _IsAvailable;

        public bool IsAvailable
        {
            get
            {
                return _IsAvailable;
            }
            private set
            {
                this.SetProperty(ref _IsAvailable, value);
            }
        }

        private bool _IsLoaded;

        public bool IsLoaded
        {
            get
            {
                return _IsLoaded;
            }
            private set
            {
                this.SetProperty(ref _IsLoaded, value);
            }
        }

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

            throw new NotImplementedException();
        }

        public override void AddRef()
        {
            lock (NativeMethods.LibraryLock)
            {
                NativeMethods.sp_album_add_ref(this.Handle);
            }
        }

        private void LoadMetadata()
        {
            Session s = this.Session;
            if (s == null)
            {
                throw new InvalidOperationException("Session was null, cannot load the metadata.");
            }

            lock (NativeMethods.LibraryLock)
            {
                IntPtr handle = this.Handle;
                if (this.IsLoaded = NativeMethods.sp_album_is_loaded(handle))
                {

                }
            }
        }
    }
}
