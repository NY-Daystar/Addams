using Addams.Utils;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Addams
{

    /// <summary>
    /// Class to handle csv export data file with spotify tracks data
    /// </summary>
    public class SpotifyExport
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Save playlist data into csv file
        /// </summary>
        /// <param name="data">List of playlist to save</param>
        public static void SavePlaylists(List<Models.Playlist> data)
        {
            // TODO gerer un mode specific pour tout save dans un fichier ou plusieurs
            // TODO faire un mode si on save tout dans un fichier ou une playlist par fichier
            // TODO faire un save dans un csv du fichier

            string exePath = System.Reflection.Assembly.GetExecutingAssembly().Location ?? throw new Exception("exe path is null");

            string exeFolder = Path.GetDirectoryName(exePath) ?? throw new Exception("exe folder path is null");

            string wDir = Path.Combine(exeFolder, "data");
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


        //TODO voir si je peux faire un seul .xlsx avec tous les onglets
        // TODO faire un mode si on save tout dans un fichier ou une playlist par fichier
        // TODO faire un save dans un csv du fichier
        // TODO to comment
        /// <summary>
        /// Save playlist data into csv file
        /// </summary>
        /// <param name="data">List of playlist to save</param>
        /// 
        public static void SavePlaylist(string path, Models.Playlist playlist)
        {
            // Format filename
            string filename = PathUtil.FormatValidFilename(playlist.Name);

            // Format path
            string csvFilePath = Path.Combine(path, $"{filename}.csv");
            //csvFilePath = PathUtil.FormatValidPath(csvFilePath);

            Logger.Info($"STORE FILE HERE : {csvFilePath}"); // TODO put log

            // TODO mettre avec le resx d'autres langues
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

            List<string> dataLines = playlist.Tracks.Select(t =>
            string.Join(",",
                t.Name.Replace(",", "-"),
                t.Artists,
                t.AlbumName,
                t.AlbumArtistName,
                t.AlbumReleaseDate,
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
                headerLine // TODO info IDE0028: Collection initialization can be simplified 
            };
            csvData.AddRange(dataLines);

            File.WriteAllLines(csvFilePath, csvData);
            // TODO gerer l'exceptiion dans le cas ou le csv est open
            // System.IO.IOException: 'The process cannot access the file
            // 'D:\Dev\Addams\Addams\bin\Debug\net7.0\data\A ecouter.csv' because it is being used by another process.'
        }
    }
}
