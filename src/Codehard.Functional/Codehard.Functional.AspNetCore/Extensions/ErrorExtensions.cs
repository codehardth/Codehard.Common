using Codehard.Functional.AspNetCore.ActionResults;
using Codehard.Functional.AspNetCore.Errors;

namespace Codehard.Functional.AspNetCore.Extensions;

public static class ErrorExtensions
{
    public static Error Flatten(this Seq<Error> errors, int errorCode, char sep = '\n')
        => Error.New(errorCode, string.Concat(sep, errors.Map(e => e.Message)));
    
    internal static IActionResult ToActionResult(this Error err)
    {
        return
            err switch
            {
                HttpResultError hre => new ErrorWrapperActionResult(hre),
                ActionResultError are => are.ActionResult,
                _ => new ObjectResult(err.Message)
                {
                    StatusCode =
                        Enum.IsDefined(typeof(HttpStatusCode), err.Code)
                            ? err.Code
                            : (int)HttpStatusCode.InternalServerError,
                }
            };
    }
}