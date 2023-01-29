using Addams.Entities;
using Addams.Exceptions;
using NUnit.Framework;

namespace Addams.Tests
{
    public class TestsApi
    {
        private SpotifyApi api;

        [SetUp]
        // TODO generer un token a terme
        public void SetUp()
        {
            SpotifyConfig cfg = new();
            api = new SpotifyApi(cfg);
        }

        [Test]
        public void TestGetPlaylistWithBadToken()
        {
            SpotifyConfig config = new()
            {
                User = "gravityx3",
                Token = "bad_token",
            };
            SpotifyApi api = new(config);

            _ = Assert.ThrowsAsync<SpotifyUnauthorizedException>(api.FetchPlaylists);
        }

        [Test]
        /// <summary>
        /// TODO feature OAUTH2 you need to regenerate token if you don't need
        /// </summary>
        public async Task TestGetPlaylistWithRightToken()
        {
            Playlists playlists = await api.FetchPlaylists();

            Assert.IsNotNull(playlists);
            Assert.IsNotEmpty(playlists.items);
            Assert.IsNull(playlists.next);
            Assert.IsNotNull(playlists.href);
            Assert.That(playlists.total == playlists.items.Count);
        }

        [Test]
        [TestCase("0CFuMybe6s77w6QQrJjW7d")]
        public async Task TestGetPlaylistTracksMoreThan100Tracks(string playlistId)
        {
            PlaylistTracks playlistTracks = await api.FetchTracks(playlistId);

            Assert.IsNotNull(playlistTracks);
            Assert.IsNotNull(playlistTracks.href);
            Assert.IsNotNull(playlistTracks.name);
            Assert.IsNotEmpty(playlistTracks.tracks.items);
            Assert.That(playlistTracks.tracks.total == playlistTracks.tracks.items.Count);
            Assert.IsNull(playlistTracks.tracks.next);
        }
    }
}