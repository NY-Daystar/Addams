using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addams.Entities
{
    // TODO to comment

    public class PlaylistTracks
    {
        public bool collaborative { get; set; }
        public string description { get; set; } = String.Empty;
        public ExternalUrls external_urls { get; set; }= new ExternalUrls();
        public Followers followers { get; set; } = new Followers(); 
        public string href { get; set; } = String.Empty;
        public string id { get; set; } = String.Empty;
        public List<Image>? images { get; set; } = new List<Image>();
        public string name { get; set; } = String.Empty;
        public Owner owner { get; set; } = new Owner();
        public object primary_color { get; set; } = String.Empty;
        public bool @public { get; set; }
        public string snapshot_id { get; set; } = String.Empty;
        public Tracks tracks { get; set; } = new Tracks();
        public string type { get; set; } = String.Empty;
        public string uri { get; set; } = String.Empty;
    }
    public class TrackItem
    {
        public DateTime added_at { get; set; } = new DateTime();
        public AddedBy added_by { get; set; } = new AddedBy();
        public bool is_local { get; set; }
        public object primary_color { get; set; } = String.Empty;
        public Track track { get; set; } = new Track();
        public VideoThumbnail video_thumbnail { get; set; } = new VideoThumbnail();
    }


    public class AddedBy
    {
        public ExternalUrls external_urls { get; set; } = new ExternalUrls();
        public string href { get; set; } = String.Empty;
        public string id { get; set; } = String.Empty;
        public string type { get; set; } = String.Empty;
        public string uri { get; set; } = String.Empty;
    }

    public class Album
    {
        public string? album_type { get; set; }
        public List<Artist>? artists { get; set; }
        public List<string>? available_markets { get; set; }
        public ExternalUrls? external_urls { get; set; }
        public string? href { get; set; } 
        public string? id { get; set; }
        public List<Image>? images { get; set; }
        public string? name { get; set; } 
        public string? release_date { get; set; } 
        public string? release_date_precision { get; set; } 
        public int? total_tracks { get; set; }
        public string? type { get; set; }
        public string? uri { get; set; }
    }

    public class Artist
    {
        public ExternalUrls external_urls { get; set; } = new ExternalUrls();
        public string href { get; set; } = String.Empty;
        public string id { get; set; } = String.Empty;
        public string name { get; set; } = String.Empty;
        public string type { get; set; } = String.Empty;
        public string uri { get; set; } = String.Empty;
    }

    public class ExternalIds
    {
        public string isrc { get; set; } = String.Empty;
    }

    public class Followers
    {
        public object href { get; set; } = String.Empty;
        public int total { get; set; }
    }

    public class Track
    {
        public Album album { get; set; } = new Album();
        public List<Artist> artists { get; set; } = new List<Artist>();
        public List<string> available_markets { get; set; } = new List<string>();
        public int disc_number { get; set; }
        public int duration_ms { get; set; }
        public bool episode { get; set; }
        public bool @explicit { get; set; }
        public ExternalIds external_ids { get; set; } = new ExternalIds();
        public ExternalUrls external_urls { get; set; } = new ExternalUrls();
        public string href { get; set; } = String.Empty;
        public string id { get; set; } = String.Empty;
        public bool is_local { get; set; }
        public string name { get; set; } = String.Empty;
        public int popularity { get; set; }
        public string preview_url { get; set; } = String.Empty;
        public bool track { get; set; }
        public int track_number { get; set; }
        public string type { get; set; } = String.Empty;
        public string uri { get; set; } = String.Empty;
    }


    public class VideoThumbnail
    {
        public object url { get; set; } = String.Empty;
    }
}