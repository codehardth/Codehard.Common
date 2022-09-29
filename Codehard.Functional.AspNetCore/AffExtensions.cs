namespace Codehard.Functional.AspNetCore;

public static class AffExtensions
{
    /// <summary>
    /// Run the async effect into IActionResult in an asynchronous manner.
    /// </summary>
    /// <param name="aff"></param>
    /// <param name="responseFunc"></param>
    /// <param name="statusCode"></param>
    /// <param name="logger"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Task<IActionResult> RunToResponseAsync<T>(
        this Aff<T> aff,
        Func<T, IActionResult> responseFunc,
        HttpStatusCode statusCode = HttpStatusCode.OK,
        ILogger? logger = default)
    {
        return aff.Run()
            .Do(fin => logger?.LogIfFail(fin))
            .Map(fin => RunToResponse(fin, responseFunc, statusCode))
            .AsTask();
    }

    /// <summary>
    /// Run the async effect into IActionResult in an asynchronous manner.
    /// </summary>
    /// <param name="aff"></param>
    /// <param name="statusCode"></param>
    /// <param name="logger"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Task<IActionResult> RunToResponseAsync<T>(
        this Aff<T> aff,
        HttpStatusCode statusCode = HttpStatusCode.OK,
        ILogger? logger = default)
    {
        return aff.Run()
            .Do(fin => logger?.LogIfFail(fin))
            .Map(fin => RunToResponse(fin, statusCode))
            .AsTask();
    }

    /// <summary>
    /// Run a Fin into IActionResult
    /// </summary>
    /// <param name="fin"></param>
    /// <param name="successStatusCode"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IActionResult RunToResponse<T>(
        this Fin<T> fin,
        HttpStatusCode successStatusCode = HttpStatusCode.OK) =>
        RunToResponse(
            fin,
            t => new ObjectResult(t)
            {
                StatusCode = (int) successStatusCode,
            }, successStatusCode);

    /// <summary>
    /// Run a Fin into IActionResult
    /// </summary>
    /// <param name="fin"></param>
    /// <param name="responseFunc"></param>
    /// <param name="successStatusCode"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IActionResult RunToResponse<T>(
        this Fin<T> fin,
        Func<T, IActionResult> responseFunc,
        HttpStatusCode successStatusCode = HttpStatusCode.OK) =>
        fin.Match(
            t => t switch
            {
                IActionResult ar => ar,
                Unit => new StatusCodeResult((int) successStatusCode),
                _ => responseFunc(t),
            },
            e => new ObjectResult(e.Message)
            {
                StatusCode =
                    Enum.IsDefined(typeof(HttpStatusCode), e.Code)
                        ? e.Code
                        : (int) HttpStatusCode.InternalServerError,
            });
}