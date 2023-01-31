using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Codehard.Common.AspNetCore.Attributes;

public class AllowOnEnvironmentAttribute : ActionFilterAttribute
{
    private readonly string environmentName;

    public AllowOnEnvironmentAttribute(string environmentName)
    {
        this.environmentName = environmentName;
    }

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