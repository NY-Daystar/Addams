using Addams.Models;
using Addams.Utils;
using NUnit.Framework;

namespace Addams.Tests;

[TestFixture]
public class TestsModel
{
    [Test]
    [TestCase(155000, "00:02:35")]
    [TestCase(5747000, "01:35:47")]
    public void TestDurationValue(int duration, string expected)
    {
        // Act
        TimeSpan ts = TimeConverter.ConvertMsToTimeSpan(duration);
        string time = TimeConverter.FormatTimeSpan(ts);

        // Assert
        Assert.That(expected == time);
    }

    [Test]
    public void TestTrackDuration()
    {
        // Arrange
        const string expected = "00:04:12";
        const int duration = 252000;

        // Act
        Track track = new() { Duration = duration };

        // Assert
        Assert.That(expected == track.DurationFormatted);
    }
}