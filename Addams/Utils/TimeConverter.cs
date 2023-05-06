using System;

namespace Addams.Utils
{
    /// <summary>
    /// Class to handle time conversion
    /// </summary>
    public static class TimeConverter
    {
        /// <summary>
        /// Get timeSpan from ms value
        /// </summary>
        /// <param name="milliSeconds">milliseconds to convert</param>
        /// <returns>TimeSpan value</returns>
        public static TimeSpan ConvertMsToTimeSpan(int milliseconds)
        {
            return TimeSpan.FromMilliseconds(milliseconds);
        }

        /// <summary>
        /// Format time (XX:YY:ZZ) with XX hours, YY minutes and ZZ seconds and removing milliseconds
        /// </summary>
        /// <param name="ts">TimeSpan with values</param>
        /// <returns>string formatted like this (01:02:35) for 1 hour, 2 minutes and 35 seconds</returns>
        public static string FormatTimeSpan(TimeSpan ts)
        {
            return new TimeSpan(ts.Hours, ts.Minutes, ts.Seconds).ToString();
        }
    }
}
