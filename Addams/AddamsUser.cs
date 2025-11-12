using Addams.Utils;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Addams;

internal static class AddamsUser
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    /// <summary>
    /// Select the playlist list that the user want
    /// </summary>
    /// <returns>List of playlist</returns>
    public static IEnumerable<Entities.Playlist> SelectPlaylist(List<Entities.Playlist> playlists)
    {
        List<string> playlistNames = playlists.ConvertAll(p => p.Name).ToList();
        List<string> playlistChosen = new();

        int choice = 0;
        do
        {
            choice = ChoosePlaylist(playlistNames);

            if (choice != -1)
            {
                playlistChosen.Add(playlistNames[choice]);
                playlistNames.RemoveAt(choice);
            }
        } while (!playlistChosen.Any());

        return playlists.Where(p => playlistChosen.Contains(p.Name)).ToList();
    }

    /// <summary>
    /// Show all playlist fetchable and ask to the user which playlist he want to export
    /// </summary>
    /// <param name="playlistNames">Playlist names</param>
    /// <returns>-1 no choice or index of the playlist</returns>
    private static int ChoosePlaylist(IEnumerable<string> playlistNames)
    {
        foreach (string playlistName in playlistNames)
        {
            int index = playlistNames.ToList().IndexOf(playlistName) + 1;
            Console.WriteLine($"[{index}] - {playlistName}");
        }
        while (true)
        {
            Console.WriteLine($"{string.Format(Language.GetString("String1"), playlistNames.Count())}");
            Console.Write(Language.GetString("String2"));

            string key = Console.ReadLine() ?? string.Empty;
            int keyInt = 0;
            try
            {
                keyInt = Convert.ToInt32(key);
            }
            catch (FormatException)
            {
                Logger.Warn(Language.GetString("String3"));
                continue;
            }

            if (keyInt < 0 || keyInt > playlistNames.Count())
            {
                Logger.Warn($"\n{Language.GetString("String13")} '{key}'. " +
                    string.Format(Language.GetString("String14"), playlistNames.Count()));
            }
            else
            {
                return keyInt - 1;
            }
        }
    }

    /// <summary>
    /// Ask the user if he want to export all playlist
    /// Yes : means true, No means false
    /// </summary>
    /// <returns>bool of the pick</returns>
    public static bool AskAllPlaylistWanted()
    {
        bool stop = false, choice = false;
        do
        {
            Console.Write($"{Language.GetString("String15")}" +
                $"\n\t{Language.GetString("String16")}" +
                $"\t{Language.GetString("String17")}");


            char key = Console.ReadKey().KeyChar;

            if (key == '1')
            {
                stop = true;
                choice = true;
            }
            else if (key == '2')
            {
                stop = true;
                choice = false;
            }
            else
            {
                Console.WriteLine($"\n{Language.GetString("String13")} '{key}'. {Language.GetString("String18")}");
            }
        } while (!stop);

        Console.WriteLine();
        return choice;
    }
}