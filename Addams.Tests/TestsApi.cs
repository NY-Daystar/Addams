using Addams.Core;
using Addams.Core.Exceptions;
using Addams.Core.Models;
using Addams.Core.Spotify;

namespace Addams.Tests;

[TestClass]
public class TestsApi
{
    [TestMethod]
    [Description("Test with wrong token")]
    public void TestGetPlaylistWithBadToken()
    {
        // Arrange
        AddamsConfig config = new()
        {
            UserName = "gravityx3",
            Token = new TokenModel { Value = "bad_token" },
        };
        SpotifyApi spotifyApi = new(config);

        // Act & Assert
        _ = Assert.ThrowsAsync<SpotifyUnauthorizedException>(spotifyApi.FetchPlaylistsAsync);
    }
}