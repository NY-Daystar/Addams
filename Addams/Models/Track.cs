using System;

namespace Addams.Models
{
    public class Track
    {

        public string Id { get; set; } = String.Empty;
        public string Name { get; set; } = String.Empty;
        public string Artists { get; set; } = String.Empty;
        public string AlbumName { get; set; } = String.Empty;
        public bool Explicit { get; set; }
        public bool IsLocal { get; set; }
        public int Duration { get; set; }


         public override string ToString() => $"Name : {Name} - Artist : {Artists} - Id : {Id}";
    }
}
