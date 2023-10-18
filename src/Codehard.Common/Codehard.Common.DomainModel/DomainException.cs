namespace Codehard.Common.DomainModel;

/// <summary>
/// Represents an exception that is thrown when a domain logic error occurs.
/// </summary>
[Serializable]
public class DomainLogicException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DomainLogicException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public DomainLogicException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DomainLogicException"/> class with a specified error message and a reference
    /// to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public DomainLogicException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}