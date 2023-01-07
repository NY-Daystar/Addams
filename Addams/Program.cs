using Addams.Exceptions;
using Addams.Export;
using Addams.Service;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Addams
{
    internal class Program
    {
        public static void Main()
        {
            Run().GetAwaiter().GetResult();
        }

        public static async Task Run()
        {
            Console.WriteLine("Setup config..."); // TODO put log
            SpotifyConfig cfg = SetupConfig();

            Console.WriteLine("Setup service..."); // TODO put log
            SpotifyService service = new(cfg);
            
            Console.WriteLine("Get OAuth2 token...");
            string newToken = await service.RefreshToken();
            service.Update(newToken);

            Console.WriteLine("Fetching playlist data..."); // TODO put log
            List<Models.Playlist>? playlists = await GetPlaylists(service);
            if (playlists == null)
            {
                // TODO put log
                return;
            }
            Console.WriteLine("Playlist fetched...");

            Console.WriteLine("Saving playlist...");
            SpotifyExport.SavePlaylists(playlists);
            Console.WriteLine("Playlist saved...");
        }

        /// <summary>
        /// Retrieve config if already exists if not we create it
        /// </summary>
        /// <returns></returns>
        public static SpotifyConfig SetupConfig()
        {
            SpotifyConfig defaultConfig = new();
            SpotifyConfig config = new();

            try
            {
                config = SpotifyConfig.Read();
                config.Token = defaultConfig.Token; // TODO get default token for now 
                SpotifyService service = new(config);
                Console.WriteLine($"Config already exists:\n{config}");
            }
            catch (SpotifyConfigException)
            {
                config.Setup();
                Console.WriteLine($"This config will be saved:\n{config}");
                config.Save();
            }
            return config;
        }

        // TODO recomment
        /// <summary>
        /// Get playlist data of user to save it after
        /// </summary>
        /// <param name="user">username to get playlist</param>
        /// <returns>List of playlists to save</returns>
        public static async Task<List<Models.Playlist>> GetPlaylists(SpotifyService service)
        {
            // Get playlist
            List<Models.Playlist>? playlists = await service.GetPlaylists();

            if (playlists == null)
            {
                return new List<Models.Playlist>();
            }
            return playlists;
        }

    }
}