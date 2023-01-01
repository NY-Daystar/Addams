using Addams.Export;
using Addams.Exceptions;
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
            SpotifyConfig cfg = SetupConfig();

            Console.WriteLine("Fetching playlist data...");
            List<Models.Playlist>? playlists = await GetPlaylists(cfg);
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
            SpotifyConfig config = new();
            try
            {
                config = SpotifyConfig.Read();
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

        /// <summary>
        /// Get playlist data of user to save it after
        /// </summary>
        /// <param name="user">username to get playlist</param>
        /// <returns>List of playlists to save</returns>
        public static async Task<List<Models.Playlist>> GetPlaylists(SpotifyConfig config)
        {
            SpotifyService service = new(config);

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