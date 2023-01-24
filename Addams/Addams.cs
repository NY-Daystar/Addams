using Addams.Exceptions;
using NLog;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Addams
{
    internal class Addams
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public static void Main()
        {
            Run().GetAwaiter().GetResult();
        }

        public static async Task Run()
        {
            string logFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Addams", "logs", "addams.log"); ;
            LogLevel level = LogLevel.Debug;
            SetupLogger(logFile, level);

            // TODO refacto la partie service qui a une config et une api on setup le service qui a une config vierge
            // TODO on charge la config dans le service
            // DU couup inverser les lignes en dessous et adapter le code
            Logger.Debug("Setup config...");
            SpotifyConfig cfg = SetupConfig();

            Logger.Debug("Setup service...");
            SpotifyService service = new SpotifyService(cfg);
            /// TODO

            // TODO Gestion OAUTH2 authorization_code
            //Console.WriteLine("Get OAuth2 token...");
            //string newToken = await service.RefreshToken();
            //service.Update(newToken);

            // Ask if you want all playlist or just a few
            bool allPlaylist = AskAllPlaylistWanted();

            Logger.Info("Fetching playlist data...");
            List<Models.Playlist>? playlists = await GetPlaylists(service, allPlaylist); ;
            if (playlists == null)
            {
                Logger.Error("None playlist found");
                return;
            }
            Logger.Info("Playlist fetched...");

            Logger.Info("Saving playlist...");
            SpotifyExport.SavePlaylists(playlists);
            Logger.Info("Playlist saved...");
        }

        /// <summary>
        /// Setup the logger with its path and it's minimum level
        /// </summary>
        /// <param name="filePath">path of the file</param>
        /// <param name="level">Minimum level to define</param>
        private static void SetupLogger(string filePath, LogLevel level)
        {
            LoggingConfiguration config = new LoggingConfiguration();
            Layout layout = "level:${uppercase:${level}} - date:${date} - caller: ${callsite-filename}:${callsite-linenumber} - ${message} ${exception:format=tostring}";

            // Targets where to log to: File and Console
            FileTarget logfile = new FileTarget("logfile")
            {
                FileName = filePath,
                ArchiveEvery = FileArchivePeriod.Minute,
                ArchiveNumbering = ArchiveNumberingMode.Rolling,
                MaxArchiveFiles = 5,
                Layout = layout

            };

            ConsoleTarget logconsole = new ConsoleTarget("logconsole")
            {
                Layout = layout
            };

            // Rules for mapping loggers to targets            
            config.AddRule(level, LogLevel.Fatal, logconsole);
            config.AddRule(LogLevel.Trace, LogLevel.Fatal, logfile);

            // Apply config           
            LogManager.Configuration = config;
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
                Logger.Debug($"Config already exists:\n{config}");
            }
            catch (SpotifyConfigException)
            {
                config.Setup();
                Logger.Warn($"This config will be saved:\n{config}");
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