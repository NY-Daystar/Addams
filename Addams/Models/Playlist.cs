using System.Collections.Generic;

namespace Addams.Models
{
    /// <summary>
    /// Playlist model to store tracks to save into csv 
    /// </summary>
    public class Playlist
    {
        public string Id { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public string Href { get; set; } = string.Empty;

        public List<Track> Tracks { get; set; } = new List<Track>();

        public Playlist() { }

        public Playlist(string id, string Name, string description, string href, List<Track> Tracks)
        {
            Id = id;
            this.Name = Name;
            Description = description;
            Href = href;
            this.Tracks = Tracks;
        }

        public override string ToString()
        {
            return $"Name : {Name} - Tracks number : {Tracks.Count} - Id : {Id}";
        }
    }
}