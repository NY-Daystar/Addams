using Newtonsoft.Json;

namespace Addams.Entities;

/// <summary>
/// External Ids outside of Spotify (Youtube, deezer, etc...)
/// </summary>
public class ExternalIds
{
    [JsonProperty(PropertyName = "isrc")]
    public string? Isrc { get; set; }
}
