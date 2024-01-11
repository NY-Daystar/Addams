using System.Collections.Generic;

namespace Addams.Entities;

public class TrackList
{
    public string? href { get; set; }
    public IEnumerable<TrackItem> items { get; set; } = new List<TrackItem>();
    public int? limit { get; set; }
    public string? next { get; set; }
    public int? offset { get; set; }
    public object? previous { get; set; }
    public int? total { get; set; }
}
