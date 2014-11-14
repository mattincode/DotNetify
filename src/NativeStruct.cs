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
    internal class NativeStruct<T> : IDisposable
        where T : struct
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

        public int Size { get; private set; }

        public NativeStruct(T structure)
        {
            this.Size = Marshal.SizeOf(typeof(T));
            this.Handle = Marshal.AllocHGlobal(this.Size);
            Marshal.StructureToPtr(structure, this.Handle, false);
        }

        ~NativeStruct()
        {
            this.Dispose();
        }

        public void Dispose()
        {
            IntPtr handle = Interlocked.Exchange(ref _Handle, IntPtr.Zero);
            Marshal.DestroyStructure(handle, typeof(T));
            Marshal.FreeHGlobal(handle);

            GC.SuppressFinalize(this);
        }

        public static implicit operator IntPtr(NativeStruct<T> nativeStruct)
        {
            return (nativeStruct != null) ? nativeStruct.Handle : IntPtr.Zero;
        }
    }
}
