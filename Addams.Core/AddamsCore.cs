using Addams.Core.Entities;
using Addams.Core.Logs;
using Addams.Core.Models;
using Addams.Core.Spotify;
using Addams.Core.Utils;
using NLog;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;
using System.Diagnostics;

namespace Addams.Core;

/// <summary>
/// Entrypoint of the program
/// </summary>
public static class AddamsCore
{
    private static string LOGFILE => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "Addams",
            "logs",
            "addams.log"
            );

    private static readonly string PLAYLIST_FOLDER = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        Application,
        "playlists"
        );

    public const string Application = "Addams";

    public const string Version = "1.1.0";

    /// <summary>
    /// Launch core application and return file of logs
    /// </summary>
    public static void Launch()
    {
        SetupLogger();
        LoggerManager.Log($"{Language.GetString("Application")}: {Application}", Level.Debug);
        LoggerManager.Log($"{Language.GetString("Version")} : {Version}", Level.Debug);
        AddamsConfig.TryGet();
    }

    public async static Task<SpotifyAuthenticationStatus> CheckTokenAsync(CancellationTokenSource cts)
    {
        return await new SpotifyService().IsTokenValidAsync(cts);
    }

    public static bool CanGenerateToken()
    {
        var config = AddamsConfig.Get();
        return
            !string.IsNullOrEmpty(config.UserName) &&
            !string.IsNullOrEmpty(config.ClientID) &&
            !string.IsNullOrEmpty(config.ClientSecret);
    }

    public async static Task<IEnumerable<PlaylistEntity>> GetPlaylistsAsync()
    {
        SpotifyService service = new();
        return await service.GetPlaylistsNameAsync();
    }

    public static async Task ExportAsync(List<string> playlistsSelected)
    {
        LoggerManager.Log(Language.GetString("String5"), Level.Debug);
        SpotifyService service = new();

        LoggerManager.Log(Language.GetString("String6"), Level.Debug);

        LoggerManager.Log(Language.GetString("String7"), Level.Warning);
        var config = AddamsConfig.Get();
        if (config.Token?.Refresh == null || config.Token?.Refresh == "")
            LoggerManager.Log(Language.GetString("String59"));
        else
            LoggerManager.Log(Language.GetString("String60"));

        LoggerManager.Log(Language.GetString("String61"), Level.Ok);

        LoggerManager.Log(Language.GetString("String8"));
        IEnumerable<Playlist>? playlists = await service.GetPlaylistsAsync(playlistsSelected);

        if (playlists == null)
        {
            LoggerManager.Log(Language.GetString("String9"), Level.Error);
            return;
        }

        // Check if tracks is on playlist
        foreach (var playlist in playlists.Where(p => !p.Tracks.Any()))
        {
            LoggerManager.Log(string.Format(Language.GetString("String31"), playlist.Name, playlist.Id), Level.Warning);
        }

        playlists = [.. playlists.Where(p => p.Tracks.Any())];

        LoggerManager.Log(Language.GetString("String10"));

        LoggerManager.Log(Language.GetString("String11"));
        SpotifyExport.SavePlaylists(PLAYLIST_FOLDER, playlists);
        LoggerManager.Log(Language.GetString("String12"));

        GoToPlaylists();
    }

    public static void GoToPlaylists()
    {
        Process.Start("explorer.exe", PLAYLIST_FOLDER);
    }

    public static IEnumerable<string> ShowPlaylists()
    {
        return Directory.EnumerateFiles(PLAYLIST_FOLDER, "*.csv");
    }

    public static AddamsConfig GetConfiguration()
    {
        return AddamsConfig.Get();
    }

    public static void OpenConfiguration()
    {
        try
        {
            var psi = new ProcessStartInfo(AddamsConfig.ConfigFilepath)
            {
                UseShellExecute = true
            };
            Process.Start(psi);
        }
        catch (Exception ex)
        {
            LoggerManager.Log($"{ex.GetType().FullName} - {ex.Message} - {ex.StackTrace}", Level.Debug);
        }
    }

    public static async Task GenerateTokenAsync(CancellationTokenSource cts)
    {
        var config = AddamsConfig.Get();
        config.Token = await new SpotifyService().AuthorizeAsync(cts);
        config.Save();
    }

    /// <summary>
    /// Setup the logger with its path and it's minimum level
    /// </summary>
    private static void SetupLogger()
    {
        LoggingConfiguration config = new();
        Layout layout = "level:${uppercase:${level}} - date:${date} - caller: ${callsite-filename}:${callsite-linenumber} - ${message} ${exception:format=tostring}";

        FileTarget logfile = new("logfile")
        {
            FileName = LOGFILE,
            ArchiveEvery = FileArchivePeriod.Hour,
            MaxArchiveFiles = 5,
            Layout = layout
        };

        config.AddRule(LogLevel.Trace, LogLevel.Fatal, logfile);

        LogManager.Configuration = config;
    }
}