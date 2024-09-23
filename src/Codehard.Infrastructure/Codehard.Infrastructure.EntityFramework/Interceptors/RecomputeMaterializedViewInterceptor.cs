using Codehard.Common.DomainModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Codehard.Infrastructure.EntityFramework.Interceptors;

public class RecomputeMaterializedViewInterceptor<TEvent> : SaveChangesInterceptor
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
            context.ChangeTracker.Entries()
                   .Where(e => e.Entity is IEntity)
                   .Select(e => e.Entity as IEntity)
                   .SelectMany(e => e!.Events);

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
            context.ChangeTracker.Entries().Where(e => e.Entity is IEntity)
                   .Select(e => e.Entity as IEntity)
                   .SelectMany(e => e!.Events);

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