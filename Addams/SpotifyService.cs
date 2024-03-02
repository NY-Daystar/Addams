using Addams.Entities;
using Addams.Exceptions;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Addams;

internal class SpotifyService
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    /// <summary>
    /// Spotify Api requests
    /// </summary>
    private readonly SpotifyConfig config;

    /// <summary>
    /// Spotify Api requests
    /// </summary>
    private readonly SpotifyApi api;

    /// <summary>
    /// Name of playlist of liked songs
    /// </summary>
    const string LIKED_SONGS = "Liked Songs";

    /// <summary>
    /// Setup SpotifyService to get playlist and track
    /// Based on configuration file (new or existing)
    /// Then create Api object with config setup
    /// </summary>
    public SpotifyService()
    {
        Logger.Debug("Setup config...");
        config = SpotifyConfig.Get();
        Logger.Debug("Setup Api...");
        api = new SpotifyApi(config);
    }

    /// <summary>
    /// Verify is service is functionnal requesting API and test result
    /// </summary>
    /// <returns>if result valid returns true, otherwise false</returns>
    public async Task<bool> IsTokenValidAsync()
    {
        // On first launch no token
        if (config.Token == null)
            return false;

        // If token has expired
        if (config.Token.ExpiredDate < DateTime.UtcNow)
        {
            Logger.Warn($"Token has expired, expiration date : {config.Token.ExpiredDate}");
            return false;
        }

        Logger.Info($"Token is still valid, the expiration date is : {config.Token.ExpiredDate}");
        try
        {
            await GetPlaylistsNameAsync();
            return true;
        }
        catch (SpotifyException ex)
        {
            Logger.Debug($"Exception caught to test service {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Setup authorization to Spotify Application to get Oauth2 Token
    /// update configuration and save it
    /// Update client and Api to request directly instead of restart app
    /// </summary>
    /// <returns>Token string</returns>
    public async Task RefreshTokenAsync()
    {
        config.Token = await api.AuthorizeAsync();

        config.Save();

        api.RefreshClient(config.Token.Value);
    }

    /// <summary>
    /// Fetch name of the playlists
    /// </summary>
    /// <returns>List of playlist names</returns>
    public async Task<IEnumerable<Playlist>> GetPlaylistsNameAsync()
    {
        // Get playlist data
        Playlists playlistsData = await api.FetchPlaylistsAsync();

        List<Playlist> playlists = playlistsData.Items.ToList();
        return playlists.Prepend(new Playlist() { Name = LIKED_SONGS }).ToList();
    }

    /// <summary>
    /// Get playlist wanted by the user created or saved in Spotify
    /// </summary>
    /// <param name="playlistSelected">List of playlist wanted</param>
    /// <returns>Playlist's models</returns>
    public async Task<IEnumerable<Models.Playlist>> GetPlaylistsAsync(IEnumerable<Entities.Playlist> playlistSelected)
    {
        List<Models.Playlist> playlists = new();

        // Get playlist data
        Playlists playlistsData = await api.FetchPlaylistsAsync();

        if (playlistsData.Items == null)
        {
            Logger.Warn("No playlists found");
            return new List<Models.Playlist>();
        }

        List<Playlist> playlistToFetch = playlistsData.Items.Where(p => playlistSelected.Select(ps => ps.Name).Contains(p.Name)).ToList();

        Logger.Debug($"Playlist selected: {string.Join("; ", playlistSelected.Select(p => p.Name))}");

        // Liked song playlist if choosen or want all playlist
        if (playlistSelected.Select(p => p.Name).Contains(LIKED_SONGS))
        {
            Models.Playlist likedPlaylist = await GetLikedTracksAsync();
            playlists.Add(likedPlaylist);
        }

        // Get tracks for each playlist
        foreach (Playlist p in playlistToFetch)
        {
            Models.Playlist playlist = await GetPlaylistAsync(p);
            Logger.Info($"Playlist {playlist}");
            playlists.Add(playlist);
        }
        return playlists;
    }

    /// <summary>
    /// Get playlist from it's id
    /// </summary>
    /// <param name="playlist">Playlist entity fetched by /playlists request</param>
    /// <returns>Playlist model with tracklist</returns>
    /// <exception cref="SpotifyException"></exception>
    public async Task<Models.Playlist> GetPlaylistAsync(Playlist playlist)
    {
        if (playlist.Id == null)
        {
            throw new SpotifyException($"getPlaylistTracks id null of the playlist name : {playlist.Name}");
        }

        IEnumerable<Models.Track> tracks = await GetPlaylistTracksAsync(playlist.Id);

        if (tracks.ToList().Count == 0)
        {
            Logger.Warn($"No tracks found for the playlist {playlist.Name} - id: {playlist.Id}");
        }

        return new Models.Playlist
        {
            Id = playlist.Id,
            Name = playlist.Name,
            Description = playlist.Description,
            Href = playlist.Href,
            Tracks = tracks
        };
    }

    /// <summary>
    /// Get playlist tracks from it's id
    /// </summary>
    /// <param name="id">Id ot the playlist</param>
    /// <returns>List of track</returns>
    public async Task<IEnumerable<Models.Track>> GetPlaylistTracksAsync(string playlistId)
    {
        // Get playlist data
        PlaylistTracks? playlistTracks = await api.FetchTracksAsync(playlistId);

        if (playlistTracks == null
            || playlistTracks.Tracks == null
            || playlistTracks.Tracks.Items == null
            )
        {
            return new List<Models.Track>();
        }

        // Get tracks data for each track
        List<Models.Track> tracks = new();
        foreach (TrackItem ti in playlistTracks.Tracks.Items)
        {
            if (ti.track == null)
            {
                continue;
            }

            Models.Track track = GetTrack(ti);

            tracks.Add(track);
        }

        return tracks;
    }

    /// <summary>
    /// Create track object base on trackData fetch from spotify API
    /// </summary>
    /// <param name="trackEntity">track data from playlist call</param>
    /// <returns>Track model object</returns>
    public static Models.Track GetTrack(TrackItem trackEntity)
    {
        Track track = trackEntity.track;
        if (track.Id == null)
        {
            Logger.Warn($"GetTrackData id null of the track name: {track.Name}");
            return new Models.Track
            {
                Name = track.Name,
                Artists = string.Join(",", track.Artists.Select(x => x.Name))
            };
        }

        return new Models.Track
        {
            Id = track.Id,
            Name = track.Name,
            Artists = string.Join("|", track.Artists.Select(x => x.Name)),
            AlbumName = track.Album.name,
            AlbumArtistName = string.Join("|", track.Album.artists.Select(x => x.Name)),
            AlbumReleaseDate = track.Album.release_date,
            DiscNumber = track.DiscNumber,
            TrackNumber = track.TrackNumber,
            Popularity = track.Popularity,
            AddedAt = trackEntity.added_at.ToString() ?? "",
            AlbumImageUrl = track.Album.images.First().url ?? "",
            TrackPreviewUrl = track.PreviewUrl,
            TrackUri = track.Uri,
            ArtistUrl = track.Artists.First().Uri ?? "",
            AlbumUrl = track.Album.uri ?? "",
            Explicit = track.Explicit,
            IsLocal = track.IsLocal,
            Duration = track.DurationMs,
        };
    }

    /// <summary>
    /// Fetch all the tracks in liked songs on Spotify
    /// </summary>
    /// <returns>Playlist model with liked songs</returns>
    public async Task<Models.Playlist> GetLikedTracksAsync()
    {
        TrackList likedPlaylistEnt = await api.FetchLikedTracksAsync();

        if (likedPlaylistEnt == null
           || likedPlaylistEnt.Items == null
           || likedPlaylistEnt.Href == null
           )
        {
            return new Models.Playlist();
        }

        // Get tracks data for each track
        List<Models.Track> tracks = new();
        foreach (TrackItem ti in likedPlaylistEnt.Items)
        {
            if (ti.track == null)
            {
                continue;
            }

            Models.Track track = GetTrack(ti);

            tracks.Add(track);
        }

        string href = likedPlaylistEnt.Href[..likedPlaylistEnt.Href.IndexOf("?")];

        return new Models.Playlist()
        {
            Name = "0 - Liked Songs",
            Description = "Playlist of liked songs",
            Href = href,
            Tracks = tracks,
        };
    }
}
