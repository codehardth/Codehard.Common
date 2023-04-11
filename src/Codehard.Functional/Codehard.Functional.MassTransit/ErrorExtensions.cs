using Codehard.Functional.MassTransit.Exceptions;

namespace LanguageExt.Common;

/// <summary>
/// Provides extension methods for the <see cref="Error"/> class.
/// </summary>
public static class ErrorExtensions
{
    /// <summary>
    /// Gets the fault message of the given error object.
    /// </summary>
    /// <param name="error">The error object.</param>
    /// <returns>A string representing the fault message of the error.</returns>
    /// <remarks>
    /// If the error object can be converted to a <see cref="MassTransitFaultMessageException"/>,
    /// then the method returns the <see cref="MassTransitFaultMessageException.Reason"/> property of that exception.
    /// Otherwise, it returns the <see cref="Exception.Message"/> property of the exception that the error object can be converted to.
    /// </remarks>
    public static string? GetFaultMessage(this Error error)
        => error.ToException() switch
        {
            MassTransitFaultMessageException fmEx => fmEx.Reason,
            { } ex => ex.Message,
        };
}