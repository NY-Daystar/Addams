using Newtonsoft.Json;
using System.Collections.Generic;

namespace Addams.Entities;

/// <summary>
/// Track data
/// </summary>
public class Track
{
    [JsonProperty(PropertyName = "album")]
    public Album Album { get; set; } = new Album();

    [JsonProperty(PropertyName = "artists")]
    public IEnumerable<Artist> Artists { get; set; } = new List<Artist>();

    [JsonProperty(PropertyName = "available_markets")]
    public IEnumerable<string>? AvailableMarkets { get; set; }

    [JsonProperty(PropertyName = "disc_number")]
    public int DiscNumber { get; set; }

    [JsonProperty(PropertyName = "duration_ms")]
    public int DurationMs { get; set; }

    [JsonProperty(PropertyName = "@explicit")]
    public bool Explicit { get; set; }

    [JsonProperty(PropertyName = "external_ids")]
    public ExternalIds? ExternalIds { get; set; }

    [JsonProperty(PropertyName = "external_urls")]
    public ExternalUrls? ExternalUrls { get; set; }

    [JsonProperty(PropertyName = "href")]
    public string? Href { get; set; }

    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; } = string.Empty;

    [JsonProperty(PropertyName = "is_local")]
    public bool IsLocal { get; set; }

    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; } = string.Empty;

    [JsonProperty(PropertyName = "popularity")]
    public int Popularity { get; set; }

    [JsonProperty(PropertyName = "preview_url")]
    public string PreviewUrl { get; set; } = string.Empty;

    [JsonProperty(PropertyName = "track_number")]
    public int TrackNumber { get; set; }

    [JsonProperty(PropertyName = "type")]
    public string? Type { get; set; }

    [JsonProperty(PropertyName = "uri")]
    public string Uri { get; set; } = string.Empty;
}
