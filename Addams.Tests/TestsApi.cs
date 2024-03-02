using Addams.Entities;
using Addams.Models;
using Addams.Exceptions;
using NUnit.Framework;

namespace Addams.Tests;

[TestFixture]
public class TestsApi
{
    private SpotifyApi api;

    [SetUp]
    public void SetUp()
    {
        SpotifyConfig cfg = new()
        {
            UserName = "gravityx3",
            Token = new TokenModel { Value = "BQBDF2im795ZY3UBVmI6vioexj7HMt47_URtBbIDbtUWKXuGaLGDbnwUrNEe0n9VbfF4aY9DcEZlN23pO354gpFhJLpUEGwjk4sBzgVlePPRfiyOxyJeji7XbOB5eFqgi_HTWU6tj8sSk30FDQvIyPIxmCGAk3_H1qtd8ZNpHi68Nn2q1Mq_5Qfmf9Z13HKHUASObLfjSgvyF9Q-fXoo0w" },
        };

        api = new SpotifyApi(cfg);
    }

    [Test]
    [Description("Test with wrong token")]
    public void TestGetPlaylistWithBadToken()
    {
        // Arrange
        SpotifyConfig config = new()
        {
            UserName = "gravityx3",
            Token = new TokenModel { Value = "bad_token" },
        };
        SpotifyApi spotifyApi = new(config);

        // Act & Assert
        _ = Assert.ThrowsAsync<SpotifyUnauthorizedException>(spotifyApi.FetchPlaylistsAsync);
    }

    [Test]
    [Description("Test to fetch playlist data")]
    public async Task TestGetPlaylistWithRightTokenAsync()
    {
        // Act
        Playlists playlists = await api.FetchPlaylistsAsync();

        // Assert
        Assert.IsNotNull(playlists);
        Assert.IsNotEmpty(playlists.Items);
        Assert.IsNull(playlists.Next);
        Assert.IsNotNull(playlists.Href);
        Assert.That(playlists.Total == playlists.Items.ToList().Count);
    }

    [Test]
    [Description("Test to fetch playlist data")]
    [TestCase("0CFuMybe6s77w6QQrJjW7d")]
    public async Task TestGetPlaylistTracksMoreThan100TracksAsync(string playlistId)
    {
        // Act
        PlaylistTracks playlistTracks = await api.FetchTracksAsync(playlistId);

        // Assert
        Assert.IsNotNull(playlistTracks);
        Assert.IsNotNull(playlistTracks.Href);
        Assert.IsNotNull(playlistTracks.Name);
        Assert.IsNotEmpty(playlistTracks.Tracks.Items);
        Assert.That(playlistTracks.Tracks.Total == playlistTracks.Tracks.Items.Count);
        Assert.IsNull(playlistTracks.Tracks.Next);
    }
}