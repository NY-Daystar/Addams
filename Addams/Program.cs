using Addams.Entities;
using Addams.Models;
using Addams.Service;
using Addams.Api;
using System;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Collections.Generic;

// TODO faire un README
// TODO faire un todo.txt pour la liste dans evernote
// TODO mettre un logger dans APPDATA
// TODO release2 : detecter l'obsolescence du token OAUTH2 et le regenerer puis le sauvegarder dauns un fichier de config dans APPDATA
// TODO faire un github action pour lancer les tests unitaires
// TODO sonarCube pour l'analyse de code
// TODO ajouter du multilangue avec un fichier Resx
// TODO faire des tests unitaires

namespace Addams
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Run().GetAwaiter().GetResult();       
        }

        public static async Task Run()
        {
            // TODO le mettre dans une config et lors du premier lancement le demander au user 
            // TODO possibilité de le changer aussi avec une option
            string user = "gravityx3";

            List<Models.Playlist>? playlists = await GetPlaylists(user);

            if(playlists == null)
            {
                // TODO put log
                return;
            }

            SavePlaylists("TT.csv", playlists);
            
            // TEST pour le token
            //var r = await pro.GetPlaylistsItems();
            //Console.WriteLine(r);
        }
        
        /// <summary>
        /// Get playlist data of user to save it after
        /// TODO doc
        /// </summary>
        /// <returns></returns>
        public static async Task<List<Models.Playlist>> GetPlaylists(string user)
        {
            Console.WriteLine("Getting playlist data...");
            SpotifyService service = new SpotifyService(user);

            // Get playlist
            List<Models.Playlist>? playlists = await service.GetPlaylists();

            if(playlists == null)
            {
                return new List<Models.Playlist>();
            }
          
            //Console.WriteLine($"HREF playlist : {playlists.href}"); // TODO has to work

            return playlists;
        }

        // TODO une fois que j'ai les data faire un SavePlaylist
        /// <summary>
        /// Save playlist data into csv file
        /// </summary>
        /// <param name="path">Path of csv file to save playlists</param>
        /// <param name="data">List of playlist to save</param>
        public static void SavePlaylists(string path, List<Models.Playlist> data)
        {
            // TODO faire un save dans un csv du fichier
        }
    }
}