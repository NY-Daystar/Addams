using System;

namespace Addams.Exceptions;

public class SpotifyConfigException : Exception
{
    public SpotifyConfigException()
    {
    }

    public SpotifyConfigException(string message)
        : base(message)
    {
    }

    public SpotifyConfigException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
