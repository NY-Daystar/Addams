using System.Collections.Generic;

namespace Addams.Entities
{
    /// <summary>
    /// Playlist's tracks entity fetch from spotify api when request /playlist/{id}/
    /// to deserialize: PlaylistTracks myDeserializedClass = JsonConvert.DeserializeObject<PlaylistTracks>(myJsonResponse);
    /// </summary>
    public class PlaylistTracks
    {
        public bool? collaborative { get; set; }
        public string? description { get; set; }
        public ExternalUrls? external_urls { get; set; }
        public Followers? followers { get; set; }
        public string? href { get; set; }
        public string? id { get; set; }
        public IEnumerable<Image>? images { get; set; }
        public string? name { get; set; }
        public Owner? owner { get; set; }
        public object? primary_color { get; set; }
        public bool? @public { get; set; }
        public string? snapshot_id { get; set; }
        public TrackList tracks { get; set; } = new TrackList();
        public string? type { get; set; }
        public string? uri { get; set; }
    }

    public class AddedBy
    {
        public ExternalUrls? external_urls { get; set; }
        public string? href { get; set; }
        public string? id { get; set; }
        public string? type { get; set; }
        public string? uri { get; set; }
    }

    public class Followers
    {
        public object? href { get; set; }
        public int? total { get; set; }
    }

    public class VideoThumbnail
    {
        public object? url { get; set; }
    }
}