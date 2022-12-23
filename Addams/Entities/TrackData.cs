using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addams.Entities
{
    /// <summary>
    /// Track dataa entity fetch from spotify api when request /track/{id}/
    /// to deserialize: TrackData myDeserializedClass = JsonConvert.DeserializeObject<TrackData>(myJsonResponse);
    /// </summary>
    public class TrackData
    {
        public Album album { get; set; } = new Album();
        public List<Artist> artists { get; set; } = new List<Artist>();
        public List<string>? available_markets { get; set; }
        public int disc_number { get; set; }
        public int duration_ms { get; set; }
        public bool @explicit { get; set; }
        public ExternalIds? external_ids { get; set; }
        public ExternalUrls? external_urls { get; set; }
        public string? href { get; set; }
        public string id { get; set; } = string.Empty;
        public bool is_local { get; set; }
        public string name { get; set; } = string.Empty;
        public int? popularity { get; set; }
        public string? preview_url { get; set; }
        public int? track_number { get; set; }
        public string? type { get; set; }
        public string? uri { get; set; }
    }
}
