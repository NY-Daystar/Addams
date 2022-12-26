using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addams.Utils
{
    public class PathUtil
    {
        // TODO to comment : remove invalid path characters for path
        public static string FormatValidPath(string path)
        {
            foreach (char invalidChar in Path.GetInvalidPathChars())
            {
                path = path.Replace(invalidChar.ToString(), "");
            }
            return path;
        }

        // TODO to comment : remove invalid path characters for filename including / and :
        public static string FormatValidFilename(string filename)
        {
            filename = filename.Replace("/", "-").Replace(":", "-");
            foreach (char invalidChar in Path.GetInvalidFileNameChars())
            {
                filename = filename.Replace(invalidChar.ToString(), "");
            }
            return filename;
        }
    }
}
