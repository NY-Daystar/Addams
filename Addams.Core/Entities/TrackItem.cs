using Newtonsoft.Json;

namespace Addams.Core.Entities;

public class TrackItem
{
    public DateTime? added_at { get; set; }
    public AddedBy? added_by { get; set; }
    public bool? is_local { get; set; }
    public object? primary_color { get; set; }

    /// <summary>
    /// Some api return "item" key and others "track" key
    /// so we use Track1 and Track2 props to match both case
    /// Return "item": with `/playlists/04y9jI5OX4GYkT2US9SipN`
    /// Return "track": with `/me/tracks`
    /// </summary>
    public TrackEntity Track { get; set; } = new TrackEntity();
    [JsonProperty(PropertyName = "item")]
    private TrackEntity Track1 { set => Track = value; }
    [JsonProperty(PropertyName = "track")]  
    private TrackEntity Track2 { set => Track = value; }

    public VideoThumbnail? video_thumbnail { get; set; }
}
