namespace Codehard.Common.Exceptions;

public sealed class UnexpectedStateException<T> : Exception
{
    public UnexpectedStateException(T expected, T actual)
        : base($"The object cannot be operate due to its state is not in '{expected}' (was '{actual}')")
    {
    }
}