using Newtonsoft.Json;

namespace Addams.Entities;

/// <summary>
/// Token class to get OAUTH2 token authorization
/// </summary>
public class Token
{
    [JsonProperty(PropertyName = "access_token")]
    public string? AccessToken { get; set; }

    [JsonProperty(PropertyName = "token_type")]
    public string? TokenType { get; set; }

    [JsonProperty(PropertyName = "expires_in")]
    public int ExpiresIn { get; set; }

    [JsonProperty(PropertyName = "scope")]
    public string? Scope { get; set; }
}
