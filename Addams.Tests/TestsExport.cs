using Addams.Core.Spotify;
using Addams.Core.Utils;
using Addams.Core.Models;

namespace Addams.Tests;

[TestClass]
public class TestsExport
{
    [TestMethod]
    public void TestSavePlaylistWithInvalidFilename()
    {
        // Arrange
        string playlistName = "Chillhop Radio 🐾 jazz/lofi hip hop beats to study/relax to | Study Music | Chillhop Music 2022";
        string exePath = typeof(TestsExport).Assembly.Location ?? throw new Exception("exe path is null");
        string exeFolder = Path.GetDirectoryName(exePath) ?? throw new Exception("exe folder path is null");
        string wDir = Path.Combine(exeFolder, "data");
        if (!Directory.Exists(wDir))
        {
            _ = Directory.CreateDirectory(wDir);
        }

        // Act
        SpotifyExport.SavePlaylist(wDir, new Playlist
        {
            Name = playlistName,
            Tracks =
            [
                new() {Name="Track1" },
                new() {Name="Track2" },
                new() {Name="Track3" },
            ]
        });
        string playlistPathAfterValidate = $"{PathUtil.FormatValidFilename(playlistName)}.csv";
        string playlistPath = Path.Combine(wDir, playlistPathAfterValidate);

        // Assert
        Assert.IsTrue(File.Exists(playlistPath));
    }
}