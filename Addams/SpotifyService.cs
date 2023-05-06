using Addams.Entities;
using Addams.Exceptions;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Addams
{
    internal class SpotifyService
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Spotify Api requests
        /// </summary>
        private readonly SpotifyConfig config;

        /// <summary>
        /// Spotify Api requests
        /// </summary>
        private readonly SpotifyApi api;

        /// <summary>
        /// Name of playlist of liked songs
        /// </summary>
        const string LIKED_SONGS = "Liked Songs";

        /// <summary>
        /// Setup SpotifyService to get playlist and track 
        /// Based on configuration file (new or existing)
        /// Then create Api object with config setup
        /// </summary>
        public SpotifyService()
        {
            Logger.Debug("Setup config...");
            config = SpotifyConfig.Get();
            Logger.Debug("Setup Api...");
            api = new SpotifyApi(config);
        }

        /// <summary>
        /// Setup authorization to Spotify Application to get OAUTH2 Token
        /// </summary>
        /// <returns>Token string</returns>
        public async Task<string> RefreshToken()
        {
            Token OAuth2 = await api.Authorize();

            return OAuth2.access_token ?? throw new SpotifyUnauthorizedException();
        }

        // TODO feature OAUTH2 to comment
        public void Update(string accessToken)
        {
            // TODO feature OAUTH2 fusionner en setToken()
            config.Token = accessToken;
            config.Save();

            api.RefreshClient(accessToken);
        }

        /// <summary>
        /// Fetch name of the playlists
        /// </summary>
        /// <returns>List of playlist names</returns>

        public async Task<IEnumerable<Playlist>> GetPlaylistsName()
        {
            // Get playlist data
            Playlists playlistsData = await api.FetchPlaylists();

            List<Playlist> playlists = playlistsData.items.ToList();
            playlists = playlists.Prepend(new Playlist() { name = LIKED_SONGS }).ToList();

            return playlists;
        }

        /// <summary>
        /// Get playlist wanted by the user created or saved in Spotify
        /// </summary>
        /// <param name="playlistSelected">List of playlist wanted</param>
        /// <returns>Playlist's models</returns>
        public async Task<IEnumerable<Models.Playlist>> GetPlaylists(IEnumerable<Entities.Playlist> playlistSelected)
        {
            List<Models.Playlist> playlists = new();

            // Get playlist data
            Playlists playlistsData = await api.FetchPlaylists();

            if (playlistsData.items == null)
            {
                Logger.Warn("No playlists found");
                return new List<Models.Playlist>();
            }

            List<Playlist> playlistToFetch = playlistsData.items.Where(p => playlistSelected.Select(ps => ps.name).Contains(p.name)).ToList();

            Logger.Debug($"Playlist selected: {string.Join("; ", playlistSelected.Select(p => p.name))}");

            // Liked song playlist if choosen or want all playlist
            if (playlistSelected.Select(p => p.name).Contains(LIKED_SONGS))
            {
                Models.Playlist likedPlaylist = await GetLikedTracks();
                playlists.Add(likedPlaylist);
            }

            // Get tracks for each playlist
            foreach (Playlist p in playlistToFetch)
            {
                Models.Playlist playlist = await GetPlaylist(p);
                Logger.Info($"Playlist {playlist}");
                playlists.Add(playlist);
            }
            return playlists;
        }

        /// <summary>
        /// Get playlist from it's id 
        /// </summary>
        /// <param name="playlist">Playlist entity fetched by /playlists request</param>
        /// <returns>Playlist model with tracklist</returns>
        /// <exception cref="SpotifyException"></exception>
        public async Task<Models.Playlist> GetPlaylist(Playlist playlist)
        {

            if (playlist.id == null)
            {
                throw new SpotifyException($"getPlaylistTracks id null of the playlist name : {playlist.name}");
            }

            IEnumerable<Models.Track> tracks = await GetPlaylistTracks(playlist.id);

            if (tracks.ToList().Count == 0)
            {
                Logger.Warn($"No tracks found for the playlist {playlist.name} - id: {playlist.id}");
            }

            return new Models.Playlist
            {
                Id = playlist.id,
                Name = playlist.name,
                Description = playlist.description,
                Href = playlist.href,
                Tracks = tracks
            };
        }

        /// <summary>
        /// Get playlist tracks from it's id 
        /// </summary>
        /// <param name="id">Id ot the playlist</param>
        /// <returns>List of track</returns>
        public async Task<IEnumerable<Models.Track>> GetPlaylistTracks(string playlistId)
        {
            // Get playlist data
            PlaylistTracks? playlistTracks = await api.FetchTracks(playlistId);

            if (playlistTracks == null
                || playlistTracks.tracks == null
                || playlistTracks.tracks.items == null
                )
            {
                return new List<Models.Track>();
            }

            // Get tracks data for each track
            List<Models.Track> tracks = new();
            foreach (TrackItem ti in playlistTracks.tracks.items)
            {
                if (ti.track == null)
                {
                    continue;
                }

                Models.Track track = GetTrack(ti);

                tracks.Add(track);
            }

            return tracks;
        }

        /// <summary>
        /// Create track object base on trackData fetch from spotify API
        /// </summary>
        /// <param name="trackEntity">track data from playlist call</param>
        /// <returns>Track model object</returns>
        public Models.Track GetTrack(TrackItem trackEntity)
        {
            Track track = trackEntity.track;
            if (track.id == null)
            {
                Logger.Warn($"GetTrackData id null of the track name: {track.name}");
                return new Models.Track
                {
                    Name = track.name,
                    Artists = string.Join(",", track.artists.Select(x => x.name))
                };
            }

            return new Models.Track
            {
                Id = track.id,
                Name = track.name,
                Artists = string.Join("|", track.artists.Select(x => x.name)),
                AlbumName = track.album.name,
                AlbumArtistName = string.Join("|", track.album.artists.Select(x => x.name)),
                AlbumReleaseDate = track.album.release_date,
                DiscNumber = track.disc_number,
                TrackNumber = track.track_number,
                Popularity = track.popularity,
                AddedAt = trackEntity.added_at.ToString() ?? "",
                AlbumImageUrl = track.album.images.First().url ?? "",
                TrackPreviewUrl = track.preview_url,
                TrackUri = track.uri,
                ArtistUrl = track.artists.First().uri ?? "",
                AlbumUrl = track.album.uri ?? "",
                Explicit = track.@explicit,
                IsLocal = track.is_local,
                _duration = track.duration_ms,
            };
        }

        /// <summary>
        /// Fetch all the tracks in liked songs on Spotify
        /// </summary>
        /// <returns>Playlist model with liked songs</returns>
        public async Task<Models.Playlist> GetLikedTracks()
        {
            TrackList likedPlaylistEnt = await api.FetchLikedTracks();

            if (likedPlaylistEnt == null
               || likedPlaylistEnt.items == null
               || likedPlaylistEnt.href == null
               )
            {
                return new Models.Playlist();
            }

            // Get tracks data for each track
            List<Models.Track> tracks = new();
            foreach (TrackItem ti in likedPlaylistEnt.items)
            {
                if (ti.track == null)
                {
                    continue;
                }

                Models.Track track = GetTrack(ti);

                tracks.Add(track);
            }

            string href = likedPlaylistEnt.href[..likedPlaylistEnt.href.IndexOf("?")];

            return new Models.Playlist()
            {
                Name = "0 - Liked Songs",
                Description = "Playlist of liked songs",
                Href = href,
                Tracks = tracks,
            };
        }
    }
}
