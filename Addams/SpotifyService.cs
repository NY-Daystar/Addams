
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

namespace Addams.Service
{
    internal class SpotifyService
    {
        private static string AUTH_TOKEN = @"BQAXn7hE5_oeBGoXu0T4I5f6R8saha_dge_jPUZ0Jw9YIgEqEIhhcwNHGtIeH2nAAdBjmk17QbU_f6UbhnGGVUjnmdV75W_hFsPv_YW017ROA_p9T7A_cwk0G0lc_sR5jCSSx0pBs07wmkpn_auPwu-fnKo0oABwiBeIzS8QbU74Prwjqd7wlMAKG-qVN0Y80jgFIAO1ZDQ";

        /// <summary>
        /// TODO to comment
        /// </summary>
        private SpotifyApi api;


        /// TODO to comment
        public SpotifyService(string user)
        {
            this.api = new SpotifyApi(user, AUTH_TOKEN);
        }

        /// TODO to comment
        public SpotifyService(string user, string authToken)
        {
            this.api = new SpotifyApi(user, authToken);
        }


        /// <summary>
        /// TODO to comment
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
                Console.WriteLine($"Playlist name : {p.name} - id : {p.id}");
                var t = await this.GetPlaylistTracks(p);
                // TODO ajouter les tracks dans le models et la variable playlist
            }



            // TODO transformer en model de playlist
            return playlists;
        }

        // TODO to comment
        public async Task<List<Models.Track>> GetPlaylistTracks(Entities.Playlist playlist)
        {

            List<Models.Track> tracks = new List<Models.Track>();

            if (playlist.id == null)
            {
                throw new SpotifyException($"getPlaylistTracks id null of the playlist name : {playlist.name}");
            }

            // Get playlist data
            PlaylistTracks? playlistTracks = await this.api.FetchPlaylistTracks(playlist.id);

            if (playlistTracks == null)
            {
                Console.WriteLine($"No tracks found for the playlist {playlist.name} - id: {playlist.id}");
                return new List<Models.Track>();
            }

            // TODO TODO TODO TODO TODO TODO
            //// Get tracks data for each track
            //foreach (Entities.TrackItem ti in playlistTracks.tracks.items)
            //    {
            //        Console.WriteLine($"Playlist name : {ti.track.name} - id : {ti.track.id}");
            //        //this.getTrackData(p); TODO call to get track data
            //    }



            // TODO transformer en model de playlist
            return tracks;
        }
    }
}
