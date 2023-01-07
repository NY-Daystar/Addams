using Addams.Entities;
using Addams.Exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Addams.Api
{
    /// <summary>
    /// Spotify API requests
    /// </summary>
    public class SpotifyApi
    {
        /// Number of playlists fetchable in spotify API
        /// </summary>
        private static readonly int PLAYLIST_LIMIT = 50;

        /// Number of tracks liked fetchable in spotify API
        /// </summary>
        private static readonly int TRACK_LIKED_LIMIT = 50;

        /// <summary>
        /// Number of tracks fetched in spotify API
        /// </summary>
        private static readonly int TRACK_LIMIT = 100;

        /// <summary>
        /// Url endpoint for Spotify Api
        /// </summary>
        private static readonly string API = @"https://api.spotify.com/v1";

        /// <summary>
        /// User to do the request on playlist
        /// </summary>
        private string User;

        /// <summary>
        /// Id of spotify app
        /// </summary>
        private string ClientID;

        /// <summary>
        /// Secret of spotify app
        /// </summary>
        private string ClientSecret;

        /// <summary>
        /// OAuth2 token to get info on spotify API for a specific user
        /// </summary>
        public string AuthToken { private get; set; }

        /// <summary>
        /// Authenticated client with OAuth Token
        /// </summary>
        private HttpClient Client;

        /// <summary>
        /// Setup Spotify API
        /// </summary>
        /// <param name="user">User to get infos</param>
        /// <param name="authToken">Authentication token to get access to the data</param>
        public SpotifyApi(SpotifyConfig cfg)
        {
            this.User = cfg.User;
            this.ClientID = cfg.ClientID;
            this.ClientSecret = cfg.ClientSecret;
            this.AuthToken = cfg.Token;
            this.Client = this.GetAuthClient();
        }

        /// <summary>
        /// Setup http client with authorization token (oauth2) for spotify api
        /// </summary>
        /// <returns>HttpClient with token authentication value</returns>
        /// 
        private HttpClient GetAuthClient()
        {
            HttpClient client = new();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.AuthToken);
            return client;
        }

        // TODO to comment
        public void RefreshClient(string token)
        {
            this.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        //TODO to comment
        public async Task<Token> Authorize()
        {
            FormUrlEncodedContent requestData = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("client_id", this.ClientID),
                new KeyValuePair<string, string>("client_secret", this.ClientSecret),
                //new KeyValuePair<string, string>("scope","playlist-read-private user-library-read ugc-image-upload user-read-playback-state user-modify-playback-state user-read-currently-playing app-remote-control playlist-read-private playlist-read-collaborative playlist-modify-private playlist-modify-public user-follow-modify user-follow-read user-read-playback-position user-top-read user-read-recently-played user-library-modify user-library-read user-read-email user-read-private"),
                new KeyValuePair<string, string>("scope","playlist-read-private user-library-read"),
                new KeyValuePair<string, string>("grant_type", "authorization_code")
            });

            string url = "https://accounts.spotify.com/api/token";
            var response = await new HttpClient().PostAsync(url, requestData);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                // TODO changer le message
                throw new SpotifyUnauthorizedException($"Can't get Authorize\nThe token {this.AuthToken}\nis invalid for user: {this.User}\n" +
                    "You need to create a new one or refresh it");
            }
            if (!response.IsSuccessStatusCode)
            {
                // TODO changer le message
                throw new SpotifyException($"Can't get Authorize\nStatusCode {response.StatusCode} : {response.Content}");
            }
            string content = await response.Content.ReadAsStringAsync();
            Token token = JsonConvert.DeserializeObject<Token>(content) ?? new Token();
            return token;
        }


        /// <summary>
        /// Fetch Liked songs of user
        /// </summary>
        /// <returns>Playlist of liked soong of a user return by spotify api /tracks</returns>
        /// <exception cref="SpotifyUnauthorizedException"></exception>
        /// <exception cref="SpotifyException"></exception>
        public async Task<LikePlaylist> FetchUserLikeTracks()
        {
            // TODO Faire une boucle qui recupere tout en se basant sur la vaaleurr total et en faisant varier ll'offset
            string url = $@"{API}/users/{this.User}/tracks?limit={TRACK_LIKED_LIMIT}&offset=50";
            //Console.WriteLine($"FetchUserLikeTracks call API: {url}"); // TODO put log

            HttpResponseMessage response = await Client.GetAsync(url);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new SpotifyUnauthorizedException($"Can't get FetchUserLikeTracks\nThe token {this.AuthToken}\nis invalid for user: {this.User}\n" +
                    "You need to create a new one or refresh it");
            }
            if (!response.IsSuccessStatusCode)
            {
                throw new SpotifyException($"Can't get FetchUserLikeTracks\nStatusCode {response.StatusCode} : {response.Content}");
            }
            string content = await response.Content.ReadAsStringAsync();
            LikePlaylist playlist = JsonConvert.DeserializeObject<LikePlaylist>(content) ?? new LikePlaylist();
            return playlist;
        }




        /// <summary>
        /// Fetch data about playlist's user
        /// </summary>
        /// <returns>Playlist data return by spotify api /playlists</returns>
        /// <exception cref="SpotifyUnauthorizedException"></exception>
        /// <exception cref="SpotifyException"></exception>
        public async Task<Playlists> FetchUserPlaylists()
        {
            string url = $@"{API}/users/{this.User}/playlists?limit={PLAYLIST_LIMIT}&offset=0";
            //Console.WriteLine($"FetchUserPlaylists call API: {url}"); // TODO put log

            HttpResponseMessage response = await Client.GetAsync(url);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new SpotifyUnauthorizedException($"Can't get FetchUserPlaylists\nThe token {this.AuthToken}\nis invalid for user: {this.User}\n" +
                    "You need to create a new one or refresh it");
            }
            if (!response.IsSuccessStatusCode)
            {
                throw new SpotifyException($"Can't get FetchUserPlaylists\nStatusCode {response.StatusCode} : {response.Content}");
            }
            string content = await response.Content.ReadAsStringAsync();
            Playlists playlists = JsonConvert.DeserializeObject<Playlists>(content) ?? new Playlists();
            return playlists;
        }

        /// <summary>
        /// Fetch tracks on specific playlist from its id
        /// </summary>
        /// <param name="id">id of playlist</param>
        /// <returns>Tracks' playlist</returns>
        /// <exception cref="SpotifyUnauthorizedException"></exception>
        /// <exception cref="SpotifyException"></exception>
        public async Task<PlaylistTracks> FetchPlaylistTracks(string id)
        {
            string url = $@"{API}/playlists/{id}";
            //Console.WriteLine($"FetchPlaylistTracks call API: {url}"); // TODO put log

            HttpResponseMessage response = await Client.GetAsync(url);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new SpotifyUnauthorizedException($"Can't get FetchPlaylistTracks\nThe token {this.AuthToken}\nis invalid for user: {this.User}\n" +
                    "You need to create a new one or refresh it");
            }
            if (!response.IsSuccessStatusCode)
            {
                throw new SpotifyException($"Can't get FetchPlaylistTracks\nStatusCode {response.StatusCode} : {response.Content}");
            }
            string content = await response.Content.ReadAsStringAsync();
            PlaylistTracks playlistTracks = JsonConvert.DeserializeObject<PlaylistTracks>(content) ?? new PlaylistTracks();

            // If can't catch every tracks for a playlist in one api call
            if (playlistTracks.tracks.total >= TRACK_LIMIT)
            {
                Console.WriteLine($"Only fetch {playlistTracks.tracks.items.Count} tracks for a total to {playlistTracks.tracks.total}");
                TrackList trackList = playlistTracks.tracks;

                // Get all tracks of specific playlist
                do
                {
                    if (trackList.next == null)
                    {
                        Console.WriteLine($"Tracklist next url is null, can't get the rest of playlist");
                        break;
                    }

                    trackList = await this.FetchOverflowTracks(trackList.next);
                    playlistTracks.tracks.items.AddRange(trackList.items);
                } while (trackList.next != null);
                playlistTracks.tracks.next = null;
            }
            return playlistTracks;
        }

        /// <summary>
        /// Fetch overflow tracks from specific API url
        /// </summary>
        /// <param name="url">url to request to fetch tracks</param>
        /// <returns>Tracklist overflowed</returns>
        /// <exception cref="SpotifyUnauthorizedException"></exception>
        /// <exception cref="SpotifyException"></exception>
        public async Task<TrackList> FetchOverflowTracks(string url)
        {
            //Console.WriteLine($"FetchOverflowTracks call API: {url}"); // TODO put log

            HttpResponseMessage response = await Client.GetAsync(url);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new SpotifyUnauthorizedException($"Can't get FetchPlaylistTracks\nThe token {this.AuthToken}\nis invalid for user: {this.User}\n" +
                    "You need to create a new one or refresh it");
            }
            if (!response.IsSuccessStatusCode)
            {
                throw new SpotifyException($"Can't get FetchPlaylistTracks\nStatusCode {response.StatusCode} : {response.Content}");
            }
            string content = await response.Content.ReadAsStringAsync();
            TrackList trackList = JsonConvert.DeserializeObject<TrackList>(content) ?? new TrackList();

            return trackList;
        }

        /// <summary>
        /// Fetch track data from its id
        /// </summary>
        /// <param name="id">id of trrack</param>
        /// <returns>Track data</returns>
        /// <exception cref="SpotifyUnauthorizedException"></exception>
        /// <exception cref="SpotifyException"></exception>
        public async Task<Track> FetchTrack(string id)
        {
            string url = $@"{API}/tracks/{id}";
            //Console.WriteLine($"FetchTrack call API: {url}"); // TODO put log

            HttpResponseMessage response = await Client.GetAsync(url);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new SpotifyUnauthorizedException($"Can't get FetchTrack\nThe token {this.AuthToken}\nis invalid for user: {this.User}\n" +
                    "You need to create a new one or refresh it");
            }
            if (!response.IsSuccessStatusCode)
            {
                throw new SpotifyException($"Can't get FetchTrack\nStatusCode {response.StatusCode} : {response.Content}");
            }
            string content = await response.Content.ReadAsStringAsync();
            Track track = JsonConvert.DeserializeObject<Track>(content) ?? new Track();
            return track;
        }
    }
}
