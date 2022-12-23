using System.Collections.Generic;

namespace Addams.Models
{
    /// <summary>
    /// TODO to comment
    ///   To use: Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    /// </summary>
    public class Playlist
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public string Href { get; set; }

        public List<Track> Tracks { get; set; }


        public Playlist(int id, string Name, string description, string href, List<Track> Tracks)
        {
            this.Id = id;
            this.Name = Name;
            this.Description = description;
            this.Href = href;
            this.Tracks = Tracks;
        }
    }
}