namespace Addams.Core.Exceptions;

public class ComboBoxException : Exception
{
    public ComboBoxException() { }

    public ComboBoxException(string message)
        : base(message) { }

    public ComboBoxException(string message, Exception inner)
        : base(message, inner) { }
}
