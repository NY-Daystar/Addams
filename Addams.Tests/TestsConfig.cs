using NUnit.Framework;

namespace Addams.Tests
{
    public class TestsConfig
    {
        [Test]
        public void TestConfigFilePathValid()
        {
            SpotifyConfig config = new SpotifyConfig();
            Assert.IsNotNull(SpotifyConfig.filePath);
            Assert.IsNotEmpty(SpotifyConfig.filePath);
        }

        [Test]
        public void TestSerializeConfig()
        {
            SpotifyConfig config = new()
            {
                clientID = "MY_CLIENT_IDa",
                token = "MY_TOKEN",
                user = "MY_USER"
            };

            config.Save();

            SpotifyConfig cfg = SpotifyConfig.Read();

            Assert.IsNotEmpty(File.ReadAllText(SpotifyConfig.filePath));
            Assert.That(cfg.Equals(config));
            Assert.IsNotNull(cfg);
        }
    }
}