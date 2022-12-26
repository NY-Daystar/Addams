using Addams.Entities;
using System;
using System.Reflection.Metadata.Ecma335;

namespace Addams.Models
{
    public class Track
    {
        public string Id { get; set; } = String.Empty;
        public string Name { get; set; } = String.Empty;
        public string Artists { get; set; } = String.Empty;
        public string AlbumName { get; set; } = String.Empty;
        public string AlbumArtistName { get; set; } = String.Empty;
        public string AlbumReleaseDate { get; set; } = String.Empty;
        public int DiscNumber { get; set; }
        public int TrackNumber { get; set; }
        public int Popularity { get; set; }
        public string AddedAt { get; set; } = String.Empty;
        public string AlbumImageUrl { get; set; } = String.Empty;
        public string TrackPreviewUrl { get; set; } = String.Empty;
        public string TrackUri { get; set; } = String.Empty;
        public string ArtistUrl { get; set; } = String.Empty;
        public bool Explicit { get; set; }
        public bool IsLocal { get; set; }
        // TODO voir la duration pour la convertir en min/secondes
        public int Duration { get; set; }

        public override string ToString() => $"Name : {Name} - Artist : {Artists} - Id : {Id}";
    }
}
