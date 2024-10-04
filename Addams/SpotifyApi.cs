using Addams.Entities;
using Addams.Exceptions;
using Addams.Models;
using IdentityModel.Client;
using Newtonsoft.Json;
using NLog;
using System;
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
    private const string API = "https://api.spotify.com/v1";

    /// <summary>
    /// Configuration for Spotify
    /// </summary>
    private SpotifyConfig Config { get; }

    /// <summary>
    /// Authenticated client with OAuth Token
    /// </summary>
    private readonly HttpClient Client;

    /// <summary>
    /// Setup Spotify API
    /// </summary>
    /// <param name="config">Configuration of the application including access token</param>
    public SpotifyApi(SpotifyConfig config)
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
    /// Generate Oauth2 token from Spotify
    /// </summary>
    /// <returns>Token</returns>
    public async Task<TokenModel> AuthorizeAsync()
    {
        // Step 1 : Redirect the user to spotify to authorize application
        var url = $"{SpotifyConfig.AuthorityUri}/authorize?" +
            $"client_id={Config.ClientID}" +
            $"&response_type={SpotifyConfig.ResponseType}" +
            $"&redirect_uri={Uri.EscapeDataString(SpotifyConfig.RedirectUri)}" +
            $"&scope={Uri.EscapeDataString(SpotifyConfig.Scope)}";

        Console.WriteLine("\n\nAuthorize application, visiting this URL :");
        Console.WriteLine($"{url}\n");

        // Step 2 : Get Authorization Code
        var authorizationCode = await FetchAuthorizationCodeAsync();

        // Step 3 : Call Spotify Api to exchange `Authorization Code` for an `Access Token`
        var tokenEntity = await FetchTokenApiAsync(authorizationCode);

        // Convert Token from entity to model
        TokenModel token = JsonConvert.DeserializeObject<TokenModel>(JsonConvert.SerializeObject(tokenEntity)) ?? new TokenModel();
        token.CalculateExpiration();
        if (string.IsNullOrEmpty(token.Value))
        {
            Logger.Error("Error when exchange authorizing code for a token");
        }

        Console.WriteLine($"\n\nToken d'accès : {token}\n\n");

        return token;
    }

    /// <summary>
    /// Create Http server to listen and get authorization code
    /// Then authorization got call spotify Api to get access token
    /// </summary>
    /// <returns>Token Oauth2</returns>
    static async Task<string> FetchAuthorizationCodeAsync()
    {
        var host = $"{SpotifyConfig.Hostname}:{SpotifyConfig.Port}/";

        // Create and start http server
        var listener = new HttpListener();
        listener.Prefixes.Add(host);
        listener.Start();

        Logger.Debug($"Server listen on : {host}");

        while (true)
        {
            var context = await listener.GetContextAsync();
            var request = context.Request;
            if (request.Url?.AbsolutePath == "/callback")
            {
                Logger.Debug("Request /callback found");
                var authorizationCode = request.QueryString.Get("code");
                return authorizationCode ?? string.Empty;
            }
        }
    }

    /// <summary>
    /// Send Http request to spotify with authorization code to get Token
    /// </summary>
    /// <param name="authorizationCode"></param>
    /// <returns>Token object</returns>
    async Task<Token> FetchTokenApiAsync(string authorizationCode)
    {
        var httpClient = new HttpClient();
        var url = $"{SpotifyConfig.AuthorityUri}/api/token";

        var response = await httpClient.RequestAuthorizationCodeTokenAsync(new AuthorizationCodeTokenRequest
        {
            Address = url,
            ClientId = Config.ClientID,
            ClientSecret = Config.ClientSecret,
            Code = authorizationCode ?? string.Empty,
            RedirectUri = SpotifyConfig.RedirectUri
        });
        string content = response.Raw ?? string.Empty;
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
        string url = $"{API}/users/{Config.UserName}/playlists?limit={PLAYLIST_LIMIT}&offset=0";
        Logger.Debug($"FetchUserPlaylists call API: {url}");

        HttpResponseMessage response = await Client.GetAsync(url);
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new SpotifyUnauthorizedException($"Can't get FetchPlaylists\nThe token {Config.Token?.Value}\nis invalid for user: {Config.UserName}\n" +
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
        string url = $"{API}/playlists/{id}";
        Logger.Debug($"FetchPlaylistTracks call API: {url}");

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
            Logger.Warn($"For {playlist.Name} : only fetch {playlist.Tracks.Items.ToList().Count} tracks for a total to {playlist.Tracks.Total}");
            playlist.Tracks.Items.AddRange(await FetchTracksOverflowAsync(playlist.Tracks));
            Logger.Info($"Fetch all tracks for the playlist {playlist.Tracks.Items.ToList().Count}/{playlist.Tracks.Total}");
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
        string url = $"{API}/users/{Config.UserName}/tracks?limit={TRACK_LIKED_LIMIT}&offset=0";
        Logger.Debug($"FetchLikedTracks call API: {url}");

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
        if (playlist.Total >= TRACK_LIMIT)
        {
            Logger.Warn($"For Liked Song : only fetch {playlist.Items.ToList().Count} tracks for a total to {playlist.Total}");
            playlist.Items.AddRange(await FetchTracksOverflowAsync(playlist));
            Logger.Info($"Fetch all tracks for the playlist {playlist.Items.ToList().Count}/{playlist.Total}");
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
    public async Task<Entities.Track> FetchTrackAsync(string id)
    {
        string url = $"{API}/tracks/{id}";
        Logger.Debug($"FetchTrack call API: {url}");

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
        Entities.Track track = JsonConvert.DeserializeObject<Entities.Track>(content) ?? new Entities.Track();
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
        List<TrackItem> allTracks = new();
        do
        {
            if (playlist.Next == null)
            {
                Logger.Warn("Playlist next url is null, can't get the rest of playlist");
                break;
            }

            // Get part of the playlist from url
            TrackList trackList = await FetchTrackListFromUrlAsync(playlist.Next);

            // Add tracks into original playlist
            allTracks.AddRange(trackList.Items);
            playlist.Next = trackList.Next;
            Logger.Info($"Get the rest of playlist {playlist.Items.ToList().Count}/{playlist.Total} songs");
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
        Logger.Debug($"FetchTrackListFromUrl call API: {url}");

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
