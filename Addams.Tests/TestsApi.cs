using Addams.Entities;
using Addams.Exceptions;
using NUnit.Framework;

namespace Addams.Tests
{
    [TestFixture]
    public class TestsApi
    {
        private SpotifyApi api;

        [SetUp]
        // TODO feature OAUTH2 : generer un token a terme
        public void SetUp()
        {
            SpotifyConfig cfg = new();
            api = new SpotifyApi(cfg);
        }

        [Test]
        public void TestGetPlaylistWithBadToken()
        {
            // Arrange
            SpotifyConfig config = new()
            {
                User = "gravityx3",
                Token = "bad_token",
            };
            SpotifyApi api = new(config);

            // Act & Assert
            _ = Assert.ThrowsAsync<SpotifyUnauthorizedException>(api.FetchPlaylists);
        }

        [Test]
        /// <summary>
        /// TODO feature OAUTH2 you need to regenerate token if you don't need
        /// </summary>
        public async Task TestGetPlaylistWithRightToken()
        {
            // Arrange
            Playlists playlists;

            // Act
            playlists = await api.FetchPlaylists();

            // Assert
            Assert.IsNotNull(playlists);
            Assert.IsNotEmpty(playlists.items);
            Assert.IsNull(playlists.next);
            Assert.IsNotNull(playlists.href);
            Assert.That(playlists.total == playlists.items.ToList().Count);
        }

        [Test]
        [TestCase("0CFuMybe6s77w6QQrJjW7d")]
        public async Task TestGetPlaylistTracksMoreThan100Tracks(string playlistId)
        {
            // Arrange
            PlaylistTracks playlistTracks;

            // Act
            playlistTracks = await api.FetchTracks(playlistId);

            // Assert
            Assert.IsNotNull(playlistTracks);
            Assert.IsNotNull(playlistTracks.href);
            Assert.IsNotNull(playlistTracks.name);
            Assert.IsNotEmpty(playlistTracks.tracks.items);
            Assert.That(playlistTracks.tracks.total == playlistTracks.tracks.items.ToList().Count);
            Assert.IsNull(playlistTracks.tracks.next);
        }
    }
}