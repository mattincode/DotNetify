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
    internal class NativeByteArray : IDisposable
    {
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

        public int Length { get; private set; }

        public NativeByteArray(byte[] array)
        {
            if (array != null)
            {
                this.Length = array.Length;
                this.Handle = Marshal.AllocHGlobal(array.Length);
                Marshal.Copy(array, 0, this.Handle, array.Length);
            }
            else
            {
                this.Handle = IntPtr.Zero;
                this.Length = 0;
            }
        }

        ~NativeByteArray()
        {
            this.Dispose();
        }

        public void Dispose()
        {
            Marshal.FreeHGlobal(Interlocked.Exchange(ref _Handle, IntPtr.Zero));
            GC.SuppressFinalize(this);
        }

        public static implicit operator IntPtr(NativeByteArray array)
        {
            return (array != null) ? array.Handle : IntPtr.Zero;
        }
    }
}
