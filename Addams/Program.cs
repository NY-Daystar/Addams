using Addams.Exceptions;
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
            // TODO refacto la partie service qui a une config et une api on setup le service qui a une config vierge
            // TODO on charge la config dans le service
            // DU couup inverser les lignes en dessous et adapter le code
            Console.WriteLine("Setup config..."); // TODO put log
            SpotifyConfig cfg = SetupConfig();

            Console.WriteLine("Setup service..."); // TODO put log
            SpotifyService service = new SpotifyService(cfg);
            /// TODO

            // TODO Gestion OAUTH2 authorization_code
            //Console.WriteLine("Get OAuth2 token...");
            //string newToken = await service.RefreshToken();
            //service.Update(newToken);

            // Ask if you want all playlist or just a few
            bool allPlaylist = AskAllPlaylistWanted();

            Console.WriteLine("Fetching playlist data..."); // TODO put log
            List<Models.Playlist>? playlists = await GetPlaylists(service, allPlaylist); ;
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

        // TODO recomment
        /// <summary>
        /// Get playlist data of user to save it after
        /// </summary>
        /// <param name="user">username to get playlist</param>
        /// <returns>List of playlists to save</returns>
        public static async Task<List<Models.Playlist>> GetPlaylists(SpotifyService service, bool allPlaylist)
        {
            // Get playlist
            List<Models.Playlist>? playlists = await service.GetPlaylists(allPlaylist);

            return playlists ?? new List<Models.Playlist>();
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

        /// <summary>
        /// Ask the user if he want to export all playlist
        /// Yes : means true, No means false
        /// </summary>
        /// <returns>bool of the pick</returns>
        public static bool AskAllPlaylistWanted()
        {
            bool answered = false;
            do
            {
                Console.Write("Do you want to export all playlist\n    [1]:Yes\t[2]:No : ");

                char key = Console.ReadKey().KeyChar;

                if (key == '1')
                {
                    return true;
                }
                else if (key == '2')
                {
                    return false;
                }
                else
                {
                    Console.WriteLine($"\nYou type '{key}'. Please choose '1' or '2'"); // TODO language
                }
            } while (!answered);

            Console.WriteLine();
            return false;
        }

    }
}