using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Codehard.Common.AspNetCore.Attributes;

/// <summary>
/// An action filter attribute that allows the action to execute only in the specified environment.
/// </summary>
public class AllowOnEnvironmentAttribute : ActionFilterAttribute
{
    private readonly string environmentName;

    /// <summary>
    /// Initializes a new instance of the <see cref="AllowOnEnvironmentAttribute"/> class.
    /// </summary>
    /// <param name="environmentName">The name of the environment in which the action is allowed to execute.</param>
    public AllowOnEnvironmentAttribute(string environmentName)
    {
        this.environmentName = environmentName;
    }

    /// <summary>
    /// Called before the action method is executed.
    /// </summary>
    /// <param name="context">The action executing context.</param>
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var environment = context.HttpContext.RequestServices.GetRequiredService<IWebHostEnvironment>();

        if (environment.EnvironmentName != this.environmentName)
        {
            context.Result = new NotFoundResult();

            return;
        }

        base.OnActionExecuting(context);
    }
}