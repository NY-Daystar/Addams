using Addams.Exceptions;
using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

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
        [JsonPropertyName("user")]
        public string User
        {
            get => "gravityx3";  // TODO to delete
            set { }
        }

        /// <summary>
        /// Spotify Client ID from app created in spotify account
        /// </summary>
        [JsonPropertyName("clientID")]
        public string ClientID { get; set; } = string.Empty;

        /// <summary>
        /// Spotify Client secret from app created in spotify account
        /// </summary>
        [JsonPropertyName("clientSecret")]
        public string ClientSecret { get; set; } = string.Empty;

        /// <summary>
        /// OAuth2 token generated 
        /// Default OAuth2 token
        /// </summary>
        [JsonPropertyName("token")]
        public string? Token { get; set; } = @"BQAhtzzQk2blmXSvV_lHNdsRp0OQNYtF13F1OoUFHuZ18Q8gi7vOi9POp7vhv8NHkXC-xO9zjpwDGd5aRYsrz1uCP7yQlh6PQyiOkNMkHFtzB3CwwCbcsBNnih6PTjWyLjlM0GgrIvzX_o14YBEty-f4wcXE7ybb_GOOzjZuJLiZwn3jGsCwy62UI9eN0rwFHxjbISgETG6xKE8nAoLJzA";

        /// <summary>
        /// Datetime of last save
        /// </summary>
        [JsonPropertyName("datetime")]
        public string _datetime => DateTime.Now.ToString("yyyy-MM-dd_HH:mm:ss");

        /// <summary>
        /// Config file store in AppData folder : %APPDATA%\Addams
        /// </summary>
        public static string filePath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"Addams\\config.json"); // TODO a combine avec un comma


        /// <summary>
        /// Ask data of user to create configuration program
        /// </summary>
        public void Setup()
        {
            Console.Write("Enter your spotify username: ");
            User = Console.ReadLine() ?? string.Empty;

            Console.Write("Enter your spotify clientID: ");
            ClientID = Console.ReadLine() ?? string.Empty;

            Console.Write("Enter your spotify clientSecret: ");
            ClientSecret = Console.ReadLine() ?? string.Empty;
        }

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
                {
                    throw new DirectoryNotFoundException($"SpotifyConfig - Save method can't find folder based on path {filePath}");
                }

                if (!Directory.Exists(folder))
                {
                    _ = Directory.CreateDirectory(folder);
                }
                File.Create(filePath).Close();
            }
            JsonSerializerOptions options = new()
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
            try
            {
                string content = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<SpotifyConfig?>(content) ?? new SpotifyConfig();
            }
            catch
            {
                throw new SpotifyConfigException();
            }
        }

        public override bool Equals(object? obj)
        {
            //Check for null and compare run-time types.
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            SpotifyConfig p = (SpotifyConfig)obj;

            return User == p.User
                && ClientID == p.ClientID
                && ClientSecret == p.ClientSecret
                && Token == p.Token;
        }

        public override int GetHashCode()
        {
            return User.GetHashCode() + ClientID.GetHashCode() + ClientSecret.GetHashCode();
        }

        public override string ToString()
        {
            return $"\tUser: '{User}'\n" +
                $"\tClientID: '{ClientID}'\n" +
                $"\tClientSecret: '{ClientSecret}'\n" +
                $"\tToken: '{Token}'\n" +
                $"\tDatetime: '{_datetime}'";
        }
    }
}
