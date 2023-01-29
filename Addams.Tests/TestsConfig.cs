using NUnit.Framework;

namespace Addams.Tests
{
    public class TestsConfig
    {
        [Test]
        public void TestConfigFilePathValid()
        {
            Assert.IsNotNull(SpotifyConfig.ConfigFilepath);
            Assert.IsNotEmpty(SpotifyConfig.ConfigFilepath);
        }

        [Test]
        public void TestSerializeConfig()
        {
            SpotifyConfig config = new()
            {
                ClientID = "MY_CLIENT_ID",
                ClientSecret = "MY_CLIENT_SECRET",
                Token = "MY_TOKEN",
                User = "MY_USER"
            };

            config.Save();

            SpotifyConfig cfg = SpotifyConfig.Read();

            Assert.IsNotEmpty(File.ReadAllText(SpotifyConfig.ConfigFilepath));
            Assert.That(cfg.Equals(config));
            Assert.IsNotNull(cfg);
        }
    }
}