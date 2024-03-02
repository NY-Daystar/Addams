using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Addams.Entities;

public class TrackList
{
    [JsonProperty(PropertyName = "href")]
    public string? Href { get; set; }

    [JsonProperty(PropertyName = "items")]
    public List<TrackItem> Items { get; set; } = new List<TrackItem>();

    [JsonProperty(PropertyName = "limit")]
    public int? Limit { get; set; }

    [JsonProperty(PropertyName = "next")]
    public string? Next { get; set; }

    [JsonProperty(PropertyName = "offset")]
    public int? Offset { get; set; }

    [JsonProperty(PropertyName = "previous")]
    public object? Previous { get; set; }

    [JsonProperty(PropertyName = "total")]
    public int? Total { get; set; }
}
