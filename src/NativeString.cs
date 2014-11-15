using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetify
{
    internal sealed class NativeString : IDisposable
    {
        public Encoding Encoding { get; private set; }

        private IntPtr _Handle;

        public IntPtr Handle
        {
            get
            {
                return _Handle;
            }
            private set
            {
                _Handle = value;
            }
        }

        public int Size { get; private set; }

        public NativeString(string s) : this(s, Encoding.UTF8) { }

        public NativeString(string s, Encoding encoding)
        {
            Contract.Requires<ArgumentNullException>(encoding != null);

            this.Encoding = encoding;
            if (s != null)
            {
                byte[] stringData = encoding.GetBytes(s);
                this.Handle = Marshal.AllocHGlobal(stringData.Length + 1);
                Marshal.Copy(stringData, 0, this.Handle, stringData.Length);
                Marshal.WriteByte(this.Handle, stringData.Length, 0);
                this.Size = stringData.Length;
            }
            else
            {
                this.Handle = IntPtr.Zero;
                this.Size = 0;
            }
        }

        ~NativeString()
        {
            this.Dispose();
        }

        public void Dispose()
        {
            this.Size = 0;
            this.Value = null;
            Marshal.FreeHGlobal(Interlocked.Exchange(ref _Handle, IntPtr.Zero));

            GC.SuppressFinalize(this);
        }

        public static implicit operator IntPtr(NativeString s)
        {
            return (s != null) ? s.Handle : IntPtr.Zero;
        }
    }
}
