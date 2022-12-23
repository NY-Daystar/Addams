
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Addams.Entities;
using Addams.Exceptions;

namespace Addams.Api
{
    /// <summary>
    /// Spotify API requests
    /// </summary>
    public class SpotifyApi
    {
        /// <summary>
        /// Url endpoint for Spotify Api
        /// </summary>
        private static string API = @"https://api.spotify.com/v1";

        /// <summary>
        /// User to do the request on playlist
        /// </summary>
        private string user = String.Empty;

        /// <summary>
        /// OAuth2 token to get info on spotify API for a specific user
        /// </summary>
        private string authToken = String.Empty;

        /// <summary>
        /// Setup Spotify API
        /// </summary>
        /// <param name="user">User to get infos</param>
        /// <param name="authToken">Authentication token to get access to the data</param>
        public SpotifyApi(string user, string authToken)
        {
            this.user = user;
            this.authToken = authToken;
        }

        /// <summary>
        /// Setup http client with authorization token (oauth2) for spotify api
        /// </summary>
        /// <returns>HttpClient with token authentication value</returns>
        /// 
        // TODO en faire un singleton l'appeler qu'uune seuule fois lors de la creation de l'objet SpotifyApi
        private HttpClient GetAuthClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.authToken);
            return client;
        }

        /// <summary>
        /// Fetch data about playlist's user
        /// </summary>
        /// <returns>Playlist data return by spotify api /playlists</returns>
        /// <exception cref="SpotifyUnauthorizedException"></exception>
        /// <exception cref="SpotifyException"></exception>
        public async Task<Playlists> FetchUserPlaylists()
        {
            const int limit = 50;
            const int offset = 0;

            string url = $@"{API}/users/{this.user}/playlists?limit={limit}&offset={offset}";
            Console.WriteLine($"CALL API: {url}");
            HttpClient client = this.GetAuthClient();

            HttpResponseMessage response = await client.GetAsync(url);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new SpotifyUnauthorizedException($"Can't get FetchUserPlaylists\nThe token {this.authToken}\nis invalid for user: {this.user}\n" +
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
            Console.WriteLine($"CALL API: {url}");
            HttpClient client = this.GetAuthClient();

            HttpResponseMessage response = await client.GetAsync(url);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new SpotifyUnauthorizedException($"Can't get FetchPlaylistTracks\nThe token {this.authToken}\nis invalid for user: {this.user}\n" +
                    "You need to create a new one or refresh it");
            }
            if (!response.IsSuccessStatusCode)
            {
                throw new SpotifyException($"Can't get FetchPlaylistTracks\nStatusCode {response.StatusCode} : {response.Content}");
            }
            string content = await response.Content.ReadAsStringAsync();
            PlaylistTracks tracks = JsonConvert.DeserializeObject<PlaylistTracks>(content) ?? new PlaylistTracks();
            return tracks;
        }

        /// <summary>
        /// Fetch track data from its id
        /// </summary>
        /// <param name="id">id of trrack</param>
        /// <returns>Track data</returns>
        /// <exception cref="SpotifyUnauthorizedException"></exception>
        /// <exception cref="SpotifyException"></exception>
        public async Task<TrackData> FetchTrack(string id)
        {
            string url = $@"{API}/tracks/{id}";
            Console.WriteLine($"CALL API: {url}");
            HttpClient client = this.GetAuthClient();

            HttpResponseMessage response = await client.GetAsync(url);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new SpotifyUnauthorizedException($"Can't get FetchTrack\nThe token {this.authToken}\nis invalid for user: {this.user}\n" +
                    "You need to create a new one or refresh it");
            }
            if (!response.IsSuccessStatusCode)
            {
                throw new SpotifyException($"Can't get FetchTrack\nStatusCode {response.StatusCode} : {response.Content}");
            }
            string content = await response.Content.ReadAsStringAsync();
            TrackData trackData= JsonConvert.DeserializeObject<TrackData>(content) ?? new TrackData();
            return trackData;
        }
    }
}
