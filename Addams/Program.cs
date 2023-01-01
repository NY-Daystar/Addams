using Addams.Export;
using Addams.Service;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

// TODO faire un README
// TODO faire un todo.txt pour la liste dans evernote
// TODO mettre un logger dans APPDATA
// TODO release2 : detecter l'obsolescence du token OAUTH2 et le regenerer puis le sauvegarder dauns un fichier de config dans APPDATA
// TODO faire un github action pour lancer les tests unitaires
// TODO sonarCube pour l'analyse de code
// TODO ajouter du multilangue avec un fichier Resx
// TODO faire des tests unitaires
// TODO try catch sur les exceptions

namespace Addams
{
    internal class Program
    {
        public static void Main()
        {
            Run().GetAwaiter().GetResult();
        }

        public static async Task Run()
        {
            // TODO faire une class config avec une method read et une method write
            // TODO la rendre serializable https://learn.microsoft.com/en-us/dotnet/standard/serialization/basic-serialization
            // TODO faire une method askValue aussi pour recuperer le nom de user et de clientid
            // TODO le mettre dans une config et lors du premier lancement le demander au user 
            // TODO possibilité de le changer aussi avec une option
            string user = "gravityx3";

            Console.WriteLine("Fetching playlist data...");
            List<Models.Playlist>? playlists = await GetPlaylists(user);
            if (playlists == null)
            {
                // TODO put log
                return;
            }
            Console.WriteLine("Playlist fetched...");

            Console.WriteLine("Saving playlist...");
            SpotifyExport.SavePlaylists(playlists);
            Console.WriteLine("Playlist saved...");
        }

        /// <summary>
        /// Get playlist data of user to save it after
        /// </summary>
        /// <param name="user">username to get playlist</param>
        /// <returns>List of playlists to save</returns>
        public static async Task<List<Models.Playlist>> GetPlaylists(string user)
        {
            SpotifyService service = new(user);

            // Get playlist
            List<Models.Playlist>? playlists = await service.GetPlaylists();

            if (playlists == null)
            {
                return new List<Models.Playlist>();
            }
            return playlists;
        }
    }
}