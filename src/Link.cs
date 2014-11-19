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
    /// Represents a link, an abstraction over a textual ID to a Spotify entity.
    /// </summary>
    public class Link : SessionObject
    {
        /// <summary>
        /// Backing field.
        /// </summary>
        private LinkType _Type;

        /// <summary>
        /// The type of entity the link is pointing to.
        /// </summary>
        public LinkType Type
        {
            get
            {
                return _Type;
            }
            private set
            {
                this.SetProperty(ref _Type, value);
            }
        }

        /// <summary>
        /// Initializes a new <see cref="Link"/>.
        /// </summary>
        /// <param name="session">The <see cref="Session"/>.</param>
        /// <param name="album">The <see cref="Album"/> that is being wrapped by the link.</param>
        public Link(Session session, Album album)
            : base(session)
        {
            Contract.Requires<ArgumentNullException>(session != null);
            Contract.Requires<ArgumentNullException>(album != null);

            lock (NativeMethods.LibraryLock)
            {
                this.Handle = NativeMethods.sp_link_create_from_album(album.Handle);
            }
            this.Initialize();
        }

        /// <summary>
        /// Initializes a new <see cref="Link"/> from the specified album cover.
        /// </summary>
        /// <remarks><see cref="P:Type"/> will be <see cref="LinkType.Image"/>.</remarks>
        /// <param name="session">The <see cref="Session"/>.</param>
        /// <param name="album">The <see cref="Album"/> to retreive the cover of.</param>
        /// <param name="imageSize">The size of the cover to retreive.</param>
        public Link(Session session, Album album, ImageSize imageSize)
            : base(session)
        {
            Contract.Requires<ArgumentNullException>(session != null);
            Contract.Requires<ArgumentNullException>(album != null);

            lock (NativeMethods.LibraryLock)
            {
                this.Handle = NativeMethods.sp_link_create_from_album_cover(album.Handle, imageSize);
            }
            this.Initialize();
        }

        /// <summary>
        /// Initializes a new <see cref="Link"/>.
        /// </summary>
        /// <param name="session">The <see cref="Session"/>.</param>
        /// <param name="artist">The <see cref="Artist"/> that is being wrapped by the link.</param>
        public Link(Session session, Artist artist)
            : base(session)
        {
            Contract.Requires<ArgumentNullException>(session != null);
            Contract.Requires<ArgumentNullException>(artist != null);

            lock (NativeMethods.LibraryLock)
            {
                this.Handle = NativeMethods.sp_link_create_from_artist(artist.Handle);
            }
            this.Initialize();
        }

        /// <summary>
        /// Initializes a new <see cref="Link"/>.
        /// </summary>
        /// <remarks><see cref="P:Type"/> will be <see cref="LinkType.Image"/>.</remarks>
        /// <param name="session">The <see cref="Session"/>.</param>
        /// <param name="artist">The <see cref="Artist"/> to retreive fanart of.</param>
        /// <param name="imageSize">The size of the fanart to retreive.</param>
        public Link(Session session, Artist artist, ImageSize imageSize)
            : base(session)
        {
            Contract.Requires<ArgumentNullException>(session != null);
            Contract.Requires<ArgumentNullException>(artist != null);

            lock (NativeMethods.LibraryLock)
            {
                this.Handle = NativeMethods.sp_link_create_from_artist_portrait(artist.Handle, imageSize);
            }
            this.Initialize();
        }

        /// <summary>
        /// Initializes a new <see cref="Link"/>.
        /// </summary>
        /// <param name="session">The <see cref="Session"/>.</param>
        /// <param name="image">The <see cref="Image"/> that will be wrapped by the <see cref="Link"/>.</param>
        public Link(Session session, Image image)
            : base(session)
        {
            Contract.Requires<ArgumentNullException>(session != null);
            Contract.Requires<ArgumentNullException>(image != null);

            lock (NativeMethods.LibraryLock)
            {
                this.Handle = NativeMethods.sp_link_create_from_image(image.Handle);
            }
            this.Initialize();
        }

        /// <summary>
        /// Initializes a new <see cref="Link"/>.
        /// </summary>
        /// <param name="session">The <see cref="Session"/>.</param>
        /// <param name="playlist">The <see cref="Playlist"/> that will be wrapped by the <see cref="Link"/>.</param>
        public Link(Session session, Playlist playlist)
            : base(session)
        {
            Contract.Requires<ArgumentNullException>(session != null);
            Contract.Requires<ArgumentNullException>(playlist != null);

            lock (NativeMethods.LibraryLock)
            {
                this.Handle = NativeMethods.sp_link_create_from_playlist(playlist.Handle);
            }
            this.Initialize();
        }

        /// <summary>
        /// Initializes a new <see cref="Link"/>.
        /// </summary>
        /// <param name="session">The <see cref="Session"/>.</param>
        /// <param name="s">The string to create a link from.</param>
        public Link(Session session, string s)
            : base(session)
        {
            Contract.Requires<ArgumentNullException>(session != null);
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(s));

            lock (NativeMethods.LibraryLock)
            {
                using (NativeString nativeLinkString = new NativeString(s))
                {
                    this.Handle = NativeMethods.sp_link_create_from_string(nativeLinkString);
                }
            }
            this.Initialize();
        }

        /// <summary>
        /// Initializes a new <see cref="Link"/>.
        /// </summary>
        /// <param name="session">The <see cref="Session"/>.</param>
        /// <param name="search">The <see cref="Search"/> that will be wrapped by the link.</param>
        public Link(Session session, Search search)
            : base(session)
        {
            Contract.Requires<ArgumentNullException>(session != null);
            Contract.Requires<ArgumentNullException>(search != null);

            lock (NativeMethods.LibraryLock)
            {
                this.Handle = NativeMethods.sp_link_create_from_search(search.Handle);
            }
            this.Initialize();
        }

        /// <summary>
        /// Initializes a new <see cref="Link"/>.
        /// </summary>
        /// <param name="session">The <see cref="Session"/>.</param>
        /// <param name="track">The <see cref="Track"/> that will be wrapped by the link.</param>
        public Link(Session session, Track track)
            : this(session, track, TimeSpan.Zero)
        {
            Contract.Requires<ArgumentNullException>(session != null);
            Contract.Requires<ArgumentNullException>(track != null);
        }

        /// <summary>
        /// Initializes a new <see cref="Link"/>.
        /// </summary>
        /// <param name="session">The <see cref="Session"/>.</param>
        /// <param name="track">The <see cref="Track"/> that will be wrapped by the link.</param>
        /// <param name="offset">The offset into the track.</param>
        public Link(Session session, Track track, TimeSpan offset)
            : base(session)
        {
            Contract.Requires<ArgumentNullException>(session != null);
            Contract.Requires<ArgumentNullException>(track != null);
            Contract.Requires<ArgumentOutOfRangeException>(offset >= TimeSpan.Zero);

            lock (NativeMethods.LibraryLock)
            {
                this.Handle = NativeMethods.sp_link_create_from_track(track.Handle, offset.Milliseconds);
            }
            this.Initialize();
        }

        /// <summary>
        /// Initializes a new <see cref="Link"/>.
        /// </summary>
        /// <param name="session">The <see cref="Session"/>.</param>
        /// <param name="user">The <see cref="User"/> that will be wrapped by the link.</param>
        public Link(Session session, User user)
            : base(session)
        {
            Contract.Requires<ArgumentNullException>(session != null);
            Contract.Requires<ArgumentNullException>(user != null);

            lock (NativeMethods.LibraryLock)
            {
                this.Handle = NativeMethods.sp_link_create_from_user(user.Handle);
            }
            this.Initialize();
        }

        /// <summary>
        /// Increments the reference count of the underlying libspotify link object.
        /// </summary>
        public override void AddRef()
        {
            lock (NativeMethods.LibraryLock)
            {
                NativeMethods.sp_link_add_ref(this.Handle);
            }
        }

        /// <summary>
        /// Resolves the <see cref="Link"/> into an <see cref="Album"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">The <see cref="LinkType"/> didn't match.</exception>
        /// <returns>The <see cref="Album"/>.</returns>
        public Album AsAlbum()
        {
            Contract.Ensures(Contract.Result<Album>() != null);

            Album result;
            if (!this.TryResolveAsAlbum(out result))
            {
                throw new InvalidOperationException("The link does not point to an album. It cannot be resolved.");
            }
            return result;
        }

        /// <summary>
        /// Resolves the <see cref="Link"/> into an <see cref="Artist"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">The <see cref="LinkType"/> didn't match.</exception>
        /// <returns>The <see cref="Artist"/>.</returns>
        public Artist AsArtist()
        {
            Contract.Ensures(Contract.Result<Artist>() != null);

            Artist result;
            if (!this.TryResolveAsArtist(out result))
            {
                throw new InvalidOperationException("The link does not point to an artist. It cannot be resolved.");
            }
            return result;
        }

        /// <summary>
        /// Converts the <see cref="Link"/> into its textual representation.
        /// </summary>
        /// <returns>The <see cref="Link"/>s textual representation.</returns>
        /// <seealso cref="M:ToString"/>
        public string AsString()
        {
            Contract.Ensures(Contract.Result<string>() != null);

            lock (NativeMethods.LibraryLock)
            {
                IntPtr handle = this.Handle;
                int size = 128;
                for (int i = 0; i < 3; i++, size *= 2)
                {
                    IntPtr buf = IntPtr.Zero;
                    try
                    {
                        buf = Marshal.AllocHGlobal(size);
                        int allocatedBytes = 0;
                        if ((allocatedBytes = NativeMethods.sp_link_as_string(handle, buf, size)) < size)
                        {
                            // Since the method is a fuckin' son of a bitch we need to read the string by hand, AsString probably won't work here.
                            byte[] buffer = new byte[allocatedBytes];
                            Marshal.Copy(buf, buffer, 0, allocatedBytes);
                            return Encoding.UTF8.GetString(buffer);
                        }
                    }
                    finally
                    {
                        Marshal.FreeHGlobal(buf);
                    }
                }

                throw new InvalidOperationException("The required characters for sp_link_as_string could not be allocated. This is weird behaviour and shouldn't happen normally.");
            }
        }

        /// <summary>
        /// Resolves the <see cref="Link"/> into an <see cref="Track"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">The <see cref="LinkType"/> didn't match.</exception>
        /// <returns>The <see cref="Track"/>.</returns>
        public Track AsTrack()
        {
            Contract.Ensures(Contract.Result<Track>() != null);

            Track result;
            if (!this.TryResolveAsTrack(out result))
            {
                throw new InvalidOperationException("The link does not point to a track. It cannot be resolved.");
            }
            return result;
        }

        /// <summary>
        /// Converts the <see cref="Link"/> into its textual representation.
        /// </summary>
        /// <returns>The <see cref="Link"/>s textual representation.</returns>
        /// <seealso cref="M:AsString"/>
        public override string ToString()
        {
            return this.AsString();
        }

        /// <summary>
        /// Tries to resolve the <see cref="Link"/> to an <see cref="Album"/> and returns success or failure.
        /// </summary>
        /// <param name="album">The <see cref="Album"/>, if the <see cref="Link"/> could be resolved.</param>
        /// <returns><c>true</c> if the <see cref="Link"/> could be resolved, otherwise <c>false</c>.</returns>
        public bool TryResolveAsAlbum(out Album album)
        {
            Contract.Ensures(!Contract.Result<bool>() || Contract.ValueAtReturn(out album) != null);

            lock (NativeMethods.LibraryLock)
            {
                IntPtr handle = NativeMethods.sp_link_as_album(this.Handle);
                if (handle != null)
                {
                    album = new Album(this.Session, handle);
                    return true;
                }
                else
                {
                    album = null;
                    return false;
                }
            }
        }

        /// <summary>
        /// Tries to resolve the <see cref="Link"/> to an <see cref="Artist"/> and returns success or failure.
        /// </summary>
        /// <param name="artist">The <see cref="Artist"/>, if the <see cref="Link"/> could be resolved.</param>
        /// <returns><c>true</c> if the <see cref="Link"/> could be resolved, otherwise <c>false</c>.</returns>
        public bool TryResolveAsArtist(out Artist artist)
        {
            Contract.Ensures(!Contract.Result<bool>() || Contract.ValueAtReturn(out artist) != null);

            lock (NativeMethods.LibraryLock)
            {
                IntPtr handle = NativeMethods.sp_link_as_artist(this.Handle);
                if (handle != null)
                {
                    artist = new Artist(this.Session, handle);
                    return true;
                }
                else
                {
                    artist = null;
                    return false;
                }
            }
        }

        /// <summary>
        /// Tries to resolve the <see cref="Link"/> to an <see cref="Track"/> and returns success or failure.
        /// </summary>
        /// <param name="track">The <see cref="Track"/>, if the <see cref="Link"/> could be resolved.</param>
        /// <returns><c>true</c> if the <see cref="Link"/> could be resolved, otherwise <c>false</c>.</returns>
        public bool TryResolveAsTrack(out Track track)
        {
            Contract.Ensures(!Contract.Result<bool>() || Contract.ValueAtReturn(out track) != null);

            lock (NativeMethods.LibraryLock)
            {
                IntPtr handle = NativeMethods.sp_link_as_track(this.Handle);
                if (handle != null)
                {
                    track = new Track(this.Session, handle);
                    return true;
                }
                else
                {
                    track = null;
                    return false;
                }
            }
        }

        /// <summary>
        /// Tries to resolve the <see cref="Link"/> to an <see cref="Track"/> and returns success or failure.
        /// </summary>
        /// <param name="track">The <see cref="Track"/>, if the <see cref="Link"/> could be resolved.</param>
        /// <param name="offset">The offset into the <see cref="Track"/>.</param>
        /// <returns><c>true</c> if the <see cref="Link"/> could be resolved, otherwise <c>false</c>.</returns>
        public bool TryResolveAsTrack(out Track track, out TimeSpan offset)
        {
            Contract.Ensures(!Contract.Result<bool>() || Contract.ValueAtReturn(out track) != null);

            lock (NativeMethods.LibraryLock)
            {
                offset = TimeSpan.Zero;
                int offsetMs = 0;
                IntPtr handle = NativeMethods.sp_link_as_track_and_offset(this.Handle, ref offsetMs);
                if (handle != null)
                {
                    offset = TimeSpan.FromMilliseconds(offsetMs);
                    track = new Track(this.Session, handle);
                    return true;
                }
                else
                {
                    track = null;
                    return false;
                }
            }
        }

        /// <summary>
        /// Tries to resolve the <see cref="Link"/> to an <see cref="User"/> and returns success or failure.
        /// </summary>
        /// <param name="user">The <see cref="User"/>, if the <see cref="Link"/> could be resolved.</param>
        /// <returns><c>true</c> if the <see cref="Link"/> could be resolved, otherwise <c>false</c>.</returns>
        public bool TryResolveAsUser(out User user)
        {
            Contract.Ensures(!Contract.Result<bool>() || Contract.ValueAtReturn(out user) != null);

            lock (NativeMethods.LibraryLock)
            {
                IntPtr handle = NativeMethods.sp_link_as_user(this.Handle);
                if (handle != null)
                {
                    user = new User(this.Session, handle);
                    return true;
                }
                else
                {
                    user = null;
                    return false;
                }
            }
        }

        /// <summary>
        /// Disposes the link decrementing the reference count of the underlying libspotify link object.
        /// </summary>
        /// <param name="disposing">Indicates whether to release managed resources as well.</param>
        protected override void Dispose(bool disposing)
        {
            lock (NativeMethods.LibraryLock)
            {
                NativeMethods.sp_link_release(this.Handle);
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Performs additional initialization work that needs to be done by every constructor.
        /// </summary>
        private void Initialize()
        {
            this.IsLoaded = true;
        }
    }
}
