using Addams.Core.Logs;
using Addams.Core.Utils;

namespace Addams.Core.Spotify;

/// <summary>
/// Class to handle csv export data file with spotify tracks data
/// </summary>
public static class SpotifyExport
{
    /// <summary>
    /// Save playlist data into csv file
    /// </summary>
    /// <param name="path">Folder path where save the csv files</param>
    /// <param name="data">List of playlist to save</param>
    public static void SavePlaylists(string path, IEnumerable<Models.Playlist> data)
    {
        if (!Directory.Exists(path))
        {
            _ = Directory.CreateDirectory(path);
        }

        // Save each playlist
        foreach (Models.Playlist playlist in data)
        {
            SavePlaylist(path, playlist);
        }
    }

    /// <summary>
    /// Save playlist data into csv file
    /// </summary>
    /// <param name="path">Path of csv file when playlist is saved</param>
    /// <param name="playlist">playlist data to save (playlist name, tracks, etc...)</param>
    public static void SavePlaylist(string path, Models.Playlist playlist)
    {
        string filename = PathUtil.FormatValidFilename(playlist.Name);
        string csvFilePath = Path.Combine(path, Path.GetFileName($"{filename}.csv"));

        string headerLine = string.Join(",", new List<string> {
            Language.GetString("String33"),
            Language.GetString("String34"),
            Language.GetString("String35"),
            Language.GetString("String36"),
            Language.GetString("String37"),
            Language.GetString("String38"),
            Language.GetString("String39"),
            Language.GetString("String40"),
            Language.GetString("String41"),
            Language.GetString("String43"),
            Language.GetString("String44"),
            Language.GetString("String45"),
            Language.GetString("String46"),
            Language.GetString("String47"),
        });

        IEnumerable<string> dataLines = [.. playlist.Tracks.Select(t =>
        string.Join(",",
            t.Name.Replace(",", "-"),
            t.Artists.Replace(",", "-").Trim(),
            t.AlbumName.Replace(",", "-").Trim(),
            t.AlbumArtistName.Replace(",", "-").Trim(),
            t.AlbumReleaseDate.Replace(",", "-").Trim(),
            t.DiscNumber,
            t.TrackNumber,
            t.DurationFormatted,
            t.Explicit,
            t.AddedAt,
            t.TrackUri,
            t.ArtistUrl,
            t.AlbumUrl,
            t.AlbumImageUrl)
        )];

        List<string> csvData = [headerLine, ..dataLines];

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
                LoggerManager.Log($"{ex} : Message: {ex.Message}\nStackTrace:{ex.StackTrace}", Level.Error);
                LoggerManager.Log(string.Format(Language.GetString("String26"), csvFilePath), Level.Error);
                LoggerManager.Log(Language.GetString("String27"), Level.Error);
            }
        } while (!exported);

        LoggerManager.Log(string.Format(Language.GetString("String30"), csvFilePath));
    }
}
