using Addams.Entities;
using Addams.Utils;
using NLog;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Resources;
using System.Threading;
using System.Threading.Tasks;

namespace Addams;

/// <summary>
/// Entrypoint of the program
/// </summary>
internal static class Addams
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    private static string LOGFILE => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "Addams",
            "logs",
            "addams.log"
            );

    private static readonly string PLAYLIST_FOLDER = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        _application,
        "playlists"
        );

    private const string _application = "Addams";

    private const string _version = "1.0.2";

    public static void Main(string[] args)
    {
        RunAsync(args).GetAwaiter().GetResult();
        Process.Start("explorer.exe", PLAYLIST_FOLDER);
        Thread.Sleep(60000);
    }

    public static async Task RunAsync(string[] args)
    {
        AddamsOptions options = AddamsOptions.DefineOptions(args);

        // TODO faire un affichage de la config si demander

        // TODO 1 - Recuperer les musiques ne venant pas de spotify mais des fichiers locaux

        Core.WriteLine("Welcome to ", ConsoleColor.Yellow, _application, ConsoleColor.White,
            " - Version : ", ConsoleColor.Yellow, _version, ConsoleColor.White);
        Console.WriteLine("---------------------");

        LogLevel level = options.Debug ? LogLevel.Debug : LogLevel.Info; // add in exe argument --debug
        SetupLogger(LOGFILE, level);

        Logger.Info(Language.GetString("String4"));

        Logger.Debug(Language.GetString("String5"));
        SpotifyService service = new();

        Logger.Debug(Language.GetString("String6"));
        if (!await service.IsTokenValidAsync())
        {
            Logger.Warn(Language.GetString("String7"));
            await service.RefreshTokenAsync();
        }

        // Ask if you want all playlist or just a few
        bool allPlaylist = AddamsUser.AskAllPlaylistWanted();

        IEnumerable<Playlist> playlistsSelected = await service.GetPlaylistsNameAsync();

        // If we don't want all playlist
        if (!allPlaylist)
            playlistsSelected = AddamsUser.SelectPlaylist(playlistsSelected.ToList());

        Logger.Info(Language.GetString("String8"));
        IEnumerable<Models.Playlist>? playlists = await service.GetPlaylistsAsync(playlistsSelected);

        if (playlists == null)
        {
            Logger.Error(Language.GetString("String9"));
            return;
        }
        Logger.Info(Language.GetString("String10"));

        Logger.Info(Language.GetString("String11"));
        SpotifyExport.SavePlaylists(PLAYLIST_FOLDER, playlists);
        Logger.Info(Language.GetString("String12"));
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
         
        config.AddRule(level, LogLevel.Fatal, logconsole);
        config.AddRule(LogLevel.Trace, LogLevel.Fatal, logfile);
       
        LogManager.Configuration = config;
    }
}