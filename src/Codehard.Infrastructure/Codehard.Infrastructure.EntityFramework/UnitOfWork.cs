using System.Collections;
using System.Collections.Concurrent;
using System.Reflection;
using Codehard.Common.DomainModel;
using Microsoft.EntityFrameworkCore;

namespace Codehard.Infrastructure.EntityFramework;

public abstract class UnitOfWork
{
    /// <summary>
    /// A cache to store reflection data related to the "events" field in entity types.
    /// </summary>
    private static readonly ConcurrentDictionary<Type, FieldInfo> FieldInfoCache = new();

    /// <summary>
    /// The database context used for interacting with the underlying database and tracking changes.
    /// </summary>
    protected readonly DbContext DbContext;

    /// <summary>
    /// Initializes a new instance of the UnitOfWork class with the provided database context.
    /// </summary>
    /// <param name="dbContext">The database context to be used for managing database transactions and changes.</param>
    protected UnitOfWork(DbContext dbContext)
    {
        this.DbContext = dbContext;
    }

    /// <summary>
    /// Asynchronous method to publish a domain event.
    /// Derived classes should implement this method to handle the actual publishing of domain events.
    /// </summary>
    /// <param name="domainEvent">The domain event to be published.</param>
    /// <returns>An asynchronous task representing the operation.</returns>
    protected abstract Task PublishDomainEventAsync(IDomainEvent domainEvent);

    /// <summary>
    /// Asynchronously saves changes to the database, publishes domain events, and clears existing events.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation (optional).</param>
    /// <returns>An asynchronous task representing the save operation.</returns>
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Retrieve domain entities from the change tracker.
        var domainEntities =
            this.DbContext.ChangeTracker
                .Entries()
                .Where(e => e.Entity.GetType().IsAssignableTo(typeof(IEntity)))
                .ToArray();

        // Extract and publish domain events.
        var events = domainEntities.SelectMany(e => ((IEntity)e.Entity).Events);

        foreach (var @event in events)
        {
            await this.PublishDomainEventAsync(@event);
        }

        // Clear all existing events
        foreach (var entityEntry in domainEntities)
        {
            var type = entityEntry.Entity.GetType();
            var eventsFieldInfo = TryGetEventsFieldInfo(type);

            if (eventsFieldInfo?.GetValue(entityEntry.Entity) is IList list)
            {
                list.Clear();
            }
        }

        await this.DbContext.SaveChangesAsync(cancellationToken);

        static FieldInfo? TryGetEventsFieldInfo(Type type)
        {
            if (FieldInfoCache.TryGetValue(type, out var fi))
            {
                return fi;
            }

            var baseType = TraverseBaseEntityType(type);

            if (baseType is null)
            {
                return default;
            }

            var fieldInfo =
                baseType.GetField("events", BindingFlags.Instance | BindingFlags.NonPublic)
                ?? throw new FieldAccessException();

            FieldInfoCache.AddOrUpdate(
                type,
                fieldInfo,
                (_, _) => fieldInfo);

            return fieldInfo;
        }

        static Type? TraverseBaseEntityType(Type? type)
        {
            while (true)
            {
                var baseType = type?.BaseType;

                if (baseType is null)
                {
                    return null;
                }

                if (baseType.IsGenericType && baseType.GetGenericTypeDefinition() == typeof(Entity<>))
                {
                    return baseType;
                }

                type = baseType;
            }
        }
    }
}