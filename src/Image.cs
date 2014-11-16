using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetify
{
    public class Image : SessionObject
    {
        private byte[] _Data;

        public byte[] Data
        {
            get
            {
                return _Data;
            }
            private set
            {
                this.SetProperty(ref _Data, value);
            }
        }

        private ImageFormat _Format;

        public ImageFormat Format
        {
            get
            {
                return _Format;
            }
            private set
            {
                this.SetProperty(ref _Format, value);
            }
        }

        private IntPtr _Id;
				
		public IntPtr Id
		{
			get
			{
				return _Id;
			}
			private set
			{
                this.SetProperty(ref _Id, value);
			}
		}

        private ImageSize _Size;

        public ImageSize Size
        {
            get
            {
                return _Size;
            }
            private set
            {
                this.SetProperty(ref _Size, value);
            }
        }

        public Image(Session session, IntPtr id)
            : base(session)
        {
            Contract.Requires<ArgumentNullException>(session != null);
            Contract.Requires<ArgumentException>(id != IntPtr.Zero);

            lock (NativeMethods.LibraryLock)
            {
                this.Handle = NativeMethods.sp_image_create(session.Handle, id);
            }

            session.MetadataUpdateReceived += (s, e) => this.LoadMetadata();
            this.LoadMetadata();
        }

        public Image(Session session, Link link)
            : base(session)
        {
            Contract.Requires<ArgumentNullException>(session != null);
            Contract.Requires<ArgumentNullException>(link != null);
            Contract.Requires<ArgumentException>(link.Type == LinkType.Image);

            lock (NativeMethods.LibraryLock)
            {
                this.Handle = NativeMethods.sp_image_create_from_link(session.Handle, link.Handle);
                this.Id = NativeMethods.sp_image_image_id(this.Handle);
            }

            session.MetadataUpdateReceived += (s, e) => this.LoadMetadata();
            this.LoadMetadata();
        }

        public override void AddRef()
        {
            lock (NativeMethods.LibraryLock)
            {
                Spotify.CheckForError(NativeMethods.sp_image_add_ref(this.Handle));
            }
        }

        private void LoadMetadata()
        {
            Session s = this.Session;
            if (s == null)
            {
                throw new InvalidOperationException("Metadata cannot be loaded, session was null. This is a programming bug, and SHOULD NOT happen at runtime. Please contact the developers.");
            }

            lock (NativeMethods.LibraryLock)
            {
                IntPtr handle = this.Handle;
                if (this.IsLoaded = NativeMethods.sp_image_is_loaded(handle))
                {
                    throw new NotImplementedException();
                }
            }
        }
    }
}
