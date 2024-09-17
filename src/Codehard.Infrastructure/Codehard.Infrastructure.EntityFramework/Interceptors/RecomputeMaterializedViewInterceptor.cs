using Codehard.Common.DomainModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Codehard.Infrastructure.EntityFramework.Interceptors;

public class RecomputeMaterializedViewInterceptor<TEntity, TEvent> : SaveChangesInterceptor
    where TEntity : class, IEntity
    where TEvent : IDomainEvent
{
    public RecomputeMaterializedViewInterceptor(string materializedViewRefreshQuery)
    {
        this.MaterializedViewRefreshQuery = materializedViewRefreshQuery;
    }

    public string MaterializedViewRefreshQuery { get; }

    public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
    {
        var context = eventData.Context;

        if (context is null)
        {
            return result;
        }

        var events =
            context.ChangeTracker.Entries<TEntity>().SelectMany(e => e.Entity.Events);

        if (events.All(e => e.GetType() != typeof(TEvent)))
        {
            return result;
        }

        this.RecomputeAsync(context).ConfigureAwait(false);

        return result;
    }

    public override async ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData,
        int result,
        CancellationToken cancellationToken = default)
    {
        var context = eventData.Context;

        if (context is null)
        {
            return result;
        }

        var events =
            context.ChangeTracker.Entries<TEntity>().SelectMany(e => e.Entity.Events);

        if (events.All(e => e.GetType() != typeof(TEvent)))
        {
            return result;
        }

        await this.RecomputeAsync(context).ConfigureAwait(false);

        return result;
    }

    protected virtual Task RecomputeAsync(DbContext dbContext)
    {
        return dbContext.Database.ExecuteSqlRawAsync(this.MaterializedViewRefreshQuery);
    }
}