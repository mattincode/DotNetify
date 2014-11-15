using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DotNetify
{
    internal delegate void albumbrowse_complete_cb(IntPtr result, IntPtr userData);

    internal delegate void artistbrowse_complete_cb(IntPtr result, IntPtr userData);

    internal delegate void image_loaded_cb(IntPtr image, IntPtr userData);

    internal delegate void search_complete_cb(IntPtr result, IntPtr userData);

    internal delegate void toplistbrowse_complete_cb(IntPtr result, IntPtr userData);

    internal delegate void inboxpost_complete_cb(IntPtr result, IntPtr userData);


    internal delegate void connection_error(IntPtr session, Result error);

    internal delegate void connectionstate_updated(IntPtr session);

    internal delegate void credentials_blob_updated(IntPtr session, IntPtr blob);

    internal delegate void end_of_track(IntPtr session);

    internal delegate void get_audio_buffer_stats(IntPtr session, ref AudioBufferStatistics stats);

    internal delegate void logged_in(IntPtr session, Result error);

    internal delegate void logged_out(IntPtr session);

    internal delegate void log_message(IntPtr session, IntPtr data);

    internal delegate void message_to_user(IntPtr session, IntPtr message);

    internal delegate void metadata_updated(IntPtr session);

    internal delegate int music_delivery(IntPtr session, ref AudioFormat format, IntPtr frames, int num_frames);

    internal delegate void notify_main_thread(IntPtr session);

    internal delegate void offline_error(IntPtr session, Result error);

    internal delegate void offline_status_updated(IntPtr session);

    internal delegate void play_token_lost(IntPtr session);

    internal delegate void private_session_mode_changed(IntPtr session, [MarshalAs(UnmanagedType.I1)] bool is_private);

    internal delegate void scrobble_error(IntPtr session, Result error);

    internal delegate void start_playback(IntPtr session);

    internal delegate void stop_playback(IntPtr session);

    internal delegate void streaming_error(IntPtr session, Result error);

    internal delegate void userinfo_updated(IntPtr session);

    internal static class NativeMethods
    {
        private static readonly object _LibraryLock = new object();

        public static object LibraryLock
        {
            get
            {
                Contract.Ensures(Contract.Result<object>() != null);

                return _LibraryLock;
            }
        }

        [DllImport("libspotify")]
        internal static extern IntPtr sp_error_message(Result error);

        [DllImport("libspotify")]
        internal static extern Result sp_session_create(ref sp_session_config config, ref IntPtr sess);

        [DllImport("libspotify")]
        internal static extern Result sp_session_release(IntPtr sess);

        [DllImport("libspotify")]
        internal static extern Result sp_session_login(IntPtr session, IntPtr username, IntPtr password, [MarshalAs(UnmanagedType.I1)] bool remember_me, IntPtr blob);

        [DllImport("libspotify")]
        internal static extern Result sp_session_relogin(IntPtr session);

        [DllImport("libspotify")]
        internal static extern int sp_session_remembered_user(IntPtr session, IntPtr buffer, UIntPtr buffer_size);

        [DllImport("libspotify")]
        internal static extern IntPtr sp_session_user_name(IntPtr session);

        [DllImport("libspotify")]
        internal static extern Result sp_session_forget_me(IntPtr session);

        [DllImport("libspotify")]
        internal static extern IntPtr sp_session_user(IntPtr session);

        [DllImport("libspotify")]
        internal static extern Result sp_session_logout(IntPtr session);

        [DllImport("libspotify")]
        internal static extern Result sp_session_flush_caches(IntPtr session);

        [DllImport("libspotify")]
        internal static extern ConnectionState sp_session_connectionstate(IntPtr session);

        [DllImport("libspotify")]
        internal static extern IntPtr sp_session_userdata(IntPtr session);

        [DllImport("libspotify")]
        internal static extern Result sp_session_set_cache_size(IntPtr session, UIntPtr size);

        [DllImport("libspotify")]
        internal static extern Result sp_session_process_events(IntPtr session, ref int next_timeout);

        [DllImport("libspotify")]
        internal static extern Result sp_session_player_load(IntPtr session, IntPtr track);

        [DllImport("libspotify")]
        internal static extern Result sp_session_player_seek(IntPtr session, int offset);

        [DllImport("libspotify")]
        internal static extern Result sp_session_player_play(IntPtr session, [MarshalAs(UnmanagedType.I1)] bool play);

        [DllImport("libspotify")]
        internal static extern Result sp_session_player_unload(IntPtr session);

        [DllImport("libspotify")]
        internal static extern Result sp_session_player_prefetch(IntPtr session, IntPtr track);

        [DllImport("libspotify")]
        internal static extern IntPtr sp_session_playlistcontainer(IntPtr session);

        [DllImport("libspotify")]
        internal static extern IntPtr sp_session_inbox_create(IntPtr session);

        [DllImport("libspotify")]
        internal static extern IntPtr sp_session_starred_create(IntPtr session);

        [DllImport("libspotify")]
        internal static extern IntPtr sp_session_starred_for_user_create(IntPtr session, IntPtr canonical_username);

        [DllImport("libspotify")]
        internal static extern IntPtr sp_session_publishedcontainer_for_user_create(IntPtr session, IntPtr canonical_username);

        [DllImport("libspotify")]
        internal static extern Result sp_session_preferred_bitrate(IntPtr session, Bitrate bitrate);

        [DllImport("libspotify")]
        internal static extern Result sp_session_preferred_offline_bitrate(IntPtr session, Bitrate bitrate, [MarshalAs(UnmanagedType.I1)] bool allow_resync);

        [DllImport("libspotify")]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool sp_session_get_volume_normalization(IntPtr session);

        [DllImport("libspotify")]
        internal static extern Result sp_session_set_volume_normalization(IntPtr session, [MarshalAs(UnmanagedType.I1)] bool on);

        [DllImport("libspotify")]
        internal static extern Result sp_session_set_private_session(IntPtr session, [MarshalAs(UnmanagedType.I1)] bool enabled);

        [DllImport("libspotify")]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool sp_session_is_private_session(IntPtr session);

        [DllImport("libspotify")]
        internal static extern Result sp_session_set_scrobbling(IntPtr session, SocialProvider provider, ScrobblingState state);

        [DllImport("libspotify")]
        internal static extern Result sp_session_is_scrobbling(IntPtr session, SocialProvider provider, ref ScrobblingState state);

        [DllImport("libspotify")]
        internal static extern Result sp_session_is_scrobbling_possible(IntPtr session, SocialProvider provider, [MarshalAs(UnmanagedType.I1)] ref bool @out);

        [DllImport("libspotify")]
        internal static extern Result sp_session_set_social_credentials(IntPtr session, SocialProvider provider, IntPtr username, IntPtr password);

        [DllImport("libspotify")]
        internal static extern Result sp_session_set_connection_type(IntPtr session, ConnectionType type);

        [DllImport("libspotify")]
        internal static extern Result sp_session_set_connection_rules(IntPtr session, ConnectionRules rules);

        [DllImport("libspotify")]
        internal static extern int sp_offline_tracks_to_sync(IntPtr session);

        [DllImport("libspotify")]
        internal static extern int sp_offline_num_playlists(IntPtr session);

        [DllImport("libspotify")]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool sp_offline_sync_get_status(IntPtr session, IntPtr status);

        [DllImport("libspotify")]
        internal static extern int sp_offline_time_left(IntPtr session);

        [DllImport("libspotify")]
        internal static extern int sp_session_user_country(IntPtr session);

        [DllImport("libspotify")]
        internal static extern IntPtr sp_link_create_from_string(IntPtr link);

        [DllImport("libspotify")]
        internal static extern IntPtr sp_link_create_from_track(IntPtr track, int offset);

        [DllImport("libspotify")]
        internal static extern IntPtr sp_link_create_from_album(IntPtr album);

        [DllImport("libspotify")]
        internal static extern IntPtr sp_link_create_from_album_cover(IntPtr album, ImageSize size);

        [DllImport("libspotify")]
        internal static extern IntPtr sp_link_create_from_artist(IntPtr artist);

        [DllImport("libspotify")]
        internal static extern IntPtr sp_link_create_from_artist_portrait(IntPtr artist, ImageSize size);

        [DllImport("libspotify")]
        internal static extern IntPtr sp_link_create_from_artistbrowse_portrait(IntPtr arb, int index);

        [DllImport("libspotify")]
        internal static extern IntPtr sp_link_create_from_search(IntPtr search);

        [DllImport("libspotify")]
        internal static extern IntPtr sp_link_create_from_playlist(IntPtr playlist);

        [DllImport("libspotify")]
        internal static extern IntPtr sp_link_create_from_user(IntPtr user);

        [DllImport("libspotify")]
        internal static extern IntPtr sp_link_create_from_image(IntPtr image);

        [DllImport("libspotify")]
        internal static extern int sp_link_as_string(IntPtr link, IntPtr buffer, int buffer_size);

        [DllImport("libspotify")]
        internal static extern LinkType sp_link_type(IntPtr link);

        [DllImport("libspotify")]
        internal static extern IntPtr sp_link_as_track(IntPtr link);

        [DllImport("libspotify")]
        internal static extern IntPtr sp_link_as_track_and_offset(IntPtr link, ref int offset);

        [DllImport("libspotify")]
        internal static extern IntPtr sp_link_as_album(IntPtr link);

        [DllImport("libspotify")]
        internal static extern IntPtr sp_link_as_artist(IntPtr link);

        [DllImport("libspotify")]
        internal static extern IntPtr sp_link_as_user(IntPtr link);

        [DllImport("libspotify")]
        internal static extern Result sp_link_add_ref(IntPtr link);

        [DllImport("libspotify")]
        internal static extern Result sp_link_release(IntPtr link);

        [DllImport("libspotify")]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool sp_track_is_loaded(IntPtr track);

        [DllImport("libspotify")]
        internal static extern Result sp_track_error(IntPtr track);

        [DllImport("libspotify")]
        internal static extern TrackOfflineStatus sp_track_offline_get_status(IntPtr track);

        [DllImport("libspotify")]
        internal static extern TrackAvailability sp_track_get_availability(IntPtr session, IntPtr track);

        [DllImport("libspotify")]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool sp_track_is_local(IntPtr session, IntPtr track);

        [DllImport("libspotify")]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool sp_track_is_autolinked(IntPtr session, IntPtr track);

        [DllImport("libspotify")]
        internal static extern IntPtr sp_track_get_playable(IntPtr session, IntPtr track);

        [DllImport("libspotify")]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool sp_track_is_placeholder(IntPtr track);

        [DllImport("libspotify")]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool sp_track_is_starred(IntPtr session, IntPtr track);

        [DllImport("libspotify")]
        internal static extern Result sp_track_set_starred(IntPtr session, IntPtr tracks, int num_tracks, [MarshalAs(UnmanagedType.I1)] bool star);

        [DllImport("libspotify")]
        internal static extern int sp_track_num_artists(IntPtr track);

        [DllImport("libspotify")]
        internal static extern IntPtr sp_track_artist(IntPtr track, int index);

        [DllImport("libspotify")]
        internal static extern IntPtr sp_track_album(IntPtr track);

        [DllImport("libspotify")]
        internal static extern IntPtr sp_track_name(IntPtr track);

        [DllImport("libspotify")]
        internal static extern int sp_track_duration(IntPtr track);

        [DllImport("libspotify")]
        internal static extern int sp_track_popularity(IntPtr track);

        [DllImport("libspotify")]
        internal static extern int sp_track_disc(IntPtr track);

        [DllImport("libspotify")]
        internal static extern int sp_track_index(IntPtr track);

        [DllImport("libspotify")]
        internal static extern IntPtr sp_localtrack_create(IntPtr artist, IntPtr title, IntPtr album, int length);

        [DllImport("libspotify")]
        internal static extern Result sp_track_add_ref(IntPtr track);

        [DllImport("libspotify")]
        internal static extern Result sp_track_release(IntPtr track);

        [DllImport("libspotify")]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool sp_album_is_loaded(IntPtr album);

        [DllImport("libspotify")]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool sp_album_is_available(IntPtr album);

        [DllImport("libspotify")]
        internal static extern IntPtr sp_album_artist(IntPtr album);

        [DllImport("libspotify")]
        internal static extern IntPtr sp_album_cover(IntPtr album, ImageSize size);

        [DllImport("libspotify")]
        internal static extern IntPtr sp_album_name(IntPtr album);

        [DllImport("libspotify")]
        internal static extern int sp_album_year(IntPtr album);

        [DllImport("libspotify")]
        internal static extern AlbumType sp_album_type(IntPtr album);

        [DllImport("libspotify")]
        internal static extern Result sp_album_add_ref(IntPtr album);

        [DllImport("libspotify")]
        internal static extern Result sp_album_release(IntPtr album);

        [DllImport("libspotify")]
        internal static extern IntPtr sp_artist_name(IntPtr artist);

        [DllImport("libspotify")]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool sp_artist_is_loaded(IntPtr artist);

        [DllImport("libspotify")]
        internal static extern IntPtr sp_artist_portrait(IntPtr artist, ImageSize size);

        [DllImport("libspotify")]
        internal static extern Result sp_artist_add_ref(IntPtr artist);

        [DllImport("libspotify")]
        internal static extern Result sp_artist_release(IntPtr artist);

        [DllImport("libspotify")]
        internal static extern IntPtr sp_albumbrowse_create(IntPtr session, IntPtr album, albumbrowse_complete_cb callback, IntPtr userdata);

        [DllImport("libspotify")]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool sp_albumbrowse_is_loaded(IntPtr alb);

        [DllImport("libspotify")]
        internal static extern Result sp_albumbrowse_error(IntPtr alb);

        [DllImport("libspotify")]
        internal static extern IntPtr sp_albumbrowse_album(IntPtr alb);

        [DllImport("libspotify")]
        internal static extern IntPtr sp_albumbrowse_artist(IntPtr alb);

        [DllImport("libspotify")]
        internal static extern int sp_albumbrowse_num_copyrights(IntPtr alb);

        [DllImport("libspotify")]
        internal static extern IntPtr sp_albumbrowse_copyright(IntPtr alb, int index);

        [DllImport("libspotify")]
        internal static extern int sp_albumbrowse_num_tracks(IntPtr alb);

        [DllImport("libspotify")]
        internal static extern IntPtr sp_albumbrowse_track(IntPtr alb, int index);

        [DllImport("libspotify")]
        internal static extern IntPtr sp_albumbrowse_review(IntPtr alb);

        [DllImport("libspotify")]
        internal static extern int sp_albumbrowse_backend_request_duration(IntPtr alb);

        [DllImport("libspotify")]
        internal static extern Result sp_albumbrowse_add_ref(IntPtr alb);

        [DllImport("libspotify")]
        internal static extern Result sp_albumbrowse_release(IntPtr alb);

        [DllImport("libspotify")]
        internal static extern IntPtr sp_artistbrowse_create(IntPtr session, IntPtr artist, ArtistBrowseType type, artistbrowse_complete_cb callback, IntPtr userdata);

        [DllImport("libspotify")]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool sp_artistbrowse_is_loaded(IntPtr arb);

        [DllImport("libspotify")]
        internal static extern Result sp_artistbrowse_error(IntPtr arb);

        [DllImport("libspotify")]
        internal static extern IntPtr sp_artistbrowse_artist(IntPtr arb);

        [DllImport("libspotify")]
        internal static extern int sp_artistbrowse_num_portraits(IntPtr arb);

        [DllImport("libspotify")]
        internal static extern IntPtr sp_artistbrowse_portrait(IntPtr arb, int index);

        [DllImport("libspotify")]
        internal static extern int sp_artistbrowse_num_tracks(IntPtr arb);

        [DllImport("libspotify")]
        internal static extern IntPtr sp_artistbrowse_track(IntPtr arb, int index);

        [DllImport("libspotify")]
        internal static extern int sp_artistbrowse_num_tophit_tracks(IntPtr arb);

        [DllImport("libspotify")]
        internal static extern IntPtr sp_artistbrowse_tophit_track(IntPtr arb, int index);

        [DllImport("libspotify")]
        internal static extern int sp_artistbrowse_num_albums(IntPtr arb);

        [DllImport("libspotify")]
        internal static extern IntPtr sp_artistbrowse_album(IntPtr arb, int index);

        [DllImport("libspotify")]
        internal static extern int sp_artistbrowse_num_similar_artists(IntPtr arb);

        [DllImport("libspotify")]
        internal static extern IntPtr sp_artistbrowse_similar_artist(IntPtr arb, int index);

        [DllImport("libspotify")]
        internal static extern IntPtr sp_artistbrowse_biography(IntPtr arb);

        [DllImport("libspotify")]
        internal static extern int sp_artistbrowse_backend_request_duration(IntPtr arb);

        [DllImport("libspotify")]
        internal static extern Result sp_artistbrowse_add_ref(IntPtr arb);

        [DllImport("libspotify")]
        internal static extern Result sp_artistbrowse_release(IntPtr arb);

        [DllImport("libspotify")]
        internal static extern IntPtr sp_image_create(IntPtr session, IntPtr image_id);

        [DllImport("libspotify")]
        internal static extern IntPtr sp_image_create_from_link(IntPtr session, IntPtr l);

        [DllImport("libspotify")]
        internal static extern Result sp_image_add_load_callback(IntPtr image, image_loaded_cb callback, IntPtr userdata);

        [DllImport("libspotify")]
        internal static extern Result sp_image_remove_load_callback(IntPtr image, image_loaded_cb callback, IntPtr userdata);

        [DllImport("libspotify")]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool sp_image_is_loaded(IntPtr image);

        [DllImport("libspotify")]
        internal static extern Result sp_image_error(IntPtr image);

        [DllImport("libspotify")]
        internal static extern ImageFormat sp_image_format(IntPtr image);

        [DllImport("libspotify")]
        internal static extern IntPtr sp_image_data(IntPtr image, ref UIntPtr data_size);

        [DllImport("libspotify")]
        internal static extern IntPtr sp_image_image_id(IntPtr image);

        [DllImport("libspotify")]
        internal static extern Result sp_image_add_ref(IntPtr image);

        [DllImport("libspotify")]
        internal static extern Result sp_image_release(IntPtr image);

        [DllImport("libspotify")]
        internal static extern IntPtr sp_search_create(IntPtr session, IntPtr query, int track_offset, int track_count, int album_offset, int album_count, int artist_offset, int artist_count, int playlist_offset, int playlist_count, SearchType search_type, search_complete_cb callback, IntPtr userdata);

        [DllImport("libspotify")]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool sp_search_is_loaded(IntPtr search);

        [DllImport("libspotify")]
        internal static extern Result sp_search_error(IntPtr search);

        [DllImport("libspotify")]
        internal static extern int sp_search_num_tracks(IntPtr search);

        [DllImport("libspotify")]
        internal static extern IntPtr sp_search_track(IntPtr search, int index);

        [DllImport("libspotify")]
        internal static extern int sp_search_num_albums(IntPtr search);

        [DllImport("libspotify")]
        internal static extern IntPtr sp_search_album(IntPtr search, int index);

        [DllImport("libspotify")]
        internal static extern int sp_search_num_playlists(IntPtr search);

        [DllImport("libspotify")]
        internal static extern IntPtr sp_search_playlist(IntPtr search, int index);

        [DllImport("libspotify")]
        internal static extern IntPtr sp_search_playlist_name(IntPtr search, int index);

        [DllImport("libspotify")]
        internal static extern IntPtr sp_search_playlist_uri(IntPtr search, int index);

        [DllImport("libspotify")]
        internal static extern IntPtr sp_search_playlist_image_uri(IntPtr search, int index);

        [DllImport("libspotify")]
        internal static extern int sp_search_num_artists(IntPtr search);

        [DllImport("libspotify")]
        internal static extern IntPtr sp_search_artist(IntPtr search, int index);

        [DllImport("libspotify")]
        internal static extern IntPtr sp_search_query(IntPtr search);

        [DllImport("libspotify")]
        internal static extern IntPtr sp_search_did_you_mean(IntPtr search);

        [DllImport("libspotify")]
        internal static extern int sp_search_total_tracks(IntPtr search);

        [DllImport("libspotify")]
        internal static extern int sp_search_total_albums(IntPtr search);

        [DllImport("libspotify")]
        internal static extern int sp_search_total_artists(IntPtr search);

        [DllImport("libspotify")]
        internal static extern int sp_search_total_playlists(IntPtr search);

        [DllImport("libspotify")]
        internal static extern Result sp_search_add_ref(IntPtr search);

        [DllImport("libspotify")]
        internal static extern Result sp_search_release(IntPtr search);

        [DllImport("libspotify")]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool sp_playlist_is_loaded(IntPtr playlist);

        [DllImport("libspotify")]
        internal static extern Result sp_playlist_add_callbacks(IntPtr playlist, IntPtr callbacks, IntPtr userdata);

        [DllImport("libspotify")]
        internal static extern Result sp_playlist_remove_callbacks(IntPtr playlist, IntPtr callbacks, IntPtr userdata);

        [DllImport("libspotify")]
        internal static extern int sp_playlist_num_tracks(IntPtr playlist);

        [DllImport("libspotify")]
        internal static extern IntPtr sp_playlist_track(IntPtr playlist, int index);

        [DllImport("libspotify")]
        internal static extern int sp_playlist_track_create_time(IntPtr playlist, int index);

        [DllImport("libspotify")]
        internal static extern IntPtr sp_playlist_track_creator(IntPtr playlist, int index);

        [DllImport("libspotify")]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool sp_playlist_track_seen(IntPtr playlist, int index);

        [DllImport("libspotify")]
        internal static extern Result sp_playlist_track_set_seen(IntPtr playlist, int index, [MarshalAs(UnmanagedType.I1)] bool seen);

        [DllImport("libspotify")]
        internal static extern IntPtr sp_playlist_track_message(IntPtr playlist, int index);

        [DllImport("libspotify")]
        internal static extern IntPtr sp_playlist_name(IntPtr playlist);

        [DllImport("libspotify")]
        internal static extern Result sp_playlist_rename(IntPtr playlist, IntPtr new_name);

        [DllImport("libspotify")]
        internal static extern IntPtr sp_playlist_owner(IntPtr playlist);

        [DllImport("libspotify")]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool sp_playlist_is_collaborative(IntPtr playlist);

        [DllImport("libspotify")]
        internal static extern Result sp_playlist_set_collaborative(IntPtr playlist, [MarshalAs(UnmanagedType.I1)] bool collaborative);

        [DllImport("libspotify")]
        internal static extern Result sp_playlist_set_autolink_tracks(IntPtr playlist, [MarshalAs(UnmanagedType.I1)] bool link);

        [DllImport("libspotify")]
        internal static extern IntPtr sp_playlist_get_description(IntPtr playlist);

        [DllImport("libspotify")]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool sp_playlist_get_image(IntPtr playlist, IntPtr image);

        [DllImport("libspotify")]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool sp_playlist_has_pending_changes(IntPtr playlist);

        [DllImport("libspotify")]
        internal static extern Result sp_playlist_add_tracks(IntPtr playlist, IntPtr tracks, int num_tracks, int position, IntPtr session);

        [DllImport("libspotify")]
        internal static extern Result sp_playlist_remove_tracks(IntPtr playlist, IntPtr tracks, int num_tracks);

        [DllImport("libspotify")]
        internal static extern Result sp_playlist_reorder_tracks(IntPtr playlist, IntPtr tracks, int num_tracks, int new_position);

        [DllImport("libspotify")]
        internal static extern int sp_playlist_num_subscribers(IntPtr playlist);

        [DllImport("libspotify")]
        internal static extern IntPtr sp_playlist_subscribers(IntPtr playlist);

        [DllImport("libspotify")]
        internal static extern Result sp_playlist_subscribers_free(IntPtr subscribers);

        [DllImport("libspotify")]
        internal static extern Result sp_playlist_update_subscribers(IntPtr session, IntPtr playlist);

        [DllImport("libspotify")]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool sp_playlist_is_in_ram(IntPtr session, IntPtr playlist);

        [DllImport("libspotify")]
        internal static extern Result sp_playlist_set_in_ram(IntPtr session, IntPtr playlist, [MarshalAs(UnmanagedType.I1)] bool in_ram);

        [DllImport("libspotify")]
        internal static extern IntPtr sp_playlist_create(IntPtr session, IntPtr link);

        [DllImport("libspotify")]
        internal static extern Result sp_playlist_set_offline_mode(IntPtr session, IntPtr playlist, [MarshalAs(UnmanagedType.I1)] bool offline);

        [DllImport("libspotify")]
        internal static extern PlaylistOfflineStatus sp_playlist_get_offline_status(IntPtr session, IntPtr playlist);

        [DllImport("libspotify")]
        internal static extern int sp_playlist_get_offline_download_completed(IntPtr session, IntPtr playlist);

        [DllImport("libspotify")]
        internal static extern Result sp_playlist_add_ref(IntPtr playlist);

        [DllImport("libspotify")]
        internal static extern Result sp_playlist_release(IntPtr playlist);

        [DllImport("libspotify")]
        internal static extern Result sp_playlistcontainer_add_callbacks(IntPtr pc, IntPtr callbacks, IntPtr userdata);

        [DllImport("libspotify")]
        internal static extern Result sp_playlistcontainer_remove_callbacks(IntPtr pc, IntPtr callbacks, IntPtr userdata);

        [DllImport("libspotify")]
        internal static extern int sp_playlistcontainer_num_playlists(IntPtr pc);

        [DllImport("libspotify")]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool sp_playlistcontainer_is_loaded(IntPtr pc);

        [DllImport("libspotify")]
        internal static extern IntPtr sp_playlistcontainer_playlist(IntPtr pc, int index);

        [DllImport("libspotify")]
        internal static extern PlaylistType sp_playlistcontainer_playlist_type(IntPtr pc, int index);

        [DllImport("libspotify")]
        internal static extern Result sp_playlistcontainer_playlist_folder_name(IntPtr pc, int index, IntPtr buffer, int buffer_size);

        [DllImport("libspotify")]
        internal static extern ulong sp_playlistcontainer_playlist_folder_id(IntPtr pc, int index);

        [DllImport("libspotify")]
        internal static extern IntPtr sp_playlistcontainer_add_new_playlist(IntPtr pc, IntPtr name);

        [DllImport("libspotify")]
        internal static extern IntPtr sp_playlistcontainer_add_playlist(IntPtr pc, IntPtr link);

        [DllImport("libspotify")]
        internal static extern Result sp_playlistcontainer_remove_playlist(IntPtr pc, int index);

        [DllImport("libspotify")]
        internal static extern Result sp_playlistcontainer_move_playlist(IntPtr pc, int index, int new_position, [MarshalAs(UnmanagedType.I1)] bool dry_run);

        [DllImport("libspotify")]
        internal static extern Result sp_playlistcontainer_add_folder(IntPtr pc, int index, IntPtr name);

        [DllImport("libspotify")]
        internal static extern IntPtr sp_playlistcontainer_owner(IntPtr pc);

        [DllImport("libspotify")]
        internal static extern Result sp_playlistcontainer_add_ref(IntPtr pc);

        [DllImport("libspotify")]
        internal static extern Result sp_playlistcontainer_release(IntPtr pc);

        [DllImport("libspotify")]
        internal static extern int sp_playlistcontainer_get_unseen_tracks(IntPtr pc, IntPtr playlist, IntPtr tracks, int num_tracks);

        [DllImport("libspotify")]
        internal static extern int sp_playlistcontainer_clear_unseen_tracks(IntPtr pc, IntPtr playlist);

        [DllImport("libspotify")]
        internal static extern IntPtr sp_user_canonical_name(IntPtr user);

        [DllImport("libspotify")]
        internal static extern IntPtr sp_user_display_name(IntPtr user);

        [DllImport("libspotify")]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool sp_user_is_loaded(IntPtr user);

        [DllImport("libspotify")]
        internal static extern Result sp_user_add_ref(IntPtr user);

        [DllImport("libspotify")]
        internal static extern Result sp_user_release(IntPtr user);

        [DllImport("libspotify")]
        internal static extern IntPtr sp_toplistbrowse_create(IntPtr session, ToplistType type, ToplistRegion region, IntPtr username, toplistbrowse_complete_cb callback, IntPtr userdata);

        [DllImport("libspotify")]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool sp_toplistbrowse_is_loaded(IntPtr tlb);

        [DllImport("libspotify")]
        internal static extern Result sp_toplistbrowse_error(IntPtr tlb);

        [DllImport("libspotify")]
        internal static extern Result sp_toplistbrowse_add_ref(IntPtr tlb);

        [DllImport("libspotify")]
        internal static extern Result sp_toplistbrowse_release(IntPtr tlb);

        [DllImport("libspotify")]
        internal static extern int sp_toplistbrowse_num_artists(IntPtr tlb);

        [DllImport("libspotify")]
        internal static extern IntPtr sp_toplistbrowse_artist(IntPtr tlb, int index);

        [DllImport("libspotify")]
        internal static extern int sp_toplistbrowse_num_albums(IntPtr tlb);

        [DllImport("libspotify")]
        internal static extern IntPtr sp_toplistbrowse_album(IntPtr tlb, int index);

        [DllImport("libspotify")]
        internal static extern int sp_toplistbrowse_num_tracks(IntPtr tlb);

        [DllImport("libspotify")]
        internal static extern IntPtr sp_toplistbrowse_track(IntPtr tlb, int index);

        [DllImport("libspotify")]
        internal static extern int sp_toplistbrowse_backend_request_duration(IntPtr tlb);

        [DllImport("libspotify")]
        internal static extern IntPtr sp_inbox_post_tracks(IntPtr session, IntPtr user, IntPtr tracks, int num_tracks, IntPtr message, inboxpost_complete_cb callback, IntPtr userdata);

        [DllImport("libspotify")]
        internal static extern Result sp_inbox_error(IntPtr inbox);

        [DllImport("libspotify")]
        internal static extern Result sp_inbox_add_ref(IntPtr inbox);

        [DllImport("libspotify")]
        internal static extern Result sp_inbox_release(IntPtr inbox);

        [DllImport("libspotify")]
        internal static extern IntPtr sp_build_id();

        public struct sp_offline_sync_status
        {
            public int queued_tracks;
            public ulong queued_bytes;
            public int done_tracks;
            public ulong done_bytes;
            public int copied_tracks;
            public ulong copied_bytes;
            public int willnotcopy_tracks;
            public int error_tracks;
            [MarshalAs(UnmanagedType.I1)]
            public bool syncing;
        }

        internal struct sp_session_callbacks
        {
            public logged_in logged_in;
            public logged_out logged_out;
            public metadata_updated metadata_updated;
            public connection_error connection_error;
            public message_to_user message_to_user;
            public notify_main_thread notify_main_thread;
            public music_delivery music_delivery;
            public play_token_lost play_token_lost;
            public log_message log_message;
            public end_of_track end_of_track;
            public streaming_error streaming_error;
            public userinfo_updated userinfo_updated;
            public start_playback start_playback;
            public stop_playback stop_playback;
            public get_audio_buffer_stats get_audio_buffer_stats;
            public offline_status_updated offline_status_updated;
            public offline_error offline_error;
            public credentials_blob_updated credentials_blob_updated;
            public connectionstate_updated connectionstate_updated;
            public scrobble_error scrobble_error;
            public private_session_mode_changed private_session_mode_changed;
        }

        internal struct sp_session_config
        {
            public int api_version;
            public IntPtr cache_location;
            public IntPtr settings_location;
            public IntPtr application_key;
            public UIntPtr application_key_size;
            public IntPtr user_agent;
            public IntPtr callbacks;
            public IntPtr userdata;
            [MarshalAs(UnmanagedType.I1)]
            public bool compress_playlists;
            [MarshalAs(UnmanagedType.I1)]
            public bool dont_save_metadata_for_playlists;
            [MarshalAs(UnmanagedType.I1)]
            public bool initially_unload_playlists;
            public IntPtr device_id;
            public IntPtr proxy;
            public IntPtr proxy_username;
            public IntPtr proxy_password;
            public IntPtr tracefile;
        }
    }
}
