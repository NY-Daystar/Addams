using System;

namespace Addams.Exceptions;

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
