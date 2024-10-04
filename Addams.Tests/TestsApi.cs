using Addams.Entities;
using Addams.Models;
using Addams.Exceptions;
using NUnit.Framework;

namespace Addams.Tests;

[TestFixture]
public class TestsApi
{
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
}