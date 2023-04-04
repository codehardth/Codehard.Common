using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Provides extension methods for the IHost interface.
/// </summary>
public static class HostExtensions
{
    /// <summary>
    /// Apply pending migrations if any.
    /// </summary>
    /// <param name="host"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TContext"></typeparam>
    /// <exception cref="InvalidOperationException"></exception>
    public static async Task ApplyMigrationsAsync<TContext>(
        this IHost host,
        CancellationToken cancellationToken = default)
        where TContext : DbContext
    {
        await using var scope = host.Services.CreateAsyncScope();

        await using var dbContext = scope.ServiceProvider.GetService<TContext>();

        if (dbContext == null)
        {
            throw new InvalidOperationException($"Unable to resolve '{typeof(TContext)}' from host.");
        }

        var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync(cancellationToken);

        if (pendingMigrations.Any())
        {
            await dbContext.Database.MigrateAsync(CancellationToken.None);
        }
    }
}