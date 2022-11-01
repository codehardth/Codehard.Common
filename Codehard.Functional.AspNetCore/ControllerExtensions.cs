using Codehard.Functional.Logger;

namespace Codehard.Functional.AspNetCore;

public static class ControllerExtensions
{
    private static IActionResult MapToActionResult<T>(HttpStatusCode statusCode, T result)
    {
        return
            result switch
            {
                IActionResult ar => ar,
                Unit => new StatusCodeResult((int)statusCode),
                _ => new ObjectResult(result)
                {
                    StatusCode = (int)statusCode,
                },
            };
    }

    private static IActionResult MapErrorToActionResult(Error err)
    {
        return new ObjectResult(err.Message)
        {
            StatusCode =
                Enum.IsDefined(typeof(HttpStatusCode), err.Code)
                    ? err.Code
                    : (int)HttpStatusCode.InternalServerError,
        };
    }

    /// <summary>
    /// Match a Fin of <typeparamref name="T"/> into IActionResult.
    /// </summary>
    public static IActionResult MatchToResult<T>(
        this Fin<T> fin,
        HttpStatusCode successStatusCode = HttpStatusCode.OK,
        ILogger? logger = default)
    {
        return fin
            .Match(
                res => MapToActionResult(successStatusCode, res),
                err =>
                {
                    logger?.Log(err);

                    return MapErrorToActionResult(err);
                });
    }

    /// <summary>
    /// Match a Fin of Option <typeparamref name="T"/> into IActionResult.
    /// When option is none the NotFound status is returned.
    /// </summary>
    public static IActionResult MatchToResult<T>(
        this Fin<Option<T>> fin,
        HttpStatusCode successStatusCode = HttpStatusCode.OK,
        ILogger? logger = default)
    {
        return fin
            .Match(
                resOpt => resOpt
                    .Match(
                        Some: res => MapToActionResult(successStatusCode, res),
                        None: new NotFoundResult()),
                err =>
                {
                    logger?.Log(err);

                    return MapErrorToActionResult(err);
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