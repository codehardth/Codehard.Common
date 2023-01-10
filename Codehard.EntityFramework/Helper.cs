using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Codehard.EntityFramework;

public static class Helper
{
    public static async Task ApplyMigrationsAsync<T>(this IHost host, CancellationToken cancellationToken = default)
        where T : DbContext
    {
        using var scope = host.Services.CreateScope();

        await using var dbContext = scope.ServiceProvider.GetService<T>();

        if (dbContext == null)
        {
            throw new InvalidOperationException($"Unable to resolve '{typeof(T)}' from host.");
        }

        var migrations = await dbContext.Database.GetPendingMigrationsAsync(cancellationToken);

        if (migrations.Any())
        {
            await dbContext.Database.MigrateAsync(CancellationToken.None);
        }

        await dbContext.Database.EnsureCreatedAsync(cancellationToken);
    }
}