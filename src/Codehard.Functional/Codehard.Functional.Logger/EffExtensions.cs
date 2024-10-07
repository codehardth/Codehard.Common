namespace Codehard.Functional.Logger;

/// <summary>
/// Provides extension methods for wrapping asynchronous effects and effects inside a logger.
/// </summary>
public static class EffExtensions
{
    /// <summary>
    /// Wrap an async effect inside a logger which is executed before and after the given async effect. 
    /// </summary>
    /// <param name="effect"></param>
    /// <param name="logger"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Eff<T> WithLog<T>(
        this Eff<T> effect,
        ILogger logger)
    {
        return WithLog(
            effect,
            logger,
            LogLevel.Information,
            Some("Executing"),
            Some(fun((T _) => "Executed")));
    }

    /// <summary>
    /// Wrap an effect inside a logger which is executed before and after the given effect.
    /// </summary>
    /// <param name="effect"></param>
    /// <param name="logger"></param>
    /// <param name="logLevel"></param>
    /// <param name="executingMessage"></param>
    /// <param name="executedMessage"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Eff<T> WithLog<T>(
        this Eff<T> effect,
        ILogger logger,
        LogLevel logLevel,
        Option<string> executingMessage = default,
        Option<Func<T, string>> executedMessage = default)
    {
        return
            from executingLog in
                liftEff(() => executingMessage.IfSome(t => logger.Log(logLevel, t)))
            from res in
                effect.MapFail(err => logger.DoLog(err, LogLevel.Error))
            from executedLog in
                liftEff(() => executedMessage.IfSome(f => logger.Log(logLevel, f(res))))
            select res;
    }
}