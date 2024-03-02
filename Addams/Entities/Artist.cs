using Newtonsoft.Json;

namespace Addams.Entities;

/// <summary>
/// Artist data
/// </summary>
public class Artist
{
    [JsonProperty(PropertyName = "external_urls")]
    public ExternalUrls? ExternalUrls { get; set; }

    [JsonProperty(PropertyName = "href")]
    public string? Href { get; set; }

    [JsonProperty(PropertyName = "id")]
    public string? Id { get; set; }

    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; } = string.Empty;

    [JsonProperty(PropertyName = "type")]
    public string? Type { get; set; }

    [JsonProperty(PropertyName = "uri")]
    public string? Uri { get; set; }
}
