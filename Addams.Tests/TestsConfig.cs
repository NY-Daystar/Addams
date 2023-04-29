using NUnit.Framework;

namespace Addams.Tests
{
    [TestFixture]
    public class TestsConfig
    {
        [Test]
        public void TestConfigFilePathValid()
        {
            // Assert
            Assert.IsNotNull(SpotifyConfig.ConfigFilepath);
            Assert.IsNotEmpty(SpotifyConfig.ConfigFilepath);
        }

        [Test]
        public void TestSerializeConfig()
        {
            // Arrange
            SpotifyConfig config = new()
            {
                ClientID = "MY_CLIENT_ID",
                ClientSecret = "MY_CLIENT_SECRET",
                Token = "MY_TOKEN",
                User = "MY_USER"
            };

            config.Save();

            // Act
            SpotifyConfig cfg = SpotifyConfig.Read();

            // Assert
            Assert.IsNotEmpty(File.ReadAllText(SpotifyConfig.ConfigFilepath));
            Assert.That(cfg.Equals(config));
            Assert.IsNotNull(cfg);
        }
    }
}