using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Codehard.Common.Asp.Attributes;

/// <summary>
/// A filter attribute that prevent the execution of specific action when running in specific environment.
/// </summary>
public class DisallowOnEnvironmentAttribute : ActionFilterAttribute
{
    private readonly string environmentName;

    public DisallowOnEnvironmentAttribute(string environmentName)
    {
        this.environmentName = environmentName;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var environment = context.HttpContext.RequestServices.GetRequiredService<IWebHostEnvironment>();

        if (environment.EnvironmentName == this.environmentName)
        {
            context.Result = new NotFoundResult();

            return;
        }

        base.OnActionExecuting(context);
    }
}