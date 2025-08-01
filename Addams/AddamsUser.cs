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

        Logger.Debug($"playlistChosen : {string.Join("; ", playlistChosen)}");

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
            Console.Write("Which playlist do you want to export "
                + $"(1 - {playlistNames.Count()}) ? \ntype 0 to exit : ");  // TODO feature language

            string key = Console.ReadLine() ?? string.Empty;
            int keyInt;
            try
            {
                keyInt = Convert.ToInt32(key);
            }
            catch (FormatException)
            {
                Logger.Warn("You typed a non-numeric value, please type a numeric value"); // TODO feature language
                continue;
            }

            if (keyInt < 0 || keyInt > playlistNames.Count())
            {
                Logger.Warn($"\nYou type '{key}'. " +
                    $"It's invalid, please choose between 0 and {playlistNames.Count()}"); // TODO feature language
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
            Console.Write("Do you want to export all playlist\n    [1]:Yes\t[2]:No : "); // TODO feature language

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
                Console.WriteLine($"\nYou type '{key}'. Please choose '1' or '2'"); // TODO feature language
            }
        } while (!stop);

        Console.WriteLine();
        return choice;
    }
}