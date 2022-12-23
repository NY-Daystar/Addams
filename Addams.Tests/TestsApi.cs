using NUnit.Framework;
using Addams.Api;
using Addams.Entities;
using Addams.Exceptions;

namespace Addams.Tests
{
    public class TestsApi
    {

        [Test]
        public void TestGetPlaylistWithBadToken()
        {
            string token = "bad_token";
            string user = "gravityx3";
            SpotifyApi api = new SpotifyApi(user, token);

            Assert.ThrowsAsync<SpotifyUnauthorizedException>(() => api.FetchUserPlaylists());
        }

        /// <summary>
        /// README you need to regenerate token if you don't need
        /// </summary>
        public void TestGetPlaylistWithRightToken()
        {
            /// README you need to regenerate token if it's expired
            string token = ""; // TODO meettrre un boon token
            string user = "gravityx3";
            SpotifyApi api = new SpotifyApi(user, token);

            Assert.ThrowsAsync<SpotifyUnauthorizedException>(() => api.FetchUserPlaylists());
            Assert.Pass();
        }
    }
}