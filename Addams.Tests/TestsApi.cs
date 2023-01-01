using Addams.Api;
using Addams.Entities;
using Addams.Exceptions;
using NUnit.Framework;

namespace Addams.Tests
{
    public class TestsApi
    {
        private string authToken = @"BQDJIh8gVYoovzVjLOSQ46Kv-tkp0sU6eoInjNg1slH0vSxjbXgxEaWrTINiDxF7T8ALqayWVV1dhcaHEaN_1tdoxEH5rIrmYrGA-dIDEWVHNDlzVeq2TJwIgoIarNY-28slsfjRcFO0ldpVaX1SyT6gt_o0-go-f593C7ZjOxG1hNnGGtjU9IJ7Yl8SwGzXZqIwVbPx1iY";
        private string user = "gravityx3";
        private SpotifyApi api;

        [SetUp]
        // TODO generer un token a terme
        public void SetUp()
        {
            SpotifyConfig cfg = new()
            {
                User = user,
                Token = authToken,
            };
            api = new SpotifyApi(cfg);
        }


        [Test]
        public void TestGetPlaylistWithBadToken()
        {

            SpotifyApi api = new SpotifyApi(new SpotifyConfig()
            {
                User = "gravityx3",
                Token = "bad_token",
            });

            Assert.ThrowsAsync<SpotifyUnauthorizedException>(() => api.FetchUserPlaylists());
        }

        [Test]
        /// <summary>
        /// TODO you need to regenerate token if you don't need
        /// </summary>
        public async Task TestGetPlaylistWithRightToken()
        {
            Playlists playlists = await api.FetchUserPlaylists();

            Assert.IsNotNull(playlists);
            Assert.IsNotEmpty(playlists.items);
            Assert.IsNull(playlists.next);
            Assert.IsNotNull(playlists.href);
            Assert.That(playlists.total == playlists.items.Count);
        }

        [Test]
        public async Task TestGetPlaylistTracksMoreThan100Tracks()
        {
            string id = "0CFuMybe6s77w6QQrJjW7d";
            PlaylistTracks playlistTracks = await api.FetchPlaylistTracks(id);

            Assert.IsNotNull(playlistTracks);
            Assert.IsNotNull(playlistTracks.href);
            Assert.IsNotNull(playlistTracks.name);
            Assert.IsNotEmpty(playlistTracks.tracks.items);
            Assert.That(playlistTracks.tracks.total == playlistTracks.tracks.items.Count);
            Assert.IsNull(playlistTracks.tracks.next);
        }
    }
}