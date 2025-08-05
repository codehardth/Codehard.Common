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

/// <summary>
/// Provides a centralized mechanism for handling exceptions and logging errors in a web API context.
/// Implements the <see cref="IExceptionHandler"/> interface to process and log errors with optional context-specific behavior.
/// </summary>
public class WebApiExceptionHandler : IExceptionHandler
{
    private static WebApiExceptionHandler instance = new();
    
    public static WebApiExceptionHandler Instance => instance;
    
    private WebApiExceptionHandler() { }
    
    public Unit Handle(Error error, ILogger logger, LogLevel logLevel = LogLevel.Information)
    {
        error.Exception.Match(
            Some: ex =>
            {
                if (ex is OperationCanceledException or TaskCanceledException &&
                    logLevel < LogLevel.Error)
                {
                    logger.Log(logLevel, "{Message}", ex.Message);
                }
                else
                {
                    logger.LogError(ex, "{Message}", error.Message);
                }
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

        return unit;
    }
}