using Addams.Entities;
using NUnit.Framework;
using System.Globalization;

namespace Addams.Tests;

[TestFixture]
public class TestsConfig
{
    [Test]
    public void TestConfigFilePathValid()
    {
        // Assert
        Assert.IsNotNull(SpotifyConfig.ConfigFilepath);
        Assert.That(SpotifyConfig.ConfigFilepath != null);
    }

    [Test]
    public void TestSerializeConfig()
    {
        // Arrange
        SpotifyConfig config = new()
        {
            UserName = "MY_USER",
            ClientID = "MY_CLIENT_ID",
            ClientSecret = "MY_CLIENT_SECRET",
            Token = new Models.TokenModel {
                Value = "MY_TOKEN",
                Type = "Bearer",
                ExpiresIn = 600,
                Scope = "MY_SCOPE",
                GeneratedAt = DateTime.ParseExact("2024-03-02T21:21:43", "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture),
                ExpiredDate = DateTime.ParseExact("2024-03-02T21:31:43", "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture),
            },
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