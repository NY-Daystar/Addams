using Addams.Entities;
using Addams.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Addams.Export
{
    // TODO to comment: classe qui gere la sauvegarde des fichiers
    public class SpotifyExport
    {
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
                Directory.CreateDirectory(wDir);

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

            Console.WriteLine($"STORE FILE HERE : {csvFilePath}"); // TODO put log

            // TODO mettre avec le resx d'autres langues
            string headerLine = string.Join(",", new List<string> {
                "Track Name", "Artist Name(s)", "Album Name",
                "Album Artist Name(s)", "Album Release Date","Disc Number",
                "Track Duration", "Track Number", "Explicit",
                "Popularity", "Added At", "Album Image Url",
                "Track Preview Url", "Track Uri", "Artist Url"
            });

            List<string> dataLines = playlist.Tracks.Select(t =>
            string.Join(",",
                t.Name.Replace(",", "-"), t.Artists, t.AlbumName,
                t.AlbumArtistName, t.AlbumReleaseDate, t.DiscNumber,
                t.TrackNumber, t.Duration, t.Explicit,
                t.Popularity, t.AddedAt, t.AlbumImageUrl,
                t.TrackPreviewUrl, t.TrackUri, t.ArtistUrl
                )).ToList();

            List<string> csvData = new();
            csvData.Add(headerLine);
            csvData.AddRange(dataLines);

            File.WriteAllLines(csvFilePath, csvData);
        }
    }
}
