using System.IO;

namespace Addams.Utils
{
    /// <summary>
    /// Class to get valid path and filename
    /// </summary>
    public class PathUtil
    {
        /// <summary>
        /// Remove invalid characters for filename including '/' and ':'
        /// </summary>
        /// <param name="filename">Filename to check and modify if not valid</param>
        /// <returns>string with valid char for a filenamee</returns>
        public static string FormatValidFilename(string filename)
        {
            filename = filename.Replace("/", "-").Replace(":", "-");
            foreach (char invalidChar in Path.GetInvalidFileNameChars())
            {
                filename = filename.Replace(invalidChar.ToString(), "");
            }
            return filename;
        }

        /// <summary>
        /// Remove invalid characters for path
        /// </summary>
        /// <param name="path">Path to check and modify if not valid</param>
        /// <returns>path with valid char for a path</returns>
        public static string FormatValidPath(string path)
        {
            foreach (char invalidChar in Path.GetInvalidPathChars())
            {
                path = path.Replace(invalidChar.ToString(), "");
            }
            return path;
        }
    }
}
