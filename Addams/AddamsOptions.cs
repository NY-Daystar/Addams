using System;

namespace Addams
{
    /// <summary>
    /// Class to handle options from arguments passed to executable
    /// </summary>
    public class AddamsOptions
    {
        /// <summary>
        /// Boolean to set if we are in debug log or not
        /// </summary>
        public bool Debug { get; set; }

        /// <summary>
        /// Define options from arguments executable file
        /// Check if arg '--debug' is mentionned
        /// </summary>
        /// <param name="args">arguments passed from files</param>
        /// <returns>Dict of options with their values</returns>
        public static AddamsOptions DefineOptions(string[] args)
        {
            return new AddamsOptions()
            {
                Debug = Array.Exists(args, el => el.StartsWith("--debug"))
            };
        }
    }
}