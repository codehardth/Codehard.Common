using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Codehard.Common.AspNetCore.Extensions;

public static class SwaggerExtensions
{
    /// <summary>
    /// Add Swagger document from generated XML if possible.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="setupAction"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public static void AddSwaggerGenWithDocuments(
        this IServiceCollection services,
        Action<SwaggerGenOptions>? setupAction = null)
    {
        var assembly = Assembly.GetCallingAssembly();

        if (assembly == null)
        {
            throw new InvalidOperationException("Caller assembly must not be null.");
        }

        var xmlFilename = $"{assembly.GetName().Name}.xml";

        var filePath = Path.Combine(AppContext.BaseDirectory, xmlFilename);

        services.AddSwaggerGen(options =>
        {
            setupAction?.Invoke(options);

            if (File.Exists(filePath))
            {
                options.IncludeXmlComments(filePath);
            }
        });
    }
}