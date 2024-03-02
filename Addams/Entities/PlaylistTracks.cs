using Newtonsoft.Json;
using System.Collections.Generic;

namespace Addams.Entities;

/// <summary>
/// Playlist's tracks entity fetch from spotify api when request /playlist/{id}/
/// to deserialize: PlaylistTracks myDeserializedClass = JsonConvert.DeserializeObject<PlaylistTracks>(myJsonResponse);
/// </summary>
public class PlaylistTracks
{
    [JsonProperty(PropertyName = "collaborative")]
    public bool? Collaborative { get; set; }

    [JsonProperty(PropertyName = "description")]
    public string? Description { get; set; }

    [JsonProperty(PropertyName = "external_urls")]
    public ExternalUrls? ExternalUrls { get; set; }

    [JsonProperty(PropertyName = "followers")]
    public Followers? Followers { get; set; }

    [JsonProperty(PropertyName = "href")]
    public string? Href { get; set; }

    [JsonProperty(PropertyName = "id")]
    public string? Id { get; set; }

    [JsonProperty(PropertyName = "images")]
    public IEnumerable<Image>? Images { get; set; }

    [JsonProperty(PropertyName = "name")]
    public string? Name { get; set; }

    [JsonProperty(PropertyName = "owner")]
    public Owner? Owner { get; set; }

    [JsonProperty(PropertyName = "primary_color")]
    public object? PrimaryColor { get; set; }

    [JsonProperty(PropertyName = "@public")]
    public bool? Public { get; set; }

    [JsonProperty(PropertyName = "snapshot_id")]
    public string? SnapshotId { get; set; }

    [JsonProperty(PropertyName = "tracks")]
    public TrackList Tracks { get; set; } = new TrackList();

    [JsonProperty(PropertyName = "type")]
    public string? Type { get; set; }

    [JsonProperty(PropertyName = "uri")]
    public string? Uri { get; set; }
}

public class AddedBy
{
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

public class Followers
{
    [JsonProperty(PropertyName = "href")]
    public object? Href { get; set; }

    [JsonProperty(PropertyName = "total")]
    public int? Total { get; set; }
}

public class VideoThumbnail
{
    [JsonProperty(PropertyName = "url")]
    public object? Url { get; set; }
}