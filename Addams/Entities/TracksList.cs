using System;
using System.Collections.Generic;

namespace Addams.Entities
{
    public class TrackList
    {
        public string? href { get; set; }
        public List<TrackItem> items { get; set; } = new List<TrackItem>();
        public int? limit { get; set; }
        public string next { get; set; } = String.Empty;
        public int? offset { get; set; }
        public object? previous { get; set; }
        public int? total { get; set; }
    }
}
