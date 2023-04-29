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
    internal static class Addams
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static string LOGFILE => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "Addams",
                "logs",
                "addams.log"
                );

        public static void Main(string[] args)
        {
            Run(args).GetAwaiter().GetResult();
        }

        public static async Task Run(string[] args)
        {
            // Get arguments from exe file
            AddamsOptions options = AddamsOptions.DefineOptions(args);

            LogLevel level = options.Debug ? LogLevel.Debug : LogLevel.Info;
            SetupLogger(LOGFILE, level);
            Logger.Info("Launching Addams Application");

            Logger.Debug("Setup service with config and api...");
            SpotifyService service = new();

            // TODO feature OAUTH2 authorization_code
            //Console.WriteLine("Get OAuth2 token...");
            //string newToken = await service.RefreshToken();
            //service.Update(newToken);

            // Ask if you want all playlist or just a few
            bool allPlaylist = AskAllPlaylistWanted();
            Console.WriteLine();

            Logger.Info("Fetching playlist data...");
            IEnumerable<Models.Playlist>? playlists = await service.GetPlaylists(allPlaylist); // Get playlist data of user to save it after


            if (playlists == null)
            {
                Logger.Error("None playlist found");
                return;
            }
            Logger.Info("Playlist fetched...");

            Logger.Info("Saving playlists...");
            SpotifyExport.SavePlaylists(playlists);
            Logger.Info("All playlists are saved...");
        }

        /// <summary>
        /// Setup the logger with its path and it's minimum level
        /// </summary>
        /// <param name="filePath">path of the file</param>
        /// <param name="level">Minimum level to define</param>
        private static void SetupLogger(string filePath, LogLevel level)
        {
            LoggingConfiguration config = new();
            Layout layout = "level:${uppercase:${level}} - date:${date} - caller: ${callsite-filename}:${callsite-linenumber} - ${message} ${exception:format=tostring}";

            // Targets where to log to: File and Console
            FileTarget logfile = new("logfile")
            {
                FileName = filePath,
                ArchiveEvery = FileArchivePeriod.Minute,
                ArchiveNumbering = ArchiveNumberingMode.Rolling,
                MaxArchiveFiles = 5,
                Layout = layout

            };

            ConsoleTarget logconsole = new("logconsole")
            {
                Layout = layout
            };

            // Rules for mapping loggers to targets            
            config.AddRule(level, LogLevel.Fatal, logconsole);
            config.AddRule(LogLevel.Trace, LogLevel.Fatal, logfile);

            // Apply config           
            LogManager.Configuration = config;
        }

        /// <summary>
        /// Ask the user if he want to export all playlist
        /// Yes : means true, No means false
        /// </summary>
        /// <returns>bool of the pick</returns>
        public static bool AskAllPlaylistWanted()
        {
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
                    Console.WriteLine($"\nYou type '{key}'. Please choose '1' or '2'"); // TODO feature language
                }
            } while (true);
        }
    }
}