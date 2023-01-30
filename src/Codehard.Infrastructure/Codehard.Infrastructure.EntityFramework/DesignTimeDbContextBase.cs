using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Codehard.Infrastructure.EntityFramework;

/// <summary>
/// A base class for design time DbContext factory, with pre-configured options and routine.
/// </summary>
/// <typeparam name="TContext"></typeparam>
public abstract class DesignTimeDbContextBase<TContext> : IDesignTimeDbContextFactory<TContext>
    where TContext : DbContext
{
    public TContext CreateDbContext(string[] args)
    {
        var options = this.GetMigrationOptions(args);

        var builder = new DbContextOptionsBuilder<TContext>();

        this.ConfigureOptions(builder, options);

        return (TContext)Activator.CreateInstance(typeof(TContext), builder.Options)!;
    }

    /// <summary>
    /// Gets a migration options instance.
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    protected abstract MigrationOptions GetMigrationOptions(string[] args);

    /// <summary>
    /// Configure additional options for DbContext.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="options"></param>
    protected abstract void ConfigureOptions(
        DbContextOptionsBuilder<TContext> builder,
        MigrationOptions options);
}