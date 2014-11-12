using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DotNetify
{
    internal static class NativeMethods
    {
        [DllImport("libspotify", EntryPoint = "sp_error_message")]
        public static extern IntPtr GetErrorMessage(Error error);
    }
}
