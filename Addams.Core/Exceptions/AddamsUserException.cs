namespace Addams.Core.Exceptions;

public class AddamsUserException : Exception
{
    public AddamsUserException() { }

    public AddamsUserException(string message)
        : base(message) { }

    public AddamsUserException(string message, Exception inner)
        : base(message, inner) { }
}
