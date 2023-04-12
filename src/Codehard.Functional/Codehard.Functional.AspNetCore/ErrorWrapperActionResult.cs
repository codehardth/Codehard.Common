using System.Dynamic;

namespace Codehard.Functional.AspNetCore;

/// <summary>
/// An <see cref="IActionResult"/> that wraps an <see cref="HttpResultError"/> object and
/// returns it as a result, including additional error information in the response body.
/// </summary>
public class ErrorWrapperActionResult : IActionResult
{
    /// <summary>
    /// Gets the <see cref="HttpResultError"/> object that is being wrapped by this action result.
    /// </summary>
    public HttpResultError Error { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ErrorWrapperActionResult"/> class with the
    /// specified <see cref="HttpResultError"/> object.
    /// </summary>
    /// <param name="error">The <see cref="HttpResultError"/> object to be returned as the result of the action.</param>
    /// <exception cref="ArgumentNullException">Thrown if the <paramref name="error"/> parameter is <c>null</c>.</exception>
    public ErrorWrapperActionResult(HttpResultError error)
    {
        this.Error = error ?? throw new ArgumentNullException(nameof(error));
    }

    /// <summary>
    /// Executes the result of the action by setting the response headers and body based on the
    /// <see cref="HttpResultError"/> object being wrapped.
    /// </summary>
    /// <param name="context">The context in which the result is executed.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task ExecuteResultAsync(ActionContext context)
    {
        Error.ErrorCode.IfSome(errCode =>
            context.HttpContext.Response.Headers.Add("x-error-code", errCode));

        await Error.Data
            .Map(
                data =>
                    data switch
                    {
                        ObjectResult objectResult => AddErrorInfo(objectResult),
                        StatusCodeResult statusCodeResult => 
                            new ObjectResult(
                                new
                                {
                                    ErrorCode = Error.ErrorCode.IfNoneUnsafe(default(string)),
                                    ErrorMessage = Error.Message,
                                })
                            {
                                StatusCode = statusCodeResult.StatusCode
                            },
                        IActionResult ar => ar,
                        _ => AddErrorInfo(new ObjectResult(data)
                        {
                            StatusCode = Error.Code
                        })
                    })
            .IfSomeAsync(ar => ar.ExecuteResultAsync(context));

        ObjectResult AddErrorInfo(ObjectResult objectResult)
        {
            IDictionary<string, object?> expando = new ExpandoObject();

            if (objectResult.Value != null)
            {
                foreach (var propertyInfo in objectResult.Value!.GetType().GetProperties())
                {
                    var currentValue = propertyInfo.GetValue(objectResult.Value);
                    expando.Add(propertyInfo.Name, currentValue);
                }
            }

            expando["ErrorInfo"] =
                new
                {
                    ErrorCode = Error.ErrorCode.IfNoneUnsafe(default(string)),
                    ErrorMessage = Error.Message,
                };

            return new ObjectResult(expando)
            {
                StatusCode = Error.Code
            };
        }
    }
}