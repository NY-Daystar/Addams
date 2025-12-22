using Addams.Core.Entities;
using Addams.Core.Models;
using Addams.Core.Utils;
using Newtonsoft.Json;

namespace Addams.Tests;

[TestClass]
public class TestsModel
{
    private const string SAMPLES = @"D:\Dev\Addams\Addams.Tests\Samples\";

    [TestMethod]
    [DataRow(155000, "00:02:35")]
    [DataRow(5747000, "01:35:47")]
    [DataRow(186038, "00:03:06")]
    public void TestDurationValue(int duration, string expected)
    {
        // Act
        TimeSpan ts = TimeConverter.ConvertMsToTimeSpan(duration);
        string time = TimeConverter.FormatTimeSpan(ts);

        // Assert
        Assert.AreEqual(expected, time);
    }

    [TestMethod]
    public void TestTrackDuration()
    {
        // Arrange
        const string expected = "00:04:12";
        const int duration = 252000;

        // Act
        Track track = new() { Duration = duration };

        // Assert
        Assert.AreEqual(expected, track.DurationFormatted);
    }


    [TestMethod]
    [DataRow("response-liked-track.json")]
    public void TestDeserializeLikeTracks(string file)
    {
        // Arrange
        string content = File.ReadAllText(Path.Combine(SAMPLES, file));

        // Act
        var trackList = JsonConvert.DeserializeObject<TrackList>(content) ?? new TrackList();

        // Assert
        Assert.IsNotEmpty(trackList.Items[0].Track.Name);
    }

    [TestMethod]
    [DataRow("response-user-playlists.json")]
    public void TestDeserializeUserPlaylist(string file)
    {
        // Arrange
        string content = File.ReadAllText(Path.Combine(SAMPLES, file));

        // Act
        Playlists playlists = JsonConvert.DeserializeObject<Playlists>(content) ?? new Playlists();

        // Assert
        Assert.IsNotEmpty(playlists.Items.ToList()[0].Name);
    }

    [TestMethod]
    [DataRow("response-playlist.json")]
    public void TestDeserializePlaylistTracks(string file)
    { 
        // Arrange
        string content = File.ReadAllText(Path.Combine(SAMPLES, file));

        // Act
        var playlist = JsonConvert.DeserializeObject<PlaylistTracks>(content) ?? new PlaylistTracks();

        // Assert
        Assert.IsNotEmpty(playlist.Tracks.Items[0].Track.Name);
    }

    [TestMethod]
    [DataRow("response-track.json")]
    public void TestDeserializeTracks(string file)
    {
        // Arrange
        string content = File.ReadAllText(Path.Combine(SAMPLES, file));

        // Act
        TrackEntity track = JsonConvert.DeserializeObject<TrackEntity>(content) ?? new TrackEntity();

        // Assert
        Assert.IsNotEmpty(track.Name);
    }
}