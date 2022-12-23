
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
    // TODO to comment
    public class SpotifyApi
    {
        // TODO to comment
        private static string API = @"https://api.spotify.com/v1";

        // TODO to comment
        private string user = String.Empty;

        // TODO to comment
        private string authToken = String.Empty;

        public SpotifyApi(string user, string authToken)
        {
            this.user = user;
            this.authToken = authToken;
        }

        private HttpClient GetAuthClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.authToken);
            return client;
        }

        /// <summary>
        /// TODO mettre la doc
        /// </summary>
        /// <returns></returns>
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
        /// TODO mettre la doc
        /// </summary>
        /// <returns></returns>
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
        /// TODO a mettre dans un tests unitaires
        /// </summary>
        /// <returns></returns>
        public async Task<string?> GetPlaylistsItems()
        {
            string url = @"https://api.spotify.com/v1/users/gravityx3/playlists?offset=30&limit=1";
            Console.WriteLine($"CALL API: {url}");
            HttpClient client = GetAuthClient();
            using (var request = new HttpRequestMessage(HttpMethod.Get, url))
            {
                request.Headers.Add("Accept", "application/json");
                string response = await client.GetStringAsync(url);
                return response;
            }
        }
    }
}
