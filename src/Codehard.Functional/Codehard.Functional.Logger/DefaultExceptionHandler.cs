using LanguageExt.Common;

namespace Codehard.Functional.Logger;

/// <summary>
/// Provides a default exception handling mechanism for logging errors using an <see cref="ILogger"/> instance.
/// </summary>
public static class DefaultExceptionHandler
{
    /// <summary>
    /// Handles logging of an error using the specified <see cref="ILogger"/> instance.
    /// </summary>
    /// <param name="logger">The logger used to log the error information.</param>
    /// <param name="error">The error to be logged, containing an optional exception and message.</param>
    /// <param name="logLevel">The logging level to use, defaults to <see cref="LogLevel.Information"/>.</param>
    public static void Handle(ILogger logger, Error error, LogLevel logLevel = LogLevel.Information)
    {
        error.Exception.Match(
            Some: ex =>
            {
                // Exception will always log as an error
                logger.LogError(ex, "{Message}", error.Message);
            },
            None: () =>
            {
                if (string.IsNullOrWhiteSpace(error.Message))
                {
                    logger.Log(logLevel, "{Code}", error.Code);
                }
                else
                {
                    logger.Log(logLevel, "{Code} : {Message}", error.Code, error.Message);
                }
            });
    }
}