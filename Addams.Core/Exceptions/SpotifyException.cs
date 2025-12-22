using System.Net.Http.Headers;

namespace Addams.Core.Exceptions;

public class SpotifyException : Exception
{
    private const string RegexTooManyRequest = "TooManyRequests";
    public bool IsTooManyRequest { get; set; }

    public TimeSpan RetryAfter { get; set; }

    public SpotifyException() { }

    public SpotifyException(string message)
        : base(message) 
    {}

    public SpotifyException(string message, HttpResponseHeaders headers)
        : base(message)
    {
        IsTooManyRequest = message.Contains(RegexTooManyRequest);
        RetryAfter = headers?.RetryAfter?.Delta ?? new TimeSpan();
    }

    public SpotifyException(string message, Exception inner)
        : base(message, inner) { }
}
