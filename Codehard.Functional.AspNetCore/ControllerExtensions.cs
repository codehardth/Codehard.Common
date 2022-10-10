namespace Codehard.Functional.AspNetCore;

public static class ControllerExtensions
{
    /// <summary>
    /// Match a Fin into IActionResult
    /// </summary>
    /// <param name="fin"></param>
    /// <param name="successStatusCode"></param>
    /// <param name="logger"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IActionResult MatchToResult<T>(
        this Fin<T> fin,
        HttpStatusCode successStatusCode = HttpStatusCode.OK,
        ILogger? logger = default)
    {
        return fin
            .Match(
                t => t switch
                {
                    IActionResult ar => ar,
                    Unit => new StatusCodeResult((int)successStatusCode),
                    _ => new ObjectResult(t)
                    {
                        StatusCode = (int)successStatusCode,
                    },
                },
                err =>
                {
                    logger?.Log(err);

                    return new ObjectResult(err.Message)
                    {
                        StatusCode =
                            Enum.IsDefined(typeof(HttpStatusCode), err.Code)
                                ? err.Code
                                : (int)HttpStatusCode.InternalServerError,
                    };
                });
    }

    /// <summary>
    /// Run the async effect into IActionResult in an asynchronous manner.
    /// </summary>
    /// <param name="aff"></param>
    /// <param name="successStatusCode"></param>
    /// <param name="logger"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static ValueTask<IActionResult> RunToResultAsync<T>(
        this Aff<T> aff,
        HttpStatusCode successStatusCode = HttpStatusCode.OK,
        ILogger? logger = default)
    {
        return aff
            .Run()
            .Map(fin => fin
                .MatchToResult(
                    successStatusCode: successStatusCode,
                    logger: logger));
    }
}