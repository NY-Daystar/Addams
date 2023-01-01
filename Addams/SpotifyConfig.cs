using System;
using System.Drawing;
using System.IO;
using System.Text.Json;

namespace Addams
{
    /// <summary>
    /// Config to use this project 
    /// Store mainly info to create OAUTH token
    /// </summary>
    public class SpotifyConfig
    {
        /// <summary>
        /// Spotify username
        /// </summary>
        public string user { get; set; } = String.Empty;

        /// <summary>
        /// Spotify Client ID from app created in spotify account
        /// </summary>
        public string clientID { get; set; } = String.Empty;

        /// <summary>
        /// Spotify Client secret from app created in spotify account
        /// </summary>
        public string clientSecret { get; set; } = String.Empty;

        /// <summary>
        /// Oauth2 token generated
        /// </summary>
        public string token { get; set; } = String.Empty;

        /// <summary>
        /// Config file store in AppData folder
        /// </summary>
        public static string filePath
        {
            get
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"Addams\\config.json");
            }
            set { }
        }

        //$@"Path.Con{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}";

        /// <summary>
        /// Serialize object to save json file in appData
        /// </summary>
        /// <exception cref="DirectoryNotFoundException"></exception>
        public void Save()
        {
            // Setup file and Directory if not exists
            if (!File.Exists(filePath))
            {
                string folder =
                    Path.GetDirectoryName(filePath)
                    ?? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"Addams"); ;
                if (folder == null)
                    throw new DirectoryNotFoundException($"SpotifyConfig - Save method can't find folder based on path {filePath}");
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                File.Create(filePath).Close();
            }
            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                WriteIndented = true,
            };
            string content = JsonSerializer.Serialize(this, options);
            File.WriteAllText(filePath, content);
        }

        /// <summary>
        /// Deserialize json file content store in appdata
        /// </summary>
        /// <returns>SpotiifyConfig with datavalue from file</returns>
        public static SpotifyConfig Read()
        {
            string content = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<SpotifyConfig>(content) ?? new SpotifyConfig();
        }

        public override bool Equals(Object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                SpotifyConfig p = (SpotifyConfig)obj;
                return user == p.user
                    && clientID == p.clientID
                    && clientSecret == p.clientSecret
                    && token == p.token;
            }
        }
    }
}
