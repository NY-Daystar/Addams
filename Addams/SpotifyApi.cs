using Addams.Entities;
using Addams.Exceptions;
using Newtonsoft.Json;
using NLog;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Addams;

/// <summary>
/// Spotify API requests
/// </summary>
public class SpotifyApi
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

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
    private const string API = @"https://api.spotify.com/v1";

    /// <summary>
    /// User to do the request on playlist
    /// </summary>
    private readonly string User;

    /// <summary>
    /// Id of spotify app
    /// </summary>
    private readonly string ClientID;

    /// <summary>
    /// Secret of spotify app
    /// </summary>
    private readonly string ClientSecret;

    /// <summary>
    /// OAuth2 token to get info on spotify API for a specific user
    /// </summary>
    public string AuthToken { private get; set; }

    /// <summary>
    /// Authenticated client with OAuth Token
    /// </summary>
    private readonly HttpClient Client;

    /// <summary>
    /// Setup Spotify API
    /// </summary>
    /// <param name="user">User to get infos</param>
    /// <param name="authToken">Authentication token to get access to the data</param>
    public SpotifyApi(SpotifyConfig cfg)
    {
        User = cfg.User;
        ClientID = cfg.ClientID;
        ClientSecret = cfg.ClientSecret;
        AuthToken = cfg.Token;
        Client = GetAuthClient();
    }

    /// <summary>
    /// Setup http client with authorization token (oauth2) for spotify api
    /// </summary>
    /// <returns>HttpClient with token authentication value</returns>
    /// 
    private HttpClient GetAuthClient()
    {
        HttpClient client = new();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthToken);
        return client;
    }

    // TODO feature OAUTH2 to comment
    public void RefreshClient(string token)
    {
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    //TODO feature OAUTH2 to comment
    public async Task<Token> AuthorizeAsync()
    {
        FormUrlEncodedContent requestData = new(new[]
        {
            new KeyValuePair<string, string>("client_id", ClientID),
            new KeyValuePair<string, string>("client_secret", ClientSecret),
            // TODO : new KeyValuePair<string, string>("scope","playlist-read-private user-library-read ugc-image-upload user-read-playback-state user-modify-playback-state user-read-currently-playing app-remote-control playlist-read-private playlist-read-collaborative playlist-modify-private playlist-modify-public user-follow-modify user-follow-read user-read-playback-position user-top-read user-read-recently-played user-library-modify user-library-read user-read-email user-read-private"),
            new KeyValuePair<string, string>("scope","playlist-read-private user-library-read"),
            new KeyValuePair<string, string>("grant_type", "authorization_code")
        });

        string url = "https://accounts.spotify.com/api/token";
        HttpResponseMessage response = await new HttpClient().PostAsync(url, requestData);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            // TODO feature OAUTH2: Change error message
            throw new SpotifyUnauthorizedException($"Can't get Authorize\nThe token {AuthToken}\nis invalid for user: {User}\n" +
                "You need to create a new one or refresh it");
        }
        if (!response.IsSuccessStatusCode)
        {
            // TODO feature OAUTH2: Change error message
            throw new SpotifyException($"Can't get Authorize\nStatusCode {response.StatusCode} : {response.Content}");
        }
        string content = await response.Content.ReadAsStringAsync();
        Token token = JsonConvert.DeserializeObject<Token>(content) ?? new Token();
        return token;
    }

    /// <summary>
    /// Fetch all playlists of a user
    /// </summary>
    /// <returns>Playlist data return by spotify api /playlists</returns>
    /// <exception cref="SpotifyUnauthorizedException"></exception>
    /// <exception cref="SpotifyException"></exception>
    public async Task<Playlists> FetchPlaylistsAsync()
    {
        string url = $@"{API}/users/{User}/playlists?limit={PLAYLIST_LIMIT}&offset=0";
        Logger.Debug($"FetchUserPlaylists call API: {url}");

        HttpResponseMessage response = await Client.GetAsync(url);
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new SpotifyUnauthorizedException($"Can't get FetchPlaylists\nThe token {AuthToken}\nis invalid for user: {User}\n" +
                "You need to create a new one or refresh it");
        }
        if (!response.IsSuccessStatusCode)
        {
            throw new SpotifyException($"Can't get FetchPlaylists\nStatusCode {response.StatusCode} : {response.Content}");
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
        string url = $@"{API}/playlists/{id}";
        Logger.Debug($"FetchPlaylistTracks call API: {url}");

        HttpResponseMessage response = await Client.GetAsync(url);
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new SpotifyUnauthorizedException($"Can't get FetchTracks\nThe token {AuthToken}\nis invalid for user: {User}\n" +
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
        if (playlist.tracks.total >= TRACK_LIMIT)
        {
            Logger.Warn($"For {playlist.name} : only fetch {playlist.tracks.items.ToList().Count} tracks for a total to {playlist.tracks.total}");
            playlist.tracks = await FetchTracksOverflowAsync(playlist.tracks);
            Logger.Info($"Fetch all tracks for the playlist {playlist.tracks.items.ToList().Count}/{playlist.tracks.total}");
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
        string url = $@"{API}/users/{User}/tracks?limit={TRACK_LIKED_LIMIT}&offset=0";
        Logger.Debug($"FetchLikedTracks call API: {url}");

        HttpResponseMessage response = await Client.GetAsync(url);
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new SpotifyUnauthorizedException($"Can't get FetchLikedTracks\nThe token {AuthToken}\nis invalid for user: {User}\n" +
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
        if (playlist.total >= TRACK_LIMIT)
        {
            Logger.Warn($"For Liked Song : only fetch {playlist.items.ToList().Count} tracks for a total to {playlist.total}");
            playlist = await FetchTracksOverflowAsync(playlist);
            Logger.Info($"Fetch all tracks for the playlist {playlist.items.ToList().Count}/{playlist.total}");
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
    public async Task<Track> FetchTrackAsync(string id)
    {
        string url = $@"{API}/tracks/{id}";
        Logger.Debug($"FetchTrack call API: {url}");

        HttpResponseMessage response = await Client.GetAsync(url);
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new SpotifyUnauthorizedException($"Can't get FetchTrack\nThe token {AuthToken}\nis invalid for user: {User}\n" +
                "You need to create a new one or refresh it");
        }
        if (!response.IsSuccessStatusCode)
        {
            throw new SpotifyException($"Can't get FetchTrack\nStatusCode {response.StatusCode} : {response.Content}");
        }
        string content = await response.Content.ReadAsStringAsync();
        Track track = JsonConvert.DeserializeObject<Track>(content) ?? new Track();
        return track;
    }

    /// <summary>
    /// Add in playlist overflow tracks from specific API url
    /// </summary>
    /// <param name="url">url to request to fetch tracks</param>
    /// <returns>Tracklist overflowed</returns>
    /// <exception cref="SpotifyUnauthorizedException"></exception>
    /// <exception cref="SpotifyException"></exception>
    private async Task<TrackList> FetchTracksOverflowAsync(TrackList playlist)
    {
        // Get all tracks of specific playlist
        do
        {
            if (playlist.next == null)
            {
                Logger.Warn("Playlist next url is null, can't get the rest of playlist");
                break;
            }

            // Get part of the playlist from url
            TrackList trackList = await FetchTrackListFromUrlAsync(playlist.next);

            // Add tracks into original playlist
            playlist.items.ToList().AddRange(trackList.items);
            playlist.next = trackList.next;
            Logger.Info($"Get the rest of playlist {playlist.items.ToList().Count}/{playlist.total} songs");

        } while (playlist.next != null);

        return playlist;
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
        Logger.Debug($"FetchTrackListFromUrl call API: {url}");

        HttpResponseMessage response = await Client.GetAsync(url);
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new SpotifyUnauthorizedException($"Can't get FetchTrackListFromUrl\nThe token {AuthToken}\nis invalid for user: {User}\n" +
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
