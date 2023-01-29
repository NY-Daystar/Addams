using Addams.Exceptions;
using NLog;
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
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Spotify username
        /// </summary>
        [JsonPropertyName("user")]
        public string User
        {
            get => "gravityx3";  // OAUTH2 TODO to delete
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
        /// TODO small-change: a changer en static
        [JsonPropertyName("token")]
        public string? Token { get; set; } = @"BQDgayBYqaebmO4dXyp5nSmpecjWDI5zegIJ1k3ERUtmAzPdEHQbN9KBQMgzWXNa_qybRpwMDK5h3cAME13JR9-soT2YKNhB6AJW-XHC81hLbquhd0scGlyEr17wwkp1oEDqdYtmNxUpFbtk3-PDc38HCjyy88FjvhCBTmMyiJtDIH6PGdtHU7T-6J7HIh7Uumtb0ITzm3JUN9EuPF1knLM";

        /// <summary>
        /// Datetime of last save
        /// </summary>
        [JsonPropertyName("datetime")]
        public string _datetime => DateTime.Now.ToString("yyyy-MM-dd_HH:mm:ss");

        /// <summary>
        /// Config file store in AppData folder : %APPDATA%\Addams
        /// </summary>
        /// TODO small-change: a combine avec un comma
        public static string filePath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"Addams\\config.json"); 

        /// <summary>
        /// Retrieve config if already exists if not we create it
        /// </summary>
        /// <returns>SpotifyConfig</returns>
        public static SpotifyConfig Get()
        {
            SpotifyConfig defaultConfig = new();
            SpotifyConfig config = new();

            try
            {
                config = Read();
                config.Token = defaultConfig.Token; // TODO get default token for now 
                Logger.Debug($"Config already exists:\n{config}");
            }
            catch (SpotifyConfigException)
            {
                config.Setup();
                Logger.Warn($"This config will be saved:\n{config}");
                config.Save();
            }
            return config;
        }

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
