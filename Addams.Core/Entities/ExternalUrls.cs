using Newtonsoft.Json;

namespace Addams.Core.Entities;

/// <summary>
/// External Urls outside of Spotify (Yt, deezer, etc...)
/// </summary>
public class ExternalUrls
{
    [JsonProperty(PropertyName = "spotify")]
    public string? Spotify { get; set; }
}
