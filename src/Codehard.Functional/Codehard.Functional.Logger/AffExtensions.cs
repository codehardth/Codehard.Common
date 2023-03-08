namespace Codehard.Functional.Logger;

public static class AffExtensions
{
    /// <summary>
    /// Wrap an async effect inside a logger which is execute before and after the given async effect. 
    /// </summary>
    /// <param name="aff"></param>
    /// <param name="logger"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Aff<T> WithLog<T>(
        this Aff<T> aff,
        ILogger logger)
    {
        return WithLog(
            aff,
            logger,
            LogLevel.Information,
            Some("Executing"),
            Some(fun((T _) => "Executed")));
    }

    /// <summary>
    /// Wrap an async effect inside a logger which is execute before and after the given async effect.
    /// </summary>
    /// <param name="aff"></param>
    /// <param name="logger"></param>
    /// <param name="logLevel"></param>
    /// <param name="executingMessage"></param>
    /// <param name="executedMessage"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Aff<T> WithLog<T>(
        this Aff<T> aff,
        ILogger logger,
        LogLevel logLevel,
        Option<string> executingMessage = default,
        Option<Func<T, string>> executedMessage = default)
    {
        return
            from executingLog in
                Eff(() => executingMessage.IfSome(t => logger.Log(logLevel, t)))
            from res in
                aff.MapFail(err => logger.DoLog(err, LogLevel.Error))
            from executedLog in
                Eff(() => executedMessage.IfSome(f => logger.Log(logLevel, f(res))))
            select res;
    }

    /// <summary>
    /// Wrap an effect inside a logger which is execute before and after the given effect.
    /// </summary>
    /// <param name="eff"></param>
    /// <param name="logger"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Eff<T> WithLog<T>(
        this Eff<T> eff,
        ILogger logger)
    {
        return WithLog(
            eff,
            logger,
            LogLevel.Information,
            Some("Executing"),
            Some(fun((T _) => "Executed")));
    }

    /// <summary>
    /// Wrap an effect inside a logger which is execute before and after the given effect.
    /// </summary>
    /// <param name="eff"></param>
    /// <param name="logger"></param>
    /// <param name="logLevel"></param>
    /// <param name="executingMessage"></param>
    /// <param name="executedMessage"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Eff<T> WithLog<T>(
        this Eff<T> eff,
        ILogger logger,
        LogLevel logLevel,
        Option<string> executingMessage = default,
        Option<Func<T, string>> executedMessage = default)
    {
        return
            from executingLog in
                Eff(() => executingMessage.IfSome(t => logger.Log(logLevel, t)))
            from res in
                eff.MapFail(err => logger.DoLog(err, LogLevel.Error))
            from executedLog in
                Eff(() => executedMessage.IfSome(f => logger.Log(logLevel, f(res))))
            select res;
    }
}