using Addams.Exceptions;
using Addams.Models;
using NLog;
using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Addams;

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
    public string UserName { get; set; } = string.Empty;

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
    public TokenModel? Token { get; set; }

    /// <summary>
    /// Hostname of uri redirection (default : http://localhost)
    /// </summary>
    [JsonPropertyName("hostname")]
    public const string Hostname = "http://localhost";

    /// <summary>
    /// Port of uri redirection (default : 8888)
    /// </summary>
    [JsonPropertyName("port")]
    public const int Port = 8888;

    /// <summary>
    /// Url request for authorize application (Ex: for spotify: "https://accounts.spotify.com")
    /// </summary>
    [JsonIgnore]
    public static string AuthorityUri { get => "https://accounts.spotify.com"; }

    /// <summary>
    /// Uri redirection /callback
    /// </summary>
    [JsonIgnore]
    public static string RedirectUri { get => $"{Hostname}:{Port}/callback"; }

    /// <summary>
    /// Type of response expected when authorization is asked (Ex for spotify: authorization code)
    /// </summary>
    [JsonIgnore]
    public const string ResponseType = "code";

    /// <summary>
    /// Scope to define which access the Oauth2 token generated has
    /// </summary>
    [JsonIgnore]
    public const string Scope = "playlist-read-private user-library-read";

    /// <summary>
    /// Datetime of last save
    /// </summary>
    [JsonPropertyName("datetime")]
    public static string Datetime => DateTime.UtcNow.ToString("yyyy-MM-dd_HH:mm:ss");

    /// <summary>
    /// Config file store in AppData folder : %APPDATA%\Addams
    /// </summary>
    public static string ConfigFilepath
    {
        get
        {
            const string _appDataFolderName = "Addams"; // Name of the project for %APPDATA% folder
            const string _appDataConfigFilename = "config.json"; // Config file name 
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), _appDataFolderName, _appDataConfigFilename);
        }
    }

    /// <summary>
    /// Retrieve config if already exists if not we create it
    /// </summary>
    /// <returns>SpotifyConfig</returns>
    public static SpotifyConfig Get()
    {
        SpotifyConfig config = new();

        try
        {
            config = Read();
            Logger.Debug($"Config already exists:\n{config}");
        }
        catch (SpotifyConfigException ex)
        {
            Logger.Debug($"Configuration cannot be read {ex.Message}");
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
        UserName = Console.ReadLine() ?? string.Empty;

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
                (Path.GetDirectoryName(ConfigFilepath)
                ?? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Addams"))
                ?? throw new DirectoryNotFoundException($"SpotifyConfig - Save method can't find folder based on path {ConfigFilepath}");
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

        return UserName == p.UserName
            && ClientID == p.ClientID
            && ClientSecret == p.ClientSecret
            && Token?.Equals(p.Token) == true;
    }

    public override int GetHashCode()
    {
        return UserName.GetHashCode() + ClientID.GetHashCode() + ClientSecret.GetHashCode();
    }

    public override string ToString()
    {
        return $"\tUser: '{UserName}'\n" +
            $"\tClientID: '{ClientID}'\n" +
            $"\tClientSecret: '{ClientSecret}'\n" +
            $"\tToken: '{Token}'\n" +
            $"\tDatetime: '{Datetime}'";
    }
}
