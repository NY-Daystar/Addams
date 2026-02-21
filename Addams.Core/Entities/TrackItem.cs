using Addams.Core.Exceptions;
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
    public TrackEntity Track
    {
        get
        {
            if (Track1 != null) return Track1;
            else if (Track2 != null) return Track2;
            else throw new TrackException("There is no `item` or `track` key in json");
        }
    }
    [JsonProperty(PropertyName = "item")]
    private TrackEntity Track1 { get; set; }
    [JsonProperty(PropertyName = "track")]
    private TrackEntity Track2 { get; set; }

    public VideoThumbnail? video_thumbnail { get; set; }
}
