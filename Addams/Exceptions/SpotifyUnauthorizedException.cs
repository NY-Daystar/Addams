using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addams.Exceptions
{
    public class SpotifyUnauthorizedException : Exception
    {
        public SpotifyUnauthorizedException()
        {
        }

        public SpotifyUnauthorizedException(string message)
            : base(message)
        {
        }

        public SpotifyUnauthorizedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
