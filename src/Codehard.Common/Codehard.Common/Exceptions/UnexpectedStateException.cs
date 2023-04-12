namespace Codehard.Common.Exceptions;

/// <summary>
/// Represents an exception that is thrown when an object cannot be operated due to being in an unexpected state.
/// </summary>
/// <typeparam name="T">The type of the state.</typeparam>
[Serializable]
public sealed class UnexpectedStateException<T> : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UnexpectedStateException{T}"/> class with the expected and actual states.
    /// </summary>
    /// <param name="expected">The expected state.</param>
    /// <param name="actual">The actual state.</param>
    public UnexpectedStateException(T expected, T actual)
        : base($"The object cannot be operated because its state is not '{expected}' (actual state was '{actual}')")
    {
    }
}