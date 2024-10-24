using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Codehard.Functional.AspNetCore;

/// <summary>
/// An action filter that logs errors occurring during the execution of an action.
/// </summary>
public class ErrorWrapperActionResultLoggingFilter : IAsyncActionFilter
{
    private readonly ILogger<ErrorWrapperActionResultLoggingFilter> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ErrorWrapperActionResultLoggingFilter"/> class.
    /// </summary>
    /// <param name="logger">The logger to log error information.</param>
    public ErrorWrapperActionResultLoggingFilter(ILogger<ErrorWrapperActionResultLoggingFilter> logger)
    {
        this.logger = logger;
    }
    
    /// <summary>
    /// Executes the action and logs any errors that occur during the execution.
    /// </summary>
    /// <param name="context">The context in which the action is executed.</param>
    /// <param name="next">The delegate to execute the next action filter or action.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var actionExecutedContext = await next();
        
        await OnActionExecutedAsync(actionExecutedContext);
    }
    
    private Task OnActionExecutedAsync(ActionExecutedContext context)
    {
        var traceId = context.HttpContext.TraceIdentifier;

        switch (context.Result)
        {
            case ErrorWrapperActionResult ewar:
                LogHttpResultError(ewar.Error);
                break;
            case IStatusCodeActionResult statusCodeResult:
            {
                if(statusCodeResult.StatusCode == StatusCodes.Status500InternalServerError)
                {
                    LogError(context.Exception);
                }

                break;
            }
        }

        return Task.CompletedTask;
        
        void LogHttpResultError(HttpResultError error)
        {
            this.logger.LogError(
                message: "TraceId: {TraceId}, {Path}, {Query}, {Method}, {ResponseStatus}, {ErrorCode}",
                traceId,
                Sanitize(context.HttpContext.Request.Path),
                Sanitize(context.HttpContext.Request.QueryString.Value),
                Sanitize(context.HttpContext.Request.Method),
                error.StatusCode,
                error.ErrorCode.IfNoneUnsafe(default(string)));

            LogErrorOpt(error.Inner);
        }
        
        void LogErrorOpt(Option<Error> errorOpt)
        {
            errorOpt.Iter(
                Some: error =>
                {
                    LogError(
                        exception: error.Exception.IfNoneUnsafe(default(Exception)));
                    
                    LogErrorOpt(error.Inner);
                });
        }

        void LogError(Exception? exception)
        {
            this.logger.LogError(
                exception: exception,
                message: "TraceId: {TraceId}, {Path}, {Query}, {Method}, {ResponseStatus}",
                traceId,
                Sanitize(context.HttpContext.Request.Path),
                Sanitize(context.HttpContext.Request.QueryString.Value),
                Sanitize(context.HttpContext.Request.Method),
                HttpStatusCode.InternalServerError);
        }
    }
    
    private static string Sanitize(string? input)
    {
        return new string(
            input
                ?.Replace(Environment.NewLine, "")
                .Replace("\n", "")
                .Replace("\r", ""));
    }
}