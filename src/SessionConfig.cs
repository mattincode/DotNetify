using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DotNetify
{
    /// <summary>
    /// Session configuration.
    /// </summary>
    public class SessionConfig : ObservableObject
    {
        /// <summary>
        /// Backing field.
        /// </summary>
        private int _ApiVersion;

        /// <summary>
        /// The version of the Spotify API your application is compiled with. Set to #SPOTIFY_API_VERSION
        /// </summary>
        public int ApiVersion
        {
            get
            {
                return _ApiVersion;
            }
            set
            {
                this.SetProperty(ref _ApiVersion, value);
            }
        }

        /// <summary>
        /// Backing field.
        /// </summary>
        private byte[] _ApplicationKey;

        /// <summary>
        /// Your application key.
        /// </summary>
        public byte[] ApplicationKey
        {
            get
            {
                return _ApplicationKey;
            }
            set
            {
                this.SetProperty(ref _ApplicationKey, value);
            }
        }

        /// <summary>
        /// Backing field.
        /// </summary>
        private string _CacheLocation;

        /// <summary>
        /// The location where Spotify will write cache files.
        /// </summary>
        public string CacheLocation
        {
            get
            {
                return _CacheLocation;
            }
            set
            {
                this.SetProperty(ref _CacheLocation, value);
            }
        }

        /// <summary>
        /// Backing field.
        /// </summary>
        private CallbackHandler _CallbackHandler;

        /// <summary>
        /// The <see cref="CallbackHandler"/> that is responsible for talking to libspotify.
        /// </summary>
        public CallbackHandler CallbackHandler
        {
            get
            {
                return _CallbackHandler;
            }
            set
            {
                this.SetProperty(ref _CallbackHandler, value);
            }
        }

        /// <summary>
        /// Backing field.
        /// </summary>
        private bool _CompressPlaylists;

        /// <summary>
        /// Compress local copy of playlists, reduces disk space usage
        /// </summary>
        public bool CompressPlaylists
        {
            get
            {
                return _CompressPlaylists;
            }
            set
            {
                this.SetProperty(ref _CompressPlaylists, value);
            }
        }

        /// <summary>
        /// Backing field.
        /// </summary>
        private string _DeviceId;

        /// <summary>
        /// Device ID for offline synchronization and logging purposes. The Device Id must be unique to the particular device instance,
        /// i.e. no two units must supply the same Device ID. The Device ID must not change between sessions or power cycles.
        /// Good examples is the device's MAC address or unique serial number.
        /// </summary>
        public string DeviceId
        {
            get
            {
                return _DeviceId;
            }
            set
            {
                this.SetProperty(ref _DeviceId, value);
            }
        }

        /// <summary>
        /// Backing field.
        /// </summary>
        private bool _DontLoadPlaylistsOnStartup;

        /// <summary>
        /// Avoid loading playlists into RAM on startup. See <see cref="Playlist.IsInRam"/> for more details.
        /// </summary>
        public bool DontLoadPlaylistsOnStartup
        {
            get
            {
                return _DontLoadPlaylistsOnStartup;
            }
            set
            {
                this.SetProperty(ref _DontLoadPlaylistsOnStartup, value);
            }
        }

        /// <summary>
        /// Backing field.
        /// </summary>
        private bool _DontSaveMetadataForPlaylists;

        /// <summary>
        /// Don't save metadata for local copies of playlists. Reduces disk space usage at the expense 
        /// of needing to request metadata from Spotify backend when loading list.
        /// </summary>
        public bool DontSaveMetadataForPlaylists
        {
            get
            {
                return _DontSaveMetadataForPlaylists;
            }
            set
            {
                this.SetProperty(ref _DontSaveMetadataForPlaylists, value);
            }
        }

        /// <summary>
        /// Backing field.
        /// </summary>
        private ProxyConfig _ProxyConfig;

        /// <summary>
        /// Proxy configuration.
        /// </summary>
        public ProxyConfig ProxyConfig
        {
            get
            {
                return _ProxyConfig;
            }
            set
            {
                this.SetProperty(ref _ProxyConfig, value);
            }
        }

        /// <summary>
        /// Backing field.
        /// </summary>
        private string _SettingsLocation;

        /// <summary>
        /// The location where Spotify will write setting files and per-user cache items. This includes playlists, track metadata, etc.
        /// </summary>
        /// <remarks>
        /// <see cref="SettingsLocation"/> may be the same path as <see cref="CacheLocation"/>.
        /// <see cref="SettingsLocation"/> folder will not be created (unlike <see cref="CacheLocation"/>),
        /// if you don't want to create the folder yourself, you can set <see cref="SettingsLocation"/> to <see cref="CacheLocation"/>.
        /// </remarks>
        public string SettingsLocation
        {
            get
            {
                return _SettingsLocation;
            }
            set
            {
                this.SetProperty(ref _SettingsLocation, value);
            }
        }

        /// <summary>
        /// Backing field.
        /// </summary>
        private string _TraceFile;

        /// <summary>
        /// Path to API trace file.
        /// </summary>
        public string TraceFile
        {
            get
            {
                return _TraceFile;
            }
            set
            {
                this.SetProperty(ref _TraceFile, value);
            }
        }

        /// <summary>
        /// Backing field.
        /// </summary>
        private string _UserAgent;

        /// <summary>
        /// "User-Agent" for your application - max 255 characters long.
        /// </summary>
        /// <remarks>
        /// The User-Agent should be a relevant, customer facing identification of your application.
        /// </remarks>
        public string UserAgent
        {
            get
            {
                return _UserAgent;
            }
            set
            {
                this.SetProperty(ref _UserAgent, value);
            }
        }

        /// <summary>
        /// Backing field.
        /// </summary>
        private byte[] _UserData;

        /// <summary>
        /// User supplied data for your application
        /// </summary>
        public byte[] UserData
        {
            get
            {
                return _UserData;
            }
            set
            {
                this.SetProperty(ref _UserData, value);
            }
        }
    }
}
