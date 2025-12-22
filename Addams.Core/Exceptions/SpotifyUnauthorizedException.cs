namespace Addams.Core.Exceptions;

public class SpotifyUnauthorizedException : SpotifyException
{
    public SpotifyUnauthorizedException() { }

    public SpotifyUnauthorizedException(string message)
        : base(message) { }

    public SpotifyUnauthorizedException(string message, Exception inner)
        : base(message, inner) { }
}
