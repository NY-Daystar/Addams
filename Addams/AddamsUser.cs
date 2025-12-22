using Addams.Exceptions;
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
            choice = ChoosePlaylist(playlistNames) ?? 0;

            if (choice != -1)
            {
                var playlist = playlistNames[choice];
                Core.WriteLine(Language.GetString("String54"), ConsoleColor.Green, playlist);
                playlistChosen.Add(playlist);
                playlistNames.RemoveAt(choice);
            }
        } while (choice != -1);

        Core.WriteLine(Language.GetString("String55"), ConsoleColor.Green, $"\n\t - {string.Join("\n\t - ", playlistChosen)}");

        return playlists.Where(p => playlistChosen.Contains(p.Name)).ToList();
    }

    /// <summary>
    /// Show all playlist fetchable and ask to the user which playlist he want to export
    /// </summary>
    /// <param name="playlistNames">Playlist names</param>
    /// <returns>-1 no choice or index of the playlist</returns>
    private static int? ChoosePlaylist(IEnumerable<string> playlistNames)
    {
        foreach (string playlistName in playlistNames)
        {
            int index = playlistNames.ToList().IndexOf(playlistName) + 1;
            Console.WriteLine($"[{index}] - {playlistName}");
        }
        while (true)
        {
            Console.WriteLine($"{string.Format(Language.GetString("String1"), playlistNames.Count())}");

            string key = Console.ReadLine() ?? string.Empty;
            int? keyInt;
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
    /// Ask the user if he want to export all playlist, or show config
    /// </summary>
    /// <returns>char of the pick</returns>
    public static string AskWhatToDo()
    {
        bool noretry = true;
        string? choice;
        do
        {
            Console.WriteLine($"\n{Language.GetString("String49")}" +
                $"\n\t[1]: {Language.GetString("String50")}" +
                $"\n\t[2]: {Language.GetString("String62")}" +
                $"\n\t[3]: {Language.GetString("String51")}" +
                $"\n\t[4]: {Language.GetString("String52")}" +
                $"\n\t[5]: {Language.GetString("String53")}");

            choice = Console.ReadKey().KeyChar.ToString() ?? "1";
            if (choice != "1" && choice != "2" && choice != "3" && choice != "4" && choice != "5")
            {
                Console.WriteLine($"\n{Language.GetString("String13")} '{choice}'. {Language.GetString("String18")}");
                noretry = false;
            }
        } while (!noretry);

        Console.WriteLine();
        return choice;
    }

    /// <summary>
    /// Ask the user if he want to export all playlist, or show config
    /// Yes : means true, No means false
    /// </summary>
    /// <returns>bool of the pick</returns>
    public static bool AskAllPlaylistWanted()
    {
        bool noretry = true, choice = false;
        do
        {
            Console.Write($"{Language.GetString("String15")}" +
                $"\n\t{Language.GetString("String16")}" +
                $"\t{Language.GetString("String17")}");

            char key = Console.ReadKey().KeyChar;

            if (key == '1')
            {
                choice = true;
            }
            else if (key == '2')
            {
                choice = false;
            }
            else
            {
                Console.WriteLine($"\n{Language.GetString("String13")} '{key}'. {Language.GetString("String18")}");
                noretry = false;
            }
        } while (!noretry);

        Console.WriteLine();
        return choice;
    }

    /// <summary>
    /// The user can seize is refresh token from spotify
    /// </summary>
    /// <returns>string with 64-128 char</returns>
    public static string SeizeData()
    {
        string token = string.Empty;
        bool noretry = true;
        do
        {
            Console.Write($"{Language.GetString("String57")}: ");

            token = Console.ReadLine() ?? string.Empty;

            if (token == null || token.Equals(string.Empty))
            {
                Console.WriteLine($"\n{Language.GetString("String13")} '{token}'. {Language.GetString("String18")}");
                noretry = false;
            }
        } while (!noretry);

        Console.WriteLine();
        if (token == null)
            throw new AddamsUserException("token seized is null");

        return token;
    }
}

