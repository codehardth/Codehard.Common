using LanguageExt.Common;

namespace Codehard.Functional.Logger;

/// <summary>
/// Provides a default implementation for handling exceptions in a logging context.
/// This class is intended to standardize how exceptions and errors are logged
/// based on the provided parameters such as the error details, logger instance,
/// and log level.
/// </summary>
public class DefaultExceptionHandler : IExceptionHandler
{
    private static DefaultExceptionHandler instance = new DefaultExceptionHandler();
    
    public static DefaultExceptionHandler Instance => instance;
    
    private DefaultExceptionHandler() { }
    
    public Unit Handle(Error error, ILogger logger, LogLevel logLevel = LogLevel.Information)
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

        return unit;
    }
}