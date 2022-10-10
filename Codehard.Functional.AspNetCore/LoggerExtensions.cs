namespace Codehard.Functional.AspNetCore;

public static class LoggerExtensions
{
    public static Unit Log(this ILogger logger, Error error)
    {
        return error.Exception.Match(
            Some: ex => logger?.LogError(ex, error.Message),
            None: () =>
            {
                if (!string.IsNullOrWhiteSpace(error.Message))
                {
                    logger?.LogInformation(error.Message);
                }
            });
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