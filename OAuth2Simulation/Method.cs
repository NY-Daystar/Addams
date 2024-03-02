using IdentityModel.Client; // Make sur NugetPackage IdentityModel is installed

namespace Oauth2Simulation;

/// <summary>
/// Method to generate Oauth2 token
/// </summary>
public class Method(string hostname, int port, string authorityUri, string redirectUri,
    string userId, string clientId, string clientSecret, string scope, string responseType)
{
    /// <summary>
    /// Hostname of uri redirection (default : http://localhost)
    /// </summary>
    public string Hostname { get; } = hostname;

    /// <summary>
    /// Port of uri redirection (default : 8888)
    /// </summary>
    public int Port { get; } = port;

    /// <summary>
    /// Url request for authorize application (Ex: for spotify: "https://accounts.spotify.com")
    /// </summary>
    public string AuthorityUri { get; } = authorityUri;

    /// <summary>
    /// Uri redirection /callback
    /// </summary>
    public string RedirectUri { get; } = redirectUri;

    /// <summary>
    /// Spotify username account
    /// </summary>
    public string UserId { get; } = userId;

    /// <summary>
    /// Spotify client app id
    /// </summary>
    public string ClientId { get; } = clientId;

    /// <summary>
    /// Spotify client app secret
    /// </summary>
    public string ClientSecret { get; } = clientSecret;

    /// <summary>
    /// Scope to define which access the Oauth2 token generated has
    /// </summary>
    public string Scope { get; } = scope;

    /// <summary>
    /// Type of response expected when authorization is asked (Ex for spotify: authorization code)
    /// </summary>
    public string ResponseType { get; } = responseType;
}