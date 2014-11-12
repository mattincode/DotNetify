using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DotNetify
{
    public static class Spotify
    {
        public const int ApiVersion = 12;

        internal static void CheckForError(Error error)
        {
            switch (error)
            {
                case Error.NetworkDisabled:
                case Error.ConnectionIssues:
                    throw new System.Net.WebException(NativeMethods.GetErrorMessage(error).AsString());
                case Error.InvalidArgument:
                    throw new ArgumentException(NativeMethods.GetErrorMessage(error).AsString());
                case Error.IndexOutOfRange:
                    throw new IndexOutOfRangeException(NativeMethods.GetErrorMessage(error).AsString());
                case Error.BadApiVersion:
                case Error.ApiInitializationFailed:
                case Error.InvalidApplicationKey:
                case Error.ClientTooOld:
                case Error.TrackNotPlayable:
                case Error.InvalidUsernameOrPassword:
                case Error.UserBanned:
                case Error.UndefinedPermanent:
                case Error.InvalidUserAgent:
                case Error.MissingCallback:
                case Error.InvalidInputData:
                case Error.UserNeedsPremium:
                case Error.Transient:
                case Error.IsLoading:
                case Error.NoStreamAvailable:
                case Error.PermissionDenied:
                case Error.TargetInboxFull:
                case Error.NoCache:
                case Error.NoSuchUser:
                case Error.NoCredentials:
                case Error.InvalidDeviceId:
                case Error.CantOpenTraceFile:
                case Error.ApplicationBanned:
                case Error.OfflineDiskCache:
                case Error.OfflineExpired:
                case Error.OfflineNotAllowed:
                case Error.OfflineLicenseLost:
                case Error.OfflineLicenseError:
                case Error.LastFmAuthenticationError:
                case Error.SystemFailure:
                    throw new InvalidOperationException(NativeMethods.GetErrorMessage(error).AsString());
                case Error.None:
                default:
                    break;
            }
        }

        internal static string AsString(this IntPtr charPtr)
        {
            return AsString(charPtr, Encoding.UTF8);
        }

        internal static string AsString(this IntPtr charPtr, Encoding encoding)
        {
            Contract.Requires<ArgumentNullException>(encoding != null);

            if (charPtr == IntPtr.Zero)
            {
                return null;
            }

            int length = 0;
            while (Marshal.ReadByte(charPtr, length) != 0)
            {
                ++length;
            }
            byte[] stringData = new byte[length];
            Marshal.Copy(charPtr, stringData, 0, stringData.Length);
            return encoding.GetString(stringData);
        }
    }
}
