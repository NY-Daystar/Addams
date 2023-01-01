using Addams.Api;
using Addams.Entities;
using Addams.Exceptions;
using NUnit.Framework;

namespace Addams.Tests
{
    public class TestsApi
    {
        private string authToken = @"BQD4FzvnXKMgRMLU3-gGNmkKqFkSEqCC9KLdvPnBzXQWpdYvO2PtdReosCdAjvaa2E3WKdWjKkO7hHLZwGCFkbD8q4LgWBD0hdEiAoamA9eeAd2TZMe8jKcJL_fJF5HvQ14l3x-aga3O8ikglvbmkMfZVIPWoZocOcwnKzpFvkjPA6h_LDki0BVXljffL6k4l6G_zgqdLhE";
        private string user = "gravityx3";
        private SpotifyApi api;

        [SetUp]
        // TODO generer un token a terme
        public void SetUp()
        {
            api = new SpotifyApi(user, authToken);
        }


        [Test]
        public void TestGetPlaylistWithBadToken()
        {
            string token = "bad_token";
            string user = "gravityx3";
            SpotifyApi api = new SpotifyApi(user, token);

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