using Addams.Utils;
using System;

namespace Addams.Models
{
    /// <summary>
    /// Track model to store track data into csv 
    /// </summary>
    public class Track
    {
        /// <summary>
        /// Spotify track id
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Name of the track
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// List of artist(separated by `|`)
        /// </summary>
        public string Artists { get; set; } = string.Empty;

        /// <summary>
        /// Name of the album
        /// </summary>
        public string AlbumName { get; set; } = string.Empty;

        /// <summary>
        /// Album's artists (separated by '|')
        /// </summary>
        public string AlbumArtistName { get; set; } = string.Empty;

        /// <summary>
        /// Release date of the album('YYYY-MM-DD')
        /// </summary>
        public string AlbumReleaseDate { get; set; } = string.Empty;

        /// <summary>
        /// If album has multiple disc
        /// </summary>
        public int DiscNumber { get; set; }

        /// <summary>
        /// Number of the track in the album
        /// </summary>
        public int TrackNumber { get; set; }

        public bool IsLocal { get; set; }

        public int _duration { get; set; }

        /// <summary>
        /// Time duration of the track ('minutes:secondes')
        /// </summary>
        /// TODO small-change: To check
        public string Duration => FormatDuration(_duration);

        /// <summary>
        /// Number in range 0-100 for unpopular to very popular
        /// </summary>
        public int Popularity { get; set; }

        /// <summary>
        /// Datetime when you add this track in your playlist
        /// </summary>
        public string AddedAt { get; set; } = string.Empty;

        /// <summary>
        /// Spotify url of the track
        /// </summary>
        public string TrackUri { get; set; } = string.Empty;

        /// <summary>
        /// Spotify url of the artist
        /// </summary>
        public string ArtistUrl { get; set; } = string.Empty;

        /// <summary>
        /// Spotify url of the album
        /// </summary>
        public string AlbumUrl { get; set; } = string.Empty;

        /// <summary>
        /// If track is explicit or not('True or False')
        /// </summary>
        public bool Explicit { get; set; }

        /// <summary>
        /// Url image of the album
        /// </summary>
        public string AlbumImageUrl { get; set; } = string.Empty;

        /// <summary>
        /// Url track preview of the album (30sec audio)
        /// </summary>
        public string TrackPreviewUrl { get; set; } = string.Empty;

        /// <summary>
        /// Show several informations of a track
        /// </summary>
        /// <returns>Name, artist and the id of the track</returns>
        public override string ToString()
        {
            return $"Name : {Name} - Artist : {Artists} - Id : {Id}";
        }

        /// <summary>
        /// Format milliseconds value to track duration standard
        /// </summary>
        /// <param name="duration">ms value</param>
        /// <returns>time formatted (HH:MM:SS)</returns>
        public string FormatDuration(int duration)
        {
            TimeSpan ts = TimeConverter.ConvertMsToTimeSpan(duration);
            return TimeConverter.FormatTimeSpan(ts);
        }
    }
}
