using Codehard.Common.DomainModel;
using Codehard.Infrastructure.EntityFramework.Processors;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Codehard.Infrastructure.EntityFramework.Interceptors;

public class DomainEventInterceptor<TEvent> : SaveChangesInterceptor
    where TEvent : IDomainEvent
{
    private readonly IDomainEventProcessor[] processors;

    public DomainEventInterceptor(params IDomainEventProcessor[] processors)
    {
        this.processors = processors;
    }

    public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
    {
        var context = eventData.Context;

        if (context is null)
        {
            return result;
        }

        var entries =
            context.ChangeTracker.Entries()
                   .Where(e => e.Entity is IEntity)
                   .Select(e => e.Entity as IEntity);

        var expectedEntries =
            entries.Where(e => e!.Events.Any(ev => ev.GetType() == typeof(TEvent)));

        if (!expectedEntries.Any())
        {
            return result;
        }

        using var transaction = context.Database.BeginTransaction();

        try
        {
            foreach (var processor in this.processors)
            {
                processor.Process(context, expectedEntries);
            }

            transaction.Commit();
        }
        catch (Exception)
        {
            transaction.Rollback();

            throw;
        }

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

        var entries =
            context.ChangeTracker.Entries()
                   .Where(e => e.Entity is IEntity)
                   .Select(e => e.Entity as IEntity);

        var expectedEntries =
            entries.Where(e => e!.Events.Any(ev => ev.GetType() == typeof(TEvent)));

        if (!expectedEntries.Any())
        {
            return result;
        }

        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            foreach (var processor in this.processors)
            {
                await processor.ProcessAsync(context, expectedEntries, cancellationToken);
            }

            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);

            throw;
        }

        return result;
    }
}