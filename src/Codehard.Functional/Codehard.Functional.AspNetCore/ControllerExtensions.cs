using Codehard.Functional.Logger;

namespace Codehard.Functional.AspNetCore;

/// <summary>
/// Provides extension methods for converting functional results into ASP.NET Core action results.
/// </summary>
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
        return
            err switch
            {
                HttpResultError hre => new ErrorWrapperActionResult(hre),
                _ => new ErrorWrapperActionResult(
                    HttpResultError.New(
                        Enum.IsDefined(typeof(HttpStatusCode), err.Code)
                            ? (HttpStatusCode)err.Code
                            : HttpStatusCode.InternalServerError,
                        err.Message,
                        error: err)),
            };
    }

    /// <summary>
    /// Matches a Fin of <typeparamref name="T"/> into an <see cref="IActionResult"/>.
    /// </summary>
    /// <typeparam name="T">The type of the successful result.</typeparam>
    /// <param name="fin">The <see cref="Fin{T}"/> to match.</param>
    /// <param name="successStatusCode">The HTTP status code to return on success.</param>
    /// <param name="logger">An optional <see cref="ILogger"/> instance used for logging.</param>
    /// <returns>An <see cref="IActionResult"/> that represents the result of matching <paramref name="fin"/>.</returns>
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
                    switch (err)
                    {
                        case HttpResultError hre:
                            logger?.Log(hre);
                            return MapErrorToActionResult(hre);
                        default:
                            logger?.Log(err);
                            return MapErrorToActionResult(err);
                    }
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
                    switch (err)
                    {
                        case HttpResultError hre:
                            logger?.Log(hre);
                            return MapErrorToActionResult(hre);
                        default:
                            logger?.Log(err);
                            return MapErrorToActionResult(err);
                    }
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
    
    /// <summary>
    /// Run the effect into IActionResult in a synchronous manner.
    /// </summary>
    public static IActionResult RunToResult<T>(
        this Eff<T> eff,
        HttpStatusCode successStatusCode = HttpStatusCode.OK,
        ILogger? logger = default)
    {
        return eff
            .Run()
            .MatchToResult(
                successStatusCode: successStatusCode,
                logger: logger);
    }
}