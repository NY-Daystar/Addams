namespace Addams.Core.Exceptions;

public class TrackException : Exception
{
    public TrackException() { }

    public TrackException(string message)
        : base(message) { }

    public TrackException(string message, Exception inner)
        : base(message, inner) { }
}
