using Addams.Core.Logs;
using Addams.Core.Exceptions;
using Addams.Core.Models;
using Addams.Core.Utils;
using Newtonsoft.Json;

namespace Addams.Core;

/// <summary>
/// Config to use this project
/// Store mainly info for Spotify OAuth Token
/// </summary>

public class AddamsConfig
{
    /// <summary>
    /// Spotify username
    /// </summary>
    [JsonProperty("user")]
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// Spotify Client ID from app created in spotify account
    /// </summary>
    [JsonProperty("clientID")]
    public string ClientID { get; set; } = string.Empty;

    /// <summary>
    /// Spotify Client secret from app created in spotify account
    /// </summary>
    [JsonProperty("clientSecret")]
    public string ClientSecret { get; set; } = string.Empty;

    /// <summary>
    /// OAuth2 token generated
    /// Default OAuth2 token
    /// </summary>
    [JsonProperty("token")]
    public TokenModel Token = new();

    /// <summary>
    /// Light or Dark Theme
    /// </summary>
    [JsonProperty("theme")]
    public AddamsTheme Theme { get; set; }

    /// <summary>
    /// Language (default : "en-US")
    /// </summary>
    [JsonProperty("language")]
    public string AppLanguage = "en-US";

    /// <summary>
    /// Debug mode (default : "false")
    /// </summary>
    [JsonProperty("debug")]
    public bool IsDebugMode = false;

    /// <summary>
    /// Hostname of uri redirection (default : http://127.0.0.1/callback)
    /// </summary>
    [JsonProperty("hostname")]
    public const string Hostname = "http://127.0.0.1";

    /// <summary>
    /// Port of uri redirection (default : 8888)
    /// </summary>
    [JsonProperty("port")]
    public const int Port = 8888;

    /// <summary>
    /// Url request for authorize application (Ex for spotify: "https://accounts.spotify.com")
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
    /// Method name to cypher code verifyer in PKCE authentication 
    /// </summary>
    [JsonIgnore]
    public const string ChallengeMethod = "S256";

    /// <summary>
    /// Datetime of last save
    /// </summary>
    [JsonProperty("datetime")]
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
    public static AddamsConfig TryGet()
    {
        AddamsConfig config = new();

        try
        {
            config = Get();
            LoggerManager.Log(string.Format(Language.GetString("String2"), config), Level.Debug);
        }
        catch (SpotifyConfigException ex)
        {
            LoggerManager.Log(string.Format(Language.GetString("String3"), ex.Message), Level.Warning);
            LoggerManager.Log(Language.GetString("String25"), Level.Important);
            config.Save();
        }
        return config;
    }

    /// <summary>
    /// Setup file and Directory if not exists
    /// Serialize object to save json file in appData
    /// </summary>
    /// <exception cref="DirectoryNotFoundException"></exception>
    public void Save()
    {
        
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

        string content = JsonConvert.SerializeObject(this, Formatting.Indented);
        File.WriteAllText(ConfigFilepath, content);
    }

    /// <summary>
    /// Deserialize json file content store in appdata
    /// </summary>
    /// <returns>SpotiifyConfig with datavalue from file</returns>
    public static AddamsConfig Get()
    {
        try
        {
            string content = File.ReadAllText(ConfigFilepath);
            LoggerManager.Log(string.Format(Language.GetString("String4"), ConfigFilepath), Level.Debug);
            return JsonConvert.DeserializeObject<AddamsConfig?>(content) ?? new AddamsConfig();
        }
        catch
        {
            throw new SpotifyConfigException();
        }
    }

    public void Reset()
    {
        Token.Refresh = string.Empty;
        Token.Value = string.Empty;
        Save();
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        AddamsConfig p = (AddamsConfig)obj;

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
        if (Token?.Value == null)
            return $"\tUser: '{UserName}'\n" +
                $"\tClientID: '{ClientID}'\n" +
                $"\tClientSecret: '{ClientSecret}'\n";

        return $"\tUser: '{UserName}'\n" +
               $"\tClientID: '{ClientID}'\n" +
               $"\tClientSecret: '{ClientSecret}'\n" +
               $"\tAccessToken: '{Token.Value}'\n" +
               $"\tRefreshToken: '{Token.Refresh}'\n" +
               $"\tGeneratedAt: '{Token.GeneratedAt}'\n" +
               $"\tExpiresIn: '{Token.ExpiresIn}'\n" +
               $"\tExpireAt: '{Token.ExpiredDate}'\n" +
               $"\tDatetime: '{Datetime}'";
    }
}
