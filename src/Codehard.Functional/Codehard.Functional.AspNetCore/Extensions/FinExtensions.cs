using Codehard.Functional.AspNetCore.Errors;
using Codehard.Functional.Logger;

namespace Codehard.Functional.AspNetCore.Extensions;

public static class FinExtensions
{
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
                res => Commons.MapToActionResult(successStatusCode, res),
                err =>
                {
                    switch (err)
                    {
                        case HttpResultError hre:
                            logger?.Log(hre);
                            return hre.ToActionResult();
                        default:
                            logger?.Log(err);
                            return err.ToActionResult();
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
                        Some: res => Commons.MapToActionResult(successStatusCode, res),
                        None: new NotFoundResult()),
                err =>
                {
                    switch (err)
                    {
                        case HttpResultError hre:
                            logger?.Log(hre);
                            return hre.ToActionResult();
                        default:
                            logger?.Log(err);
                            return err.ToActionResult();
                    }
                });
    }
}