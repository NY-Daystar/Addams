using Addams.Entities;
using Addams.Utils;
using Microsoft.VisualBasic;
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

    private const string _version = "1.0.3";

    public static void Main(string[] args)
    {
        RunAsync(args).GetAwaiter().GetResult();
        Thread.Sleep(60000);
    }

    public static async Task RunAsync(string[] args)
    {
        AddamsOptions options = AddamsOptions.DefineOptions(args);

        Core.WriteLine("Welcome to ", ConsoleColor.Yellow, _application, ConsoleColor.White,
            " - Version : ", ConsoleColor.Yellow, _version, ConsoleColor.White);
        Console.WriteLine("---------------------");

        LogLevel level = options.Debug ? LogLevel.Debug : LogLevel.Info; // add in exe argument --debug
        SetupLogger(LOGFILE, level);

        Logger.Info(Language.GetString("String4"));

        while (true)
        {
            switch (AddamsUser.AskWhatToDo())
            {
                case "1":
                    await ExportAsync();
                    break;

                case "2":
                    ShowConfiguration();
                    break;

                case "3":
                    ModifyConfiguration();
                    break;

                case "4":
                    ShowLogs();
                    break;
                default:
                    return;
            }
        }
    }

    private static async Task ExportAsync()
    {
        Logger.Debug(Language.GetString("String5"));
        SpotifyService service = new();

        Logger.Debug(Language.GetString("String6"));
        if (!await service.IsTokenValidAsync())
        {
            Logger.Warn(Language.GetString("String7"));
            await service.RefreshTokenAsync();
        }

        IEnumerable<Playlist> playlistsSelected = await service.GetPlaylistsNameAsync();

        if (!AddamsUser.AskAllPlaylistWanted())
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

        Process.Start("explorer.exe", PLAYLIST_FOLDER);
    }

    private static void ShowConfiguration()
    {
        var config = SpotifyConfig.Get();
        Console.WriteLine("---------------");
        Console.WriteLine(config);
        Console.WriteLine("---------------");
    }

    private static void ModifyConfiguration()
    {
        try
        {
            var psi = new ProcessStartInfo(SpotifyConfig.ConfigFilepath)
            {
                UseShellExecute = true
            };
            Process.Start(psi);
        }
        catch (Exception ex)
        {
            Logger.Debug(ex.Message);
        }
    }

    private static void ShowLogs()
    {
        var tempfile = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.txt");
        var directory = Directory.GetParent(LOGFILE);
        if (directory != null)
        {
            FileManager.ConcatFiles(directory.FullName, tempfile);
        }

        try
        {
            var psi = new ProcessStartInfo(tempfile)
            {
                UseShellExecute = true
            };
            Process.Start(psi);
        }
        catch (Exception ex)
        {
            Logger.Debug(ex.Message);
        }
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