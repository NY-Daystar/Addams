using Addams.Utils;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Addams;

/// <summary>
/// Class to handle csv export data file with spotify tracks data
/// </summary>
public static class SpotifyExport
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    private const string _application = "Addams";

    /// <summary>
    /// Save playlist data into csv file
    /// </summary>
    /// <param name="data">List of playlist to save</param>
    public static void SavePlaylists(IEnumerable<Models.Playlist> data)
    {
        // TODO faire un excel avec une playlist pas onglet
        const string appFolder = "playlists";
        
        string wDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), _application, appFolder);

        if (!Directory.Exists(wDir))
        {
            _ = Directory.CreateDirectory(wDir);
        }

        // Save each playlist
        foreach (Models.Playlist playlist in data)
        {
            SavePlaylist(wDir, playlist);
        }
    }

    /// <summary>
    /// Save playlist data into csv file
    /// </summary>
    /// <param name="path">Path of csv file when playlist is saved</param>
    /// <param name="playlist">playlist data to save (playlist name, tracks, etc...)</param>
    public static void SavePlaylist(string path, Models.Playlist playlist)
    {
        // Format filename
        string filename = PathUtil.FormatValidFilename(playlist.Name);

        // Format path
        string csvFilePath = Path.Combine(path, $"{filename}.csv");

        // TODO feature language: mettre avec le resx d'autres langues
        string headerLine = string.Join(",", new List<string> {
            "Track Name",
            "Artist Name(s)",
            "Album Name",
            "Album Artist Name(s)",
            "Album Release Date",
            "Disc Number",
            "Track Number",
            "Track Duration",
            "Explicit",
            "Popularity",
            "Added At",
            "Track Uri",
            "Artist Url",
            "Album Url",
            "Album Image Url",
            "Track Preview Url",
        });

        IEnumerable<string> dataLines = playlist.Tracks.Select(t =>
        string.Join(",",
            t.Name.Replace(",", "-"),
            t.Artists.Replace(",", "-").Trim(),
            t.AlbumName.Replace(",", "-").Trim(),
            t.AlbumArtistName.Replace(",", "-").Trim(),
            t.AlbumReleaseDate.Replace(",", "-").Trim(),
            t.DiscNumber,
            t.TrackNumber,
            t.Duration,
            t.Explicit,
            t.Popularity,
            t.AddedAt,
            t.TrackUri,
            t.ArtistUrl,
            t.AlbumUrl,
            t.AlbumImageUrl,
            t.TrackPreviewUrl)
        ).ToList();

        List<string> csvData = new()
        {
            headerLine
        };
        csvData.AddRange(dataLines);

        bool exported = false;
        do
        {
            try
            {
                File.WriteAllLines(csvFilePath, csvData);
                exported = true;
            }
            catch (IOException ex)
            {
                Logger.Error($"{ex} : Message: {ex.Message}\nStackTrace:{ex.StackTrace}");
                Logger.Error($"Le fichier {csvFilePath} est déjà ouvert par un autre processus" +
                    "\nVeuillez le fermer pour réessayer"); // TODO feature language
            }
        } while (!exported);

        Logger.Info($"Votre playlist est sauvegardé ici : {csvFilePath}"); // feature language
    }
}
