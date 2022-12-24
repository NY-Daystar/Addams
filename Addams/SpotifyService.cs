
using Newtonsoft.Json;
using System;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using System.Reflection.Metadata;
using Addams.Models;
using Addams.Entities;
using Addams.Api;
using System.Collections.Generic;
using System.Linq;
using Addams.Exceptions;
using System.Collections;
using System.Diagnostics;

namespace Addams.Service
{
    internal class SpotifyService
    {
        /// <summary>
        /// Default OAuth2 token to get access to spotify API data
        /// </summary>
        private static string AUTH_TOKEN = @"BQA8JH8vlHjyOaOsCuBbukEOEU4PfzKeLEgWbQ_7-4VHDHasMUGKfam3TZnTZwa4stynhYxBJcuNnWF5y0lu-Wuy17IodlZXAsHN1N6Kv-6gUiZTinJLu7owAT2lmH2iUZy3vEeoiwDkdgmnVZ3xXqbktSJob8iZ1-O2rA6LaowKwMNs3EZRp_x6_vkPRMFNMPCwhmVGAyQ";

        /// <summary>
        /// Spotify Api requests
        /// </summary>
        private SpotifyApi api;


        /// <summary>
        /// Spotify service to get playlist and track for a user with a default token
        /// </summary>
        /// <param name="user">Spotify user name</param>
        /// <param name="authToken">OAuth2 token authentication generated</param>
        public SpotifyService(string user)
        {
            this.api = new SpotifyApi(user, AUTH_TOKEN);
        }

        /// <summary>
        /// Spotify service to get playlist and track for a user with a specific token
        /// </summary>
        /// <param name="user">Spotify user name</param>
        /// <param name="authToken">OAuth2 token authentication generated</param>
        public SpotifyService(string user, string authToken)
        {
            this.api = new SpotifyApi(user, authToken);
        }

        /// <summary>
        /// Get all playlist created or saved by a user
        /// </summary>
        /// <returns></returns>
        public async Task<List<Models.Playlist>> GetPlaylists()
        {

            List<Models.Playlist> playlists = new List<Models.Playlist>();

            // Get playlist data
            Playlists playlistsData = await this.api.FetchUserPlaylists();


            if (playlistsData == null || playlistsData.items == null)
            {
                Console.WriteLine("No playlists found");
                return new List<Models.Playlist>();
            }

            // playlistsData.items = playlistsData.items.Take(1);

            // Get tracks for each playlist
            foreach (Entities.Playlist p in playlistsData.items)
            {
                Models.Playlist playlist = await this.GetPlaylist(p);
                Console.WriteLine($"Playlist {playlist}");
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
        public async Task<Models.Playlist> GetPlaylist(Entities.Playlist playlist)
        {

            if (playlist.id == null)
            {
                throw new SpotifyException($"getPlaylistTracks id null of the playlist name : {playlist.name}");
            }

            List<Models.Track> tracks = await this.GetPlaylistTracks(playlist.id);

            if (tracks.Count == 0)
            {
                Console.WriteLine($"No tracks found for the playlist {playlist.name} - id: {playlist.id}");
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
        public async Task<List<Models.Track>> GetPlaylistTracks(string id)
        {
            // Get playlist data
            PlaylistTracks? playlistTracks = await this.api.FetchPlaylistTracks(id);

            if (playlistTracks == null
                || playlistTracks.tracks == null
                || playlistTracks.tracks.items == null
                )
            {
                return new List<Models.Track>();
            }

            // Get tracks data for each track
            List<Models.Track> tracks = new List<Models.Track>();
            foreach (Entities.TrackItem ti in playlistTracks.tracks.items)
            {
                if (ti.track == null)
                    continue;
                Models.Track track = await this.GetTrackData(ti.track);
                tracks.Add(track);
            }

            return tracks;
        }

        /// <summary>
        /// Create track object base on trackData fetch from spotify API
        /// </summary>
        /// <param name="trackEntity">track data from playlist call</param>
        /// <returns>Track model object</returns>
        /// <exception cref="SpotifyException"></exception>
        public async Task<Models.Track> GetTrackData(Entities.Track trackEntity)
        {
            if (trackEntity.id == null)
            {
                Console.WriteLine($"GetTrackData id null of the track name: {trackEntity.name}");
                return new Models.Track
                {
                    Name = trackEntity.name,
                    Artists = string.Join(",", trackEntity.artists.Select(x => x.name))
                };
            }

            // Get track data
            TrackData trackData = await this.api.FetchTrack(trackEntity.id) ?? new TrackData();

            Models.Track track = new Models.Track
            {
                Id = trackData.id,
                Name = trackData.name,
                Artists = string.Join(",", trackData.artists.Select(x => x.name)),
                AlbumName = trackData.album.name,
                Explicit = trackData.@explicit,
                IsLocal = trackData.is_local,
                Duration = trackData.duration_ms,// TODO convertir en min/secondes
                // TODO add: album name,
                // TODO add: album artist name,
                // TODO add: album release date,
                // TODO add: disc number,
                // TODO add: track number,
                // TODO add: popularity, 
                // TODO add: added at,
                // TODO add: album image url,
                // TODO add: track url,
                // TODO add: artist url
            };
            return track;
        }
    }
}
