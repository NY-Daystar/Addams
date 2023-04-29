using System.Collections.Generic;

namespace Addams.Entities
{
    /// <summary>
    /// Album data from spotify
    /// </summary>
    public class Album
    {
        public string? album_type { get; set; }
        public IEnumerable<Artist> artists { get; set; } = new List<Artist>();
        public IEnumerable<string>? available_markets { get; set; }
        public ExternalUrls? external_urls { get; set; }
        public string? href { get; set; }
        public string? id { get; set; }
        public IEnumerable<Image> images { get; set; } = new List<Image>();
        public string name { get; set; } = string.Empty;
        public string release_date { get; set; } = string.Empty;
        public string? release_date_precision { get; set; }
        public int? total_tracks { get; set; }
        public string? type { get; set; }
        public string? uri { get; set; }
    }
}
