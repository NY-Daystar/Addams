using Addams.Core.Logs;
using Addams.Core.Entities;
using Addams.Core.Exceptions;
using Addams.Core.Models;
using Addams.Core.Utils;
using Duende.IdentityModel.Client;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;

namespace Addams.Core.Spotify;

internal class SpotifyService
{
    /// <summary>
    /// Spotify Api requests
    /// </summary>
    private readonly AddamsConfig Config;

    /// <summary>
    /// Spotify Api requests
    /// </summary>
    private readonly SpotifyApi Api;

    /// <summary>
    /// Name of playlist of liked songs
    /// </summary>
    const string LIKED_SONGS = "Liked Songs";

    private HttpListener _listener = new();

    /// <summary>
    /// Setup SpotifyService to get playlist and track
    /// Based on configuration file (new or existing)
    /// Then create Api object with config setup
    /// </summary>
    public SpotifyService()
    {
        Debug.WriteLine("Setup config...");
        Config = AddamsConfig.Get();
        Debug.WriteLine("Setup Api...");
        Api = new SpotifyApi(Config);
    }

    /// <summary>
    /// Verify is service is functionnal requesting API and test result
    /// </summary>
    /// <returns>if result valid returns true, otherwise false</returns>
    internal async Task<SpotifyAuthenticationStatus> IsTokenValidAsync(CancellationTokenSource cts)
    {
        if (Config.Token.ExpiredDate < DateTime.UtcNow || string.IsNullOrEmpty(Config.Token.Value))
        {
            Debug.WriteLine(string.Format(Language.GetString("String28"), Config.Token.ExpiredDate.ToLocalTime()));

            if (Config.Token is null || string.IsNullOrEmpty(Config.Token.Refresh))
                return new(false);

            return new(await RefreshAccessTokenAsync(cts));
        }
        Debug.WriteLine(string.Format(Language.GetString("String29"), Config.Token.ExpiredDate.ToLocalTime()));

        try
        {
            await GetPlaylistsNameAsync();
            return new(true);
        }
        catch (SpotifyUnauthorizedException ex)
        {
            return new(false) { Exception = ex };
        }
        catch (SpotifyException ex)
        {
            return new(false) { Exception = ex };
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"{ex.GetType().FullName} - {ex.Message} - {ex.StackTrace}");
            return new(false);
        }
    }

    /// <summary>
    /// Setup authorization to Spotify Application to get Oauth2 Token
    /// update configuration and save it
    /// if already done just an update with refresh token
    /// Update client and Api to request directly instead of restart app
    /// </summary>
    /// <returns>Token string</returns>
    public async Task<bool> RefreshAccessTokenAsync(CancellationTokenSource cts)
    {
        if (!string.IsNullOrEmpty(Config.Token?.Refresh))
            Config.Token = await RefreshAsync();

        Config.Save();
        Api.RefreshClient(Config.Token.Value);

        if (!string.IsNullOrEmpty(Config.Token.Value))
            return true;

        return false;
    }

    /// <summary>
    /// Generate Oauth2 token from Spotify
    /// </summary>
    /// <returns>Token</returns>
    internal async Task<TokenModel> AuthorizeAsync(CancellationTokenSource cts)
    {
        _listener = new HttpListener();
        _listener.Prefixes.Add($"{AddamsConfig.Hostname}:{AddamsConfig.Port}/");
        _listener.Start();

        var codeVerifier = Cypher.GenerateCodeVerifier();
        var codeChallenge = Cypher.GenerateCodeChallenge(codeVerifier);

        // Step 1 : Redirect the user to spotify to authorize application with PKCE Flow
        var url = $"{AddamsConfig.AuthorityUri}/authorize?" +
            $"client_id={Config.ClientID}" +
            $"&response_type={AddamsConfig.ResponseType}" +
            $"&redirect_uri={Uri.EscapeDataString(AddamsConfig.RedirectUri)}" +
            $"&scope={Uri.EscapeDataString(AddamsConfig.Scope)}" +
            $"&code_challenge_method={AddamsConfig.ChallengeMethod}" +
            $"&code_challenge={codeChallenge}";

        Debug.WriteLine($"Authenticate url : {url}");

        Process.Start(new ProcessStartInfo
        {
            FileName = url,
            UseShellExecute = true
        });

        var authorizationCode = string.Empty;
        while (true)
        {
            try
            {
                while (!cts.IsCancellationRequested)
                {
                    var context = await _listener.GetContextAsync();
                    var request = context.Request;
                    if (request.Url?.AbsolutePath == "/callback")
                    {
                        authorizationCode = request.QueryString.Get("code");
                        context.Response.Close();
                        cts.Cancel();
                        _listener.Stop();
                        break;
                    }
                }
            }
            catch (HttpListenerException) when (cts.IsCancellationRequested)
            {
                Debug.WriteLine("HttpListenerException to context async");
            }
            break;
        }

        // Step 2 : Call Spotify Api to exchange `Authorization Code` and `Code Verifier` for an `Access Token`
        var tokenEntity = await FetchTokenApiAsync(authorizationCode ?? string.Empty, codeVerifier);

        // Step 3 : Convert Token from entity to model
        TokenModel token = JsonConvert.DeserializeObject<TokenModel>(JsonConvert.SerializeObject(tokenEntity)) ?? new TokenModel();
        token.CalculateExpiration();
        if (string.IsNullOrEmpty(token.Value))
        {
            Debug.WriteLine(Language.GetString("String19"));
        }

        Debug.WriteLine($"\n\nToken d'accès : {token}\n\n");

        return token;
    }

    async Task<TokenModel> FetchTokenApiAsync(string authorizationCode, string codeVerifier)
    {
        var httpClient = new HttpClient();
        var url = $"{AddamsConfig.AuthorityUri}/api/token";

        var response = await httpClient.RequestAuthorizationCodeTokenAsync(new AuthorizationCodeTokenRequest
        {
            Address = url,
            ClientId = Config.ClientID,
            ClientSecret = Config.ClientSecret,
            Code = authorizationCode ?? string.Empty,
            RedirectUri = AddamsConfig.RedirectUri,
            CodeVerifier = codeVerifier
        });
        string content = response.Raw ?? string.Empty;
        TokenModel token = JsonConvert.DeserializeObject<TokenModel>(content) ?? new TokenModel();
        return token;
    }

    /// <summary>
    /// If we have refresh token from spotify we can use it to get access token
    /// </summary>
    /// <returns>Token</returns>
    public async Task<TokenModel> RefreshAsync()
    {
        Debug.WriteLine($"\n{Language.GetString("String56")}");

        HttpClient client = new();

        var data = new Dictionary<string, string>
        {
            { "grant_type", "refresh_token" },
            { "client_id", Config.ClientID },
            { "refresh_token", Config.Token.Refresh }
        };

        var response = await client.PostAsync("https://accounts.spotify.com/api/token", new FormUrlEncodedContent(data));

        response.EnsureSuccessStatusCode();

        string content = await response.Content.ReadAsStringAsync();

        TokenModel token = JsonConvert.DeserializeObject<TokenModel>(content) ?? new TokenModel();
        token.CalculateExpiration();

        Debug.WriteLine($"Token d'accès : {token}");

        return token;
    }

    /// <summary>
    /// Fetch name of the playlists
    /// </summary>
    /// <returns>List of playlist names</returns>
    public async Task<IEnumerable<PlaylistEntity>> GetPlaylistsNameAsync()
    {
        Playlists playlistsData = await Api.FetchPlaylistsAsync();

        List<PlaylistEntity> playlists = [.. playlistsData.Items];
        return [.. playlists.Prepend(new PlaylistEntity() { Name = LIKED_SONGS })];
    }

    /// <summary>
    /// Get playlist wanted by the user created or saved in Spotify
    /// </summary>
    /// <param name="playlistSelected">List of playlist wanted</param>
    /// <returns>Playlist's models</returns>
    public async Task<IEnumerable<Playlist>> GetPlaylistsAsync(IEnumerable<string> playlistSelected)
    {
        List<Playlist> playlists = [];
        Playlists playlistsData = await Api.FetchPlaylistsAsync();

        if (playlistsData.Items == null)
        {
            Debug.WriteLine(Language.GetString("String9"), Level.Warning);
            return [];
        }

        List<PlaylistEntity> playlistToFetch = [.. playlistsData.Items.Where(p => playlistSelected.Contains(p.Name))];

        Debug.WriteLine(string.Format(Language.GetString("String32"), string.Join("; ", playlistSelected)));

        if (playlistSelected.Contains(LIKED_SONGS))
        {
            Playlist likedPlaylist = await GetLikedTracksAsync();
            playlists.Add(likedPlaylist);
        }

        foreach (PlaylistEntity pe in playlistToFetch)
        {
            Playlist playlist = await GetPlaylistAsync(pe);
            Debug.WriteLine($"Playlist {playlist}");
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
    public async Task<Playlist> GetPlaylistAsync(PlaylistEntity playlist)
    {
        if (playlist.Id == null)
        {
            throw new SpotifyException($"getPlaylistTracks id null of the playlist name : {playlist.Name}");
        }

        IEnumerable<Track> tracks = await GetPlaylistTracksAsync(playlist.Id);

        return new Playlist
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
    public async Task<IEnumerable<Track>> GetPlaylistTracksAsync(string playlistId)
    {
        PlaylistTracks? playlistTracks = await Api.FetchTracksAsync(playlistId);

        if (playlistTracks == null
            || playlistTracks.Tracks == null
            || playlistTracks.Tracks.Items == null
            || !playlistTracks.Tracks.Items.Any()
            )
        {
            return [];
        }

        List<Track> tracks = [];
        foreach (TrackItem ti in playlistTracks.Tracks.Items)
        {
            if (ti.Track == null)
                continue;
            tracks.Add(GetTrack(ti));
        }

        return tracks;
    }

    /// <summary>
    /// Create track object base on trackData fetch from spotify API
    /// </summary>
    /// <param name="trackEntity">track data from playlist call</param>
    /// <returns>Track model object</returns>
    public static Track GetTrack(TrackItem trackEntity)
    {
        TrackEntity track = trackEntity.Track;
        if (track.Id == null)
        {
            Debug.WriteLine($"GetTrackData id null of the track name: {track.Name}");
            return new Track
            {
                Name = track.Name,
                Artists = string.Join(",", track.Artists.Select(x => x.Name))
            };
        }

        return new Track
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
            AddedAt = trackEntity.added_at.ToString() ?? string.Empty,
            AlbumImageUrl = track.Album.images.FirstOrDefault()?.url ?? string.Empty,
            TrackPreviewUrl = track.PreviewUrl,
            TrackUri = track.Uri,
            ArtistUrl = track.Artists.FirstOrDefault()?.Uri ?? string.Empty,
            AlbumUrl = track.Album.uri ?? string.Empty,
            Explicit = track.Explicit,
            IsLocal = track.IsLocal,
            Duration = track.DurationMs,
        };
    }

    /// <summary>
    /// Fetch all the tracks in liked songs on Spotify
    /// </summary>
    /// <returns>Playlist model with liked songs</returns>
    public async Task<Playlist> GetLikedTracksAsync()
    {
        TrackList likedPlaylistEnt = await Api.FetchLikedTracksAsync();

        if (likedPlaylistEnt == null
           || likedPlaylistEnt.Items == null
           || likedPlaylistEnt.Href == null
           )
        {
            return new Playlist();
        }

        List<Track> tracks = [];
        foreach (TrackItem ti in likedPlaylistEnt.Items)
        {
            if (ti.Track == null)
                continue;
            tracks.Add(GetTrack(ti));
        }

        string href = likedPlaylistEnt.Href[..likedPlaylistEnt.Href.IndexOf('?')];

        return new Playlist()
        {
            Name = "0 - Liked Songs",
            Description = "Playlist of liked songs",
            Href = href,
            Tracks = tracks,
        };
    }
}
