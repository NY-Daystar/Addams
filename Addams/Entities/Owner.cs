using Newtonsoft.Json;

namespace Addams.Entities;

/// <summary>
/// Owner data
/// </summary>
public class Owner
{
    [JsonProperty(PropertyName = "display_name")]
    public string? DisplayName { get; set; }

    [JsonProperty(PropertyName = "external_urls")]
    public ExternalUrls? ExternalUrls { get; set; }

    [JsonProperty(PropertyName = "href")]
    public string? Href { get; set; }

    [JsonProperty(PropertyName = "id")]
    public string? Id { get; set; }

    [JsonProperty(PropertyName = "type")]
    public string? Type { get; set; }

    [JsonProperty(PropertyName = "uri")]
    public string? Uri { get; set; }
}
