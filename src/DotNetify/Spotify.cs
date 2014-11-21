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
    /// <remarks>
    /// For a starting point in the API, see <see cref="Session"/>.
    /// </remarks>
    /// <seealso cref="Session"/>
    public static class Spotify
    {
        /// <summary>
        /// The version of the API DotNetify is compiled against.
        /// </summary>
        public const int ApiVersion = 12;

        /// <summary>
        /// Converts the specified, UTF-8 encoded, zero terminated character array into a <see cref="String"/>.
        /// </summary>
        /// <param name="charPtr">The pointer to the char array.</param>
        /// <returns>The decoded <see cref="String"/>.</returns>
        internal static string AsString(this IntPtr charPtr)
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
        internal static string AsString(this IntPtr charPtr, Encoding encoding)
        {
            Contract.Requires<ArgumentNullException>(encoding != null);
            Contract.Ensures(charPtr == IntPtr.Zero || Contract.Result<string>() != null);

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

        /// <summary>
        /// Gets the exception that is associated with the specified <see cref="Result"/>.
        /// </summary>
        /// <param name="resultCode">The error code.</param>
        /// <returns>The exception associated with the specified <see cref="Result"/>.</returns>
        public static Exception GetException(this Result resultCode)
        {
            Contract.Requires<ArgumentException>(resultCode != Result.Ok);

            string errorMessage;
            lock (NativeMethods.LibraryLock)
            {
                errorMessage = NativeMethods.sp_error_message(resultCode).AsString();
            }

            switch (resultCode)
            {
                case Result.NetworkDisabled:
                case Result.ConnectionIssues:
                    return new System.Net.WebException(errorMessage);
                case Result.InvalidArgument:
                    return new ArgumentException(errorMessage);
                case Result.IndexOutOfRange:
                    return new IndexOutOfRangeException(errorMessage);
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
                    return new InvalidOperationException(errorMessage);
                default:
                    throw new ArgumentException("The value of parameter error was undefined.", "error");
            }
        }

        /// <summary>
        /// Checks whether the specified <paramref name="resultCode"/> is an error and throws the appropriate exception.
        /// </summary>
        /// <param name="resultCode">The <see cref="Result"/> to check.</param>
        public static void ThrowIfError(this Result resultCode)
        {
            if (resultCode != Result.Ok)
            {
                throw resultCode.GetException();
            }
        }

        /// <summary>
        /// Returns the size of a sample in bytes for the given <paramref name="sampleType"/>.
        /// </summary>
        /// <param name="sampleType">The <see cref="Type"/> of sample.</param>
        /// <returns>The size in bytes of a single sample as specified by <paramref name="sampleType"/>.</returns>
        public static int GetFrameSize(this SampleType sampleType)
        {
            switch (sampleType)
            {
                case SampleType.SInt16Native:
                    return sizeof(short);
                default:
                    throw new ArgumentException("The value of parameter sampleType was undefined.", "sampleType");
            }
        }
    }
}
