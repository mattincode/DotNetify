using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DotNetify
{
    /// <summary>
    /// Contains global helper methods and constants related to Spotify.
    /// </summary>
    public static class Spotify
    {
        /// <summary>
        /// The version of the API DotNetify is compiled against.
        /// </summary>
        public const int ApiVersion = 12;

        /// <summary>
        /// Checks whether the specified <paramref name="resultCode"/> is an error and throws the appropriate exception.
        /// </summary>
        /// <param name="resultCode">The <see cref="Result"/> to check.</param>
        public static void CheckForError(Result resultCode)
        {
            string errorMessage;
            lock (NativeMethods.LibraryLock)
            {
                errorMessage = NativeMethods.sp_error_message(resultCode).AsString();
            }

            switch (resultCode)
            {
                case Result.NetworkDisabled:
                case Result.ConnectionIssues:
                    throw new System.Net.WebException(errorMessage);
                case Result.InvalidArgument:
                    throw new ArgumentException(errorMessage);
                case Result.IndexOutOfRange:
                    throw new IndexOutOfRangeException(errorMessage);
                case Result.BadApiVersion:
                case Result.ApiInitializationFailed:
                case Result.InvalidApplicationKey:
                case Result.ClientTooOld:
                case Result.TrackNotPlayable:
                case Result.InvalidUsernameOrPassword:
                case Result.UserBanned:
                case Result.UndefinedPermanent:
                case Result.InvalidUserAgent:
                case Result.MissingCallback:
                case Result.InvalidInputData:
                case Result.UserNeedsPremium:
                case Result.Transient:
                case Result.IsLoading:
                case Result.NoStreamAvailable:
                case Result.PermissionDenied:
                case Result.TargetInboxFull:
                case Result.NoCache:
                case Result.NoSuchUser:
                case Result.NoCredentials:
                case Result.InvalidDeviceId:
                case Result.CantOpenTraceFile:
                case Result.ApplicationBanned:
                case Result.OfflineDiskCache:
                case Result.OfflineExpired:
                case Result.OfflineNotAllowed:
                case Result.OfflineLicenseLost:
                case Result.OfflineLicenseError:
                case Result.LastFmAuthenticationError:
                case Result.SystemFailure:
                    throw new InvalidOperationException(errorMessage);
                case Result.None:
                    break;
                default:
                    throw new ArgumentException("The value of parameter error was undefined.", "error");
            }
        }

        /// <summary>
        /// Converts the specified, UTF-8 encoded, zero terminated character array into a <see cref="String"/>.
        /// </summary>
        /// <param name="charPtr">The pointer to the char array.</param>
        /// <returns>The decoded <see cref="String"/>.</returns>
        public static string AsString(this IntPtr charPtr)
        {
            return AsString(charPtr, Encoding.UTF8);
        }

        /// <summary>
        /// Converts the speicified, zero terminated character array into a <see cref="String"/>. The specified
        /// <paramref name="encoding"/> will be used to decode the data.
        /// </summary>
        /// <param name="charPtr">The pointer to the char array.</param>
        /// <param name="encoding">The encoding used to decode the data into the <see cref="String"/>.</param>
        /// <returns>The decoded <see cref="String"/>.</returns>
        public static string AsString(this IntPtr charPtr, Encoding encoding)
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
