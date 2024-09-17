using Codehard.Common.DomainModel;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Codehard.Infrastructure.EntityFramework.Interceptors;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TResult"></typeparam>
public delegate TResult MaterializedEntityTransformerDelegate<in TEntity, out TResult>(TEntity entity)
    where TEntity : class, IAggregateRoot;

/// <summary>
/// 
/// </summary>
internal class EntityToMaterializedViewInterceptor<TEntity, TResult>
    : SaveChangesInterceptor where TEntity : class, IAggregateRoot where TResult : class
{
    private readonly MaterializedEntityTransformerDelegate<TEntity, TResult> transformer;

    internal EntityToMaterializedViewInterceptor(MaterializedEntityTransformerDelegate<TEntity, TResult> transformer)
    {
        this.transformer = transformer;
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        var context = eventData.Context;

        if (context is null)
        {
            return result;
        }

        var changeTracker = context.ChangeTracker;

        if (!changeTracker.HasChanges())
        {
            return result;
        }

        var entries = changeTracker.Entries<TEntity>();

        var results =
            entries.Select(e => e.Entity).Select(e => this.transformer(e)).ToArray();

        context.Set<TResult>().AddRange(results);

        return result;
    }
}