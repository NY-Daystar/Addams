﻿
using Addams.Api;
using Addams.Entities;
using Addams.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Addams.Service
{
    internal class SpotifyService
    {

        /// <summary>
        /// Spotify Api requests
        /// </summary>
        private SpotifyConfig config;

        /// <summary>
        /// Spotify Api requests
        /// </summary>
        private SpotifyApi api;

        /// <summary>
        /// Default Spotify service to get playlist and track with config setup
        /// </summary>
        /// <param name="user">Spotify user name</param>
        /// <param name="authToken">OAuth2 token authentication generated</param>
        public SpotifyService(SpotifyConfig cfg)
        {
            this.config = cfg;
            this.api = new SpotifyApi(cfg);
        }

        /// <summary>
        /// Setup authorization to Spotify Application to get OAUTH2 Token
        /// </summary>
        /// <returns>Token string</returns>
        public async Task<string> RefreshToken()
        {
            Token OAuth2 = await this.api.Authorize();

            if (OAuth2.access_token == null) throw new Exception();// TODO changer exception en AuthorizeException
            return OAuth2.access_token;
        }

        // TODO to comment

        public void Update(string accessToken)
        {
            // TODO fusionner en setToken()
            this.config.Token = accessToken;
            this.config.Save();

            this.api.RefreshClient(accessToken);
        }

        /// <summary>
        /// Get all playlist created or saved by a user
        /// </summary>
        /// <returns></returns>
        public async Task<List<Models.Playlist>> GetPlaylists()
        {

            List<Models.Playlist> playlists = new();

            // Get playlist data
            Playlists playlistsData = await this.api.FetchUserPlaylists();

            //Liked song playlist
            Models.Playlist likedPlaylist = await this.GetLikedTracks();
            playlists.Add(likedPlaylist);

            if (playlistsData == null || playlistsData.items == null)
            {
                Console.WriteLine("No playlists found");
                return new List<Models.Playlist>();
            }

            // Get tracks for each playlist
            foreach (Playlist p in playlistsData.items)
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
            List<Models.Track> tracks = new();
            foreach (Entities.TrackItem ti in playlistTracks.tracks.items)
            {
                if (ti.track == null)
                    continue;
                Models.Track track = this.GetTrack(ti);

                tracks.Add(track);
            }

            return tracks;
        }

        /// <summary>
        /// Create track object base on trackData fetch from spotify API
        /// </summary>
        /// <param name="trackEntity">track data from playlist call</param>
        /// <returns>Track model object</returns>
        public Models.Track GetTrack(Entities.TrackItem trackEntity)
        {
            Entities.Track track = trackEntity.track;
            if (track.id == null)
            {
                Console.WriteLine($"GetTrackData id null of the track name: {track.name}");
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



        //TODO TO  COMMENT
        public async Task<Models.Playlist> GetLikedTracks()
        {
            //TODO convvertir enn Model playlist
            LikePlaylist playlist = await this.api.FetchUserLikeTracks();

            return new Models.Playlist();

        }
    }
}
