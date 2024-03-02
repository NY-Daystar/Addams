using Addams.Utils;
using NUnit.Framework;

namespace Addams.Tests;

[TestFixture]
public class TestsExport
{
    [Test]
    public void TestSavePlaylistWithInvalidFilename()
    {
        // Arrange
        string playlistName = "Chillhop Radio 🐾 jazz/lofi hip hop beats to study/relax to | Study Music | Chillhop Music 2022";
        string exePath = System.Reflection.Assembly.GetExecutingAssembly().Location ?? throw new Exception("exe path is null");
        string exeFolder = Path.GetDirectoryName(exePath) ?? throw new Exception("exe folder path is null");
        string wDir = Path.Combine(exeFolder, "data");
        if (!Directory.Exists(wDir))
        {
            _ = Directory.CreateDirectory(wDir);
        }

        // Act
        SpotifyExport.SavePlaylist(wDir, new Models.Playlist
        {
            Name = playlistName,
            Tracks = new List<Models.Track>
            {
                new Models.Track {Name="Track1" },
                new Models.Track {Name="Track2" },
                new Models.Track {Name="Track3" },
            }
        });
        string playlistPathAfterValidate = $"{PathUtil.FormatValidFilename(playlistName)}.csv";
        string playlistPath = Path.Combine(wDir, playlistPathAfterValidate);

        // Assert
        Assert.IsTrue(File.Exists(playlistPath));
    }
}