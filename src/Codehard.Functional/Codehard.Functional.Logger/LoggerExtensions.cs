using LanguageExt.Common;

namespace Codehard.Functional.Logger;

public static class LoggerExtensions
{
    private static Unit Log(this ILogger logger, Option<Error> errorOpt, LogLevel logLevel = LogLevel.Information)
    {
        return
            errorOpt.Match(
                Some: error =>
                {
                    Log(logger, error.Inner, logLevel);

                    return
                        error.Exception.Match(
                            Some: ex => logger.LogError(ex, error.Message),
                            None: () =>
                            {
                                if (string.IsNullOrWhiteSpace(error.Message))
                                {
                                    logger.Log(logLevel, "{code}", error.Code);
                                }
                                else
                                {
                                    logger.Log(logLevel, "{code} : {message}", error.Code, error.Message);
                                }
                            });
                },
                None: unit);
    }

    /// <summary>
    /// Log a message then returns a unit.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="message"></param>
    /// <param name="logLevel"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public static Unit Log(
        this ILogger logger,
        string message,
        LogLevel logLevel = LogLevel.Information,
        params object?[] args)
    {
        logger.Log(logLevel, message, args);

        return unit;
    }

    /// <summary>
    /// Log as error if an error contains exception, otherwise log information if there is a message within an error object.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="error"></param>
    /// <param name="logLevel"></param>
    /// <returns></returns>
    public static Error Log(this ILogger logger, Error error, LogLevel logLevel = LogLevel.Information)
    {
        Log(logger, Some(error), logLevel);

        return error;
    }

    /// <summary>
    /// Log error if the Fin is failed and the error contains an exception, otherwise log information with error message.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="fin"></param>
    /// <param name="logLevel"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Fin<T> LogIfFail<T>(this ILogger logger, Fin<T> fin, LogLevel logLevel = LogLevel.Information)
    {
        fin.IfFail(err => Log(logger, err, logLevel));

        return fin;
    }
}