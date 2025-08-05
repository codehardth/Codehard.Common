using LanguageExt.Common;

namespace Codehard.Functional.Logger;

/// <summary>
/// An interface for handling exceptions and errors in a structured and customizable manner.
/// This interface allows implementations to define how errors are processed and logged
/// using a specified logging framework and log level.
/// </summary>
public interface IExceptionHandler
{
    /// <summary>
    /// Handles an error by logging it using the provided logger at the specified log level.
    /// Can use a custom logic defined in the implementing class to process and log errors based
    /// on the error details, associated exception, and log level.
    /// </summary>
    /// <param name="error">The error object encapsulating exception details and an optional message.</param>
    /// <param name="logger">The logger instance used to log the error details.</param>
    /// <param name="logLevel">The severity level at which the error should be logged. Defaults to <see cref="LogLevel.Information"/>.</param>
    /// <returns>A <see cref="LanguageExt.Unit"/> value indicating the outcome of the operation.</returns>
    Unit Handle(Error error, ILogger logger, LogLevel logLevel = LogLevel.Information);
}