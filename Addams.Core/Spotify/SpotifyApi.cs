using Addams.Core.Logs;
using Addams.Core.Entities;
using Addams.Core.Exceptions;
using Addams.Core.Utils;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;

namespace Addams.Core.Spotify;

/// <summary>
/// Spotify API requests
/// </summary>
public class SpotifyApi
{
    /// <summary>
    /// Number of playlists fetchable in spotify API
    /// </summary>
    private const int PLAYLIST_LIMIT = 50;

    /// <summary>
    /// Number of tracks liked fetchable in spotify API
    /// </summary>
    private const int TRACK_LIKED_LIMIT = 50;

    /// <summary>
    /// Number of tracks fetched in spotify API
    /// </summary>
    private const int TRACK_LIMIT = 100;

    /// <summary>
    /// Url endpoint for Spotify Api
    /// </summary>
    private const string API = "https://api.spotify.com/v1";

    /// <summary>
    /// Configuration for Spotify
    /// </summary>
    private AddamsConfig Config { get; }

    /// <summary>
    /// Authenticated client with OAuth Token
    /// </summary>
    private readonly HttpClient Client;

    /// <summary>
    /// Setup Spotify API
    /// </summary>
    /// <param name="config">Configuration of the application including access token</param>
    public SpotifyApi(AddamsConfig config)
    {
        Config = config;
        Client = GetAuthClient();
    }

    /// <summary>
    /// Setup http client with authorization token (oauth2) for spotify api
    /// </summary>
    /// <returns>HttpClient with token authentication value</returns>
    ///
    private HttpClient GetAuthClient()
    {
        if (Client != null)
            return Client;

        HttpClient client = new();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Config.Token?.Value);
        return client;
    }

    /// <summary>
    /// Set a new token for the client
    /// </summary>
    /// <param name="token">New Access token</param>
    public void RefreshClient(string token)
    {
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    /// <summary>
    /// Fetch all playlists of a user
    /// </summary>
    /// <returns>Playlist data return by spotify api /playlists</returns>
    /// <exception cref="SpotifyUnauthorizedException"></exception>
    /// <exception cref="SpotifyException"></exception>
    public async Task<Playlists> FetchPlaylistsAsync()
    {
        string url = $"{API}/me/playlists?limit={PLAYLIST_LIMIT}&offset=0";
        LoggerManager.Log($"FetchUserPlaylists call API: {url}", Level.Debug);

        HttpResponseMessage response = await Client.GetAsync(url);
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new SpotifyUnauthorizedException($"Can't get FetchPlaylists\n" +
                $"Token invalid for user: {Config.UserName}");
        }
        if (!response.IsSuccessStatusCode)
        {
             throw new SpotifyException($"Can't get FetchPlaylists\n" +
                $"StatusCode {response.StatusCode} : {response.Content}", response.Headers);
        }
        string content = await response.Content.ReadAsStringAsync();
        Playlists playlists = JsonConvert.DeserializeObject<Playlists>(content) ?? new Playlists();
        return playlists;
    }

    /// <summary>
    /// Fetch tracks on specific playlist from its id
    /// </summary>
    /// <param name="id">id of playlist</param>
    /// <returns>Tracks' playlist</returns>
    /// <exception cref="SpotifyUnauthorizedException"></exception>
    /// <exception cref="SpotifyException"></exception>
    public async Task<PlaylistTracks> FetchTracksAsync(string id)
    {
        string url = $"{API}/playlists/{id}";
        LoggerManager.Log($"FetchPlaylistTracks call API: {url}", Level.Debug);

        HttpResponseMessage response = await Client.GetAsync(url);
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new SpotifyUnauthorizedException($"Can't get FetchTracks\nThe token {Config.Token?.Value}\nis invalid for user: {Config.UserName}\n" +
                "You need to create a new one or refresh it");
        }
        if (!response.IsSuccessStatusCode)
        {
            throw new SpotifyException($"Can't get FetchTracks\nStatusCode {response.StatusCode} : {response.Content}");
        }
        string content = await response.Content.ReadAsStringAsync();
        PlaylistTracks playlist = JsonConvert.DeserializeObject<PlaylistTracks>(content) ?? new PlaylistTracks();

        // If can't catch every tracks for a playlist in one api call
        // Add all the tracks overflow by api on the playlist
        if (playlist.Tracks.Total >= TRACK_LIMIT)
        {
            playlist.Tracks.Items.AddRange(await FetchTracksOverflowAsync(playlist.Tracks));
            LoggerManager.Log(string.Format(Language.GetString("String21"), playlist.Tracks.Items.ToList().Count, playlist.Tracks.Total));
        }
        return playlist;
    }

    /// <summary>
    /// Fetch Liked songs of user
    /// </summary>
    /// <returns>Playlist of liked songs of a user return by spotify api /tracks</returns>
    /// <exception cref="SpotifyUnauthorizedException"></exception>
    /// <exception cref="SpotifyException"></exception>
    public async Task<TrackList> FetchLikedTracksAsync()
    {
        string url = $"{API}/me/tracks?limit={TRACK_LIKED_LIMIT}&offset=0";
        LoggerManager.Log($"FetchLikedTracks call API: {url}", Level.Debug);

        HttpResponseMessage response = await Client.GetAsync(url);
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new SpotifyUnauthorizedException($"Can't get FetchLikedTracks\nThe token {Config.Token?.Value}\nis invalid for user: {Config.UserName}\n" +
                "You need to create a new one or refresh it");
        }
        if (!response.IsSuccessStatusCode)
        {
            throw new SpotifyException($"Can't get FetchLikedTracks\nStatusCode {response.StatusCode} : {response.Content}");
        }
        string content = await response.Content.ReadAsStringAsync();
        TrackList playlist = JsonConvert.DeserializeObject<TrackList>(content) ?? new TrackList();

        // If can't catch every tracks for a playlist in one api call
        // Add all the tracks overflow by api on the playlist
        if (playlist.Total >= TRACK_LIKED_LIMIT)
        {
            playlist.Items.AddRange(await FetchTracksOverflowAsync(playlist));
            LoggerManager.Log(string.Format(Language.GetString("String21"), playlist.Items.ToList().Count, playlist.Total));
        }
        return playlist;
    }

    /// <summary>
    /// Fetch track data from its id
    /// </summary>
    /// <param name="id">id of track</param>
    /// <returns>Track data</returns>
    /// <exception cref="SpotifyUnauthorizedException"></exception>
    /// <exception cref="SpotifyException"></exception>
    public async Task<TrackEntity> FetchTrackAsync(string id)
    {
        string url = $"{API}/tracks/{id}";
        LoggerManager.Log($"FetchTrack call API: {url}", Level.Debug);

        HttpResponseMessage response = await Client.GetAsync(url);
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new SpotifyUnauthorizedException($"Can't get FetchTrack\nThe token {Config.Token?.Value}\nis invalid for user: {Config.UserName}\n" +
                "You need to create a new one or refresh it");
        }
        if (!response.IsSuccessStatusCode)
        {
            throw new SpotifyException($"Can't get FetchTrack\nStatusCode {response.StatusCode} : {response.Content}");
        }
        string content = await response.Content.ReadAsStringAsync();
        TrackEntity track = JsonConvert.DeserializeObject<TrackEntity>(content) ?? new TrackEntity();
        return track;
    }

    /// <summary>
    /// Add in playlist overflow tracks from specific API url
    /// </summary>
    /// <param name="url">url to request to fetch tracks</param>
    /// <returns>Tracklist overflowed</returns>
    /// <exception cref="SpotifyUnauthorizedException"></exception>
    /// <exception cref="SpotifyException"></exception>
    private async Task<List<TrackItem>> FetchTracksOverflowAsync(TrackList playlist)
    {
        List<TrackItem> allTracks = [];
        do
        {
            if (playlist.Next == null)
            {
                LoggerManager.Log(Language.GetString("String23"), Level.Error);
                break;
            }

            // Get part of the playlist from url
            TrackList trackList = await FetchTrackListFromUrlAsync(playlist.Next);

            // Add tracks into original playlist
            allTracks.AddRange(trackList.Items);
            playlist.Next = trackList.Next;
            LoggerManager.Log(string.Format(Language.GetString("String24"), allTracks.Count, playlist.Total));
        } while (playlist.Next != null);

        return allTracks;
    }

    /// <summary>
    /// Fetch overflow tracks from specific API url
    /// </summary>
    /// <param name="url">url to request to fetch tracks</param>
    /// <returns>Tracklist overflowed</returns>
    /// <exception cref="SpotifyUnauthorizedException"></exception>
    /// <exception cref="SpotifyException"></exception>
    private async Task<TrackList> FetchTrackListFromUrlAsync(string url)
    {
        LoggerManager.Log($"FetchTrackListFromUrl call API: {url}", Level.Debug);

        HttpResponseMessage response = await Client.GetAsync(url);
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new SpotifyUnauthorizedException($"Can't get FetchTrackListFromUrl\nThe token {Config.Token?.Value}\nis invalid for user: {Config.UserName}\n" +
                "You need to create a new one or refresh it");
        }
        if (!response.IsSuccessStatusCode)
        {
            throw new SpotifyException($"Can't get FetchTrackListFromUrl\nStatusCode {response.StatusCode} : {response.Content}");
        }
        string content = await response.Content.ReadAsStringAsync();
        TrackList trackList = JsonConvert.DeserializeObject<TrackList>(content) ?? new TrackList();

        return trackList;
    }
}
