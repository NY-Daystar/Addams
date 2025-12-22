using Addams.Core.Exceptions;

namespace Addams.Core.Spotify;

public class SpotifyAuthenticationStatus(bool status)
{
    public bool Status { get; set; } = status;

    public SpotifyException? Exception { get; set; }
}
