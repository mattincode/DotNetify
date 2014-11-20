DotNetify
=========

A .NET Spotify wrapper that attempts to encapsulate and abstract the libspotify C API as much as possible and useful.
Every feature of libspotify will be wrapped and nicely presented as classes. The reference counting algorithm of libspotify
will also be abstracted using <c>IDisposable</c>.


## How to start

To get playback started you need to create a [Session](https://github.com/SkyLapse/DotNetify/blob/master/src/Session.cs), 
which needs a [SessionConfig](https://github.com/SkyLapse/DotNetify/blob/master/src/SessionConfig.cs) for initialization.

```
SessionConfig config = new SessionConfig()
{
    ApiVersion = Spotify.ApiVersion, // Set to the API-version you want to target, in nearly all cases this will be Spotify.ApiVersion
    ApplicationKey = myAppKey, // Application key as byte[] from Spotify
    CacheLocation = myCacheFolder, // Optional, Spotify will cache music here so it doesn't need to be downloaded again
    CallbackHandler = myCallbackHandler, // Class that receives the callbacks from libspotify, required.
    CompressPlaylists = true, // Compress local copy of playlists, reduces disk space usage.
    DeviceId = "123abc", // Device ID for offline synchronization and logging purposes.
    DontLoadPlaylistsOnStartup = false, // Avoid loading playlists into RAM on startup to reduce memory usage.
    DontSaveMetadataForPlaylists = false, // Don't save metadata for local copies of playlists.
    ProxyConfig = default(ProxyConfig), // Proxy configuration
    SettingsLocation = mySettingsLocation, // The location where Spotify will write setting files and per-user cache items.
    TraceFile = myTraceFile, // Path to trace file
    UserAgent = "DotNetify", // The user agent of the application using the API
    UserData = null // Optional userdata which will be available later
};

Session session = new Session(config);
```

Inside the [CallbackHandler](https://github.com/SkyLapse/DotNetify/blob/master/src/CallbackHandler.cs) 
(and through the event) you'll receive raw PCM data from libspotify which you can play.

```
public class CustomCallbackHandler : CallbackHandler
{
    public override int MusicDelivery(Session session, MusicPacket musicPacket)
    {
        // Process PCM data here, return the amount of frames your application consumed in one go
    }
}
```
