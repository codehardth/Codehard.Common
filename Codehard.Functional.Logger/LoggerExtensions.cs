using LanguageExt.Common;

namespace Codehard.Functional.Logger;

public static class LoggerExtensions
{
    private static Unit Log(this ILogger logger, Option<Error> errorOpt)
    {
        return
            errorOpt.Match(
                Some: error =>
                {
                    Log(logger, error.Inner);

                    return
                        error.Exception.Match(
                            Some: ex => logger.LogError(ex, error.Message),
                            None: () =>
                            {
                                if (string.IsNullOrWhiteSpace(error.Message))
                                {
                                    logger.LogInformation("{code}", error.Code);
                                }
                                else
                                {
                                    logger.LogInformation("{code} : {message}", error.Code, error.Message);
                                }
                            });
                },
                None: unit);
    }

    /// <summary>
    /// Log as error if an error contains exception, otherwise log information if there is a message within an error object.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="error"></param>
    /// <returns></returns>
    public static Unit Log(this ILogger logger, Error error)
    {
        return Log(logger, Some(error));
    }

    /// <summary>
    /// Log error if the Fin is failed and the error contains an exception, otherwise log information with error message.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="fin"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Unit LogIfFail<T>(this ILogger logger, Fin<T> fin)
    {
        return fin.IfFail(err => Log(logger, err));
    }
}