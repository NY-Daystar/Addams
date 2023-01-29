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
        [JsonPropertyName("token")]
        public string Token { get; set; } = _token;

        private static readonly string _token = @"BQCS1lHoufRq4ZMxvU_EepMmQxGoPOGfekIb3FMTfIeBDA4iqh62CSj1_Rrlbql1DiXFdRzATTf1wfjAPhghsYGu3HqhUJQ0t9rKkgXYyJZ6bIkVe3b8pZHtmfkocQ069FXQaxktjQ6aH10QZlMk72M8T8hCZkUEeBfDHWXwGiFDUaI8gTWEeb1Kyx1791NWBgmyShSIv4IC_N8Yk0Yw8gA";

        /// <summary>
        /// Datetime of last save
        /// </summary>
        [JsonPropertyName("datetime")]
        public string _datetime => DateTime.Now.ToString("yyyy-MM-dd_HH:mm:ss");

        /// <summary>
        /// Config file store in AppData folder : %APPDATA%\Addams
        /// </summary>
        public static string ConfigFilepath
        {
            get
            {
                string _appDataFolderName = "Addams"; // Name of the project for %APPDATA% folder
                string _appDataConfigFilename = "config.json"; // Config file name 
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), _appDataFolderName, _appDataConfigFilename);
            }
        }

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
            if (!File.Exists(ConfigFilepath))
            {
                string folder =
                    Path.GetDirectoryName(ConfigFilepath)
                    ?? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"Addams"); ;
                if (folder == null)
                {
                    throw new DirectoryNotFoundException($"SpotifyConfig - Save method can't find folder based on path {ConfigFilepath}");
                }

                if (!Directory.Exists(folder))
                {
                    _ = Directory.CreateDirectory(folder);
                }
                File.Create(ConfigFilepath).Close();
            }
            JsonSerializerOptions options = new()
            {
                WriteIndented = true,
            };
            string content = JsonSerializer.Serialize(this, options);
            File.WriteAllText(ConfigFilepath, content);
        }

        /// <summary>
        /// Deserialize json file content store in appdata
        /// </summary>
        /// <returns>SpotiifyConfig with datavalue from file</returns>
        public static SpotifyConfig Read()
        {
            try
            {
                string content = File.ReadAllText(ConfigFilepath);
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
