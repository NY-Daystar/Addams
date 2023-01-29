using Addams.Models;
using Addams.Utils;
using NUnit.Framework;

namespace Addams.Tests
{
    public class TestsModel
    {
        [Test]
        public void TestDurationValue()
        {
            // First test
            string expected = "00:02:35";
            int duration = 155000;

            TimeSpan ts = TimeConverter.ConvertMsToTimeSpan(duration);
            string time = TimeConverter.FormatTimeSpan(ts);

            Assert.That(expected == time);

            // Second test
            expected = "01:35:47";
            duration = 5747000;

            ts = TimeConverter.ConvertMsToTimeSpan(duration);
            time = TimeConverter.FormatTimeSpan(ts);
            Assert.That(expected == time);
        }

        [Test]
        public void TestTrackDuration()
        {
            string expected = "00:04:12";
            int duration = 252000;

            Track t = new() { _duration = duration };

            Assert.That(expected == t.Duration);

        }
    }
}