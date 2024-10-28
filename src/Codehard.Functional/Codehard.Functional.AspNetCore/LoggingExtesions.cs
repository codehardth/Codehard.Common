using Codehard.Functional.Logger;

namespace Codehard.Functional.AspNetCore;

/// <summary>
/// Contains extension methods for logging HTTP result errors with an <see cref="ILogger"/> instance.
/// </summary>
public static class LoggingExtensions
{
    /// <summary>
    /// Logs the specified HTTP result error using the provided logger.
    /// </summary>
    /// <param name="logger">The logger to use for logging the error.</param>
    /// <param name="error">The HTTP result error to log.</param>
    public static void Log(this ILogger logger, HttpResultError error)
    {
        switch (error.StatusCode)
        {
            case >= HttpStatusCode.InternalServerError:
                logger.LogError(
                    exception: error.Exception.IfNoneUnsafe(default(Exception)),
                    message: "{ResponseStatus}, {ErrorCode}, {Message}",
                    error.StatusCode,
                    error.ErrorCode.IfNoneUnsafe(default(string)),
                    error.Message);

                error.Inner.Do(
                    err =>
                        logger.Log(err, logLevel: LogLevel.Error));
                break;
            
            case >= HttpStatusCode.BadRequest:
                logger.LogWarning(
                    exception: error.Exception.IfNoneUnsafe(default(Exception)),
                    message: "{ResponseStatus}, {ErrorCode}, {Message}",
                    error.StatusCode,
                    error.ErrorCode.IfNoneUnsafe(default(string)),
                    error.Message);

                error.Inner.Do(
                    err =>
                        logger.Log(err, logLevel: LogLevel.Warning));
                break;
        }
    }
}