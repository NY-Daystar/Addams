using System.Collections.Generic;

namespace Addams.Entities
{
    /// <summary>
    /// Playlist entity fetch from spotify api when request /playlists/
    /// to deserialize: Playlists myDeserializedClass = JsonConvert.DeserializeObject<Playlists>(myJsonResponse);
    /// </summary>
    public class Playlists
    {
        public string? href { get; set; }
        public List<Playlist> items { get; set; } = new List<Playlist>();
        public int? limit { get; set; }
        public string? next { get; set; }
        public int? offset { get; set; }
        public object? previous { get; set; }
        public int? total { get; set; }
    }

    public class Playlist
    {
        public bool? collaborative { get; set; }
        public string description { get; set; } = string.Empty;
        public ExternalUrls? external_urls { get; set; }
        public string href { get; set; } = string.Empty;
        public string id { get; set; } = string.Empty;
        public List<Image>? images { get; set; }
        public string name { get; set; } = string.Empty;
        public Owner? owner { get; set; }
        public object? primary_color { get; set; }
        public bool? @public { get; set; }
        public string? snapshot_id { get; set; }
        public TrackList? tracks { get; set; }
        public string? type { get; set; }
        public string? uri { get; set; }
    }
}

