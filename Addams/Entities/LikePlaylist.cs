using System;
using System.Collections.Generic;

namespace Addams.Entities
{
    /// <summary>
    /// Like song entity fetch from spotify api when request /tracks
    /// to deserialize: LikePlaylist myDeserializedClass = JsonConvert.DeserializeObject<LikePlaylist>(myJsonResponse);
    /// </summary>
    public class LikePlaylist
    {
        public string? href { get; set; }
        public List<LikeTrack>? items { get; set; }
        public int limit { get; set; }
        public string? next { get; set; }
        public int offset { get; set; }
        public object? previous { get; set; }
        public int total { get; set; }
    }

    public class LikeTrack
    {
        public DateTime added_at { get; set; }

        public Track? track { get; set; }
    }
}

