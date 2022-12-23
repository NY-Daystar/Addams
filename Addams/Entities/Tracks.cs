using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addams.Entities
{
    public class Tracks
    {
        public string? href { get; set; }
        public List<TrackItem>? items { get; set; }
        public int? limit { get; set; }
        public string? next { get; set; }
        public int? offset { get; set; }
        public object? previous { get; set; }
        public int? total { get; set; }
    }
}
