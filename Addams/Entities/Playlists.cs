using Newtonsoft.Json;
using System.Collections.Generic;

namespace Addams.Entities;

/// <summary>
/// Playlist entity fetch from spotify api when request /playlists/
/// to deserialize: Playlists myDeserializedClass = JsonConvert.DeserializeObject<Playlists>(myJsonResponse);
/// </summary>
public class Playlists
{
    [JsonProperty(PropertyName = "href")]
    public string? Href { get; set; }

    [JsonProperty(PropertyName = "items")]
    public IEnumerable<Playlist> Items { get; set; } = new List<Playlist>();

    [JsonProperty(PropertyName = "limit")]
    public int? Limit { get; set; }

    [JsonProperty(PropertyName = "next")]
    public string? Next { get; set; }

    [JsonProperty(PropertyName = "offset")]
    public int? Offset { get; set; }

    [JsonProperty(PropertyName = "previous")]
    public object? Previous { get; set; }

    [JsonProperty(PropertyName = "total")]
    public int? Total { get; set; }
}

public class Playlist
{
    [JsonProperty(PropertyName = "collaborative")]
    public bool? Collaborative { get; set; }

    [JsonProperty(PropertyName = "description")]
    public string Description { get; set; } = string.Empty;

    [JsonProperty(PropertyName = "external_urls")]
    public ExternalUrls? ExternalUrls { get; set; }

    [JsonProperty(PropertyName = "href")]
    public string Href { get; set; } = string.Empty;

    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; } = string.Empty;

    [JsonProperty(PropertyName = "images")]
    public IEnumerable<Image>? Images { get; set; }

    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; } = string.Empty;

    [JsonProperty(PropertyName = "owner")]
    public Owner? Owner { get; set; }

    [JsonProperty(PropertyName = "primary_color")]
    public object? PrimaryColor { get; set; }

    [JsonProperty(PropertyName = "@public")]
    public bool? Public { get; set; }

    [JsonProperty(PropertyName = "snapshot_id")]
    public string? SnapshotId { get; set; }

    [JsonProperty(PropertyName = "tracks")]
    public TrackList? Tracks { get; set; }

    [JsonProperty(PropertyName = "type")]
    public string? Type { get; set; }

    [JsonProperty(PropertyName = "uri")]
    public string? Uri { get; set; }
}
