using System;

namespace Addams.Entities;

public class TrackItem
{
    public DateTime? added_at { get; set; }
    public AddedBy? added_by { get; set; }
    public bool? is_local { get; set; }
    public object? primary_color { get; set; }
    public Track track { get; set; } = new Track();
    public VideoThumbnail? video_thumbnail { get; set; }
}
