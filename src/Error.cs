using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetify
{
    /// <summary>
    /// Contains spotify error and return values.
    /// </summary>
    public enum Error
    {
        /// <summary>
        /// No errors encountered.
        /// </summary>
        None = 0,

        /// <summary>
        /// No errors encountered.
        /// </summary>
        Ok = None,

        /// <summary>
        /// The library version targeted does not match the one you claim you support.
        /// </summary>
        BadApiVersion = 1,

        /// <summary>
        /// Initialization of library failed - are cache locations, etc. valid?
        /// </summary>
        ApiInitializationFailed = 2,

        /// <summary>
        /// The track specified for playing cannot be played.
        /// </summary>
        TrackNotPlayable = 3,

        /// <summary>
        /// The application key is invalid.
        /// </summary>
        InvalidApplicationKey = 5,

        /// <summary>
        /// Login failed because of bad username and/or password.
        /// </summary>
        InvalidUsernameOrPassword = 6,

        /// <summary>
        /// The specified username is banned.
        /// </summary>
        UserBanned = 7, 

        /// <summary>
        /// Cannot connect to the Spotify backend system.
        /// </summary>
        ConnectionIssues = 8,

        /// <summary>
        /// Client is too old, library will need to be updated.
        /// </summary>
        ClientTooOld = 9,

        /// <summary>
        /// Some other error occurred, and it is permanent (e.g. trying to relogin will not help).
        /// </summary>
        UndefinedPermanent = 10,

        /// <summary>
        /// The user agent string is invalid or too long.
        /// </summary>
        InvalidUserAgent = 11,

        /// <summary>
        /// No valid callback registered to handle events.
        /// </summary>
        MissingCallback = 12,

        /// <summary>
        /// Input data was either missing or invalid.
        /// </summary>
        InvalidInputData = 13,

        /// <summary>
        /// Index out of range.
        /// </summary>
        IndexOutOfRange = 14,

        /// <summary>
        /// The specified user needs a premium account.
        /// </summary>
        UserNeedsPremium = 15,

        /// <summary>
        /// A transient error occurred.
        /// </summary>
        Transient = 16,

        /// <summary>
        /// The resource is currently loading.
        /// </summary>
        IsLoading = 17,

        /// <summary>
        /// Could not find any suitable stream to play.
        /// </summary>
        NoStreamAvailable = 18,

        /// <summary>
        /// Requested operation is not allowed.
        /// </summary>
        PermissionDenied = 19,

        /// <summary>
        /// Target inbox is full.
        /// </summary>
        TargetInboxFull = 20, 

        /// <summary>
        /// Cache is not enabled.
        /// </summary>
        NoCache = 21, 

        /// <summary>
        /// Requested user does not exist.
        /// </summary>
        NoSuchUser = 22, 

        /// <summary>
        /// No credentials are stored.
        /// </summary>
        NoCredentials = 23, 

        /// <summary>
        /// Network disabled.
        /// </summary>
        NetworkDisabled = 24, 

        /// <summary>
        /// Invalid device ID.
        /// </summary>
        InvalidDeviceId = 25, 

        /// <summary>
        /// Unable to open trace file.
        /// </summary>
        CantOpenTraceFile = 26, 

        /// <summary>
        /// This application is no longer allowed to use the Spotify service.
        /// </summary>
        ApplicationBanned = 27, 

        /// <summary>
        /// Reached the device limit for number of tracks to download.
        /// </summary>
        TooManyTracks = 31, 

        /// <summary>
        /// Disk cache is full so no more tracks can be downloaded to offline mode.
        /// </summary>
        OfflineDiskCache = 32, 

        /// <summary>
        /// Offline key has expired, the user needs to go online again.
        /// </summary>
        OfflineExpired = 33, 

        /// <summary>
        /// This user is not allowed to use offline mode.
        /// </summary>
        OfflineNotAllowed = 34, 

        /// <summary>
        /// The license for this device has been lost. Most likely because the user used offline on three other device.
        /// </summary>
        OfflineLicenseLost = 35, 

        /// <summary>
        /// The Spotify license server does not respond correctly.
        /// </summary>
        OfflineLicenseError = 36, 

        /// <summary>
        /// A LastFM scrobble authentication error has occurred.
        /// </summary>
        LastFmAuthenticationError = 39, 

        /// <summary>
        /// An invalid argument was specified.
        /// </summary>
        InvalidArgument = 40, 

        /// <summary>
        /// An operating system error.
        /// </summary>
        SystemFailure = 41, 
    } 
}
