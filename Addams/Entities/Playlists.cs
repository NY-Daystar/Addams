using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addams.Entities
{
    /// <summary>
    /// TODO to comment
    /// Liste des playlists
    /// </summary>
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Playlists
    {
        public string? href { get; set; }
        public List<Playlist>? items { get; set; }
        public int? limit { get; set; }
        public string? next { get; set; }
        public int? offset { get; set; }
        public object? previous { get; set; }
        public int? total { get; set; }
    }

    public class Playlist
    {
        public bool? collaborative { get; set; }
        public string? description { get; set; }
        public ExternalUrls? external_urls { get; set; }
        public string? href { get; set; }
        public string? id { get; set; }
        public List<Image>? images { get; set; }
        public string? name { get; set; }
        public Owner? owner { get; set; }
        public object? primary_color { get; set; }
        public bool? @public { get; set; }
        public string? snapshot_id { get; set; }
        public Tracks? tracks { get; set; }
        public string? type { get; set; }
        public string? uri { get; set; }
    }
}

