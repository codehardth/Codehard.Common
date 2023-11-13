using System.Collections;
using System.Collections.Concurrent;
using System.Reflection;
using Codehard.Common.DomainModel;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Codehard.Infrastructure.EntityFramework.Interceptors;

/// <summary>
/// Represents a delegate for asynchronously publishing domain events.
/// </summary>
/// <param name="domainEvent">The domain event to be published.</param>
/// <returns>An asynchronous task representing the publishing operation.</returns>
public delegate Task PublishDomainEventDelegate(IDomainEvent domainEvent);

internal class DomainEventPublisherInterceptor : SaveChangesInterceptor
{
    /// <summary>
    /// A cache to store reflection data related to the "events" field in entity types.
    /// </summary>
    private static readonly ConcurrentDictionary<Type, FieldInfo> FieldInfoCache = new();

    private readonly PublishDomainEventDelegate? publisher;

    /// <summary>
    /// </summary>
    /// <param name="publisher">Sets a publisher function that has higher priority over publisher event from <see cref="IDomainEventDbContext"/></param>
    public DomainEventPublisherInterceptor(PublishDomainEventDelegate? publisher = default)
    {
        this.publisher = publisher;
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        var context = eventData.Context;

        if (context is null)
        {
            return new InterceptionResult<int>();
        }

        var publisherFunc =
            this.publisher ??
            (context is IDomainEventDbContext domainEventDbContext
                ? domainEventDbContext.PublishDomainEventAsync
                : null);

        if (publisherFunc is null)
        {
            return new InterceptionResult<int>();
        }

        // Retrieve domain entities from the change tracker.
        var domainEntities =
            context.ChangeTracker
                .Entries()
                .Where(e => e.Entity.GetType().IsAssignableTo(typeof(IEntity)))
                .ToArray();

        // Extract and publish domain events.
        var events = domainEntities.SelectMany(e => ((IEntity)e.Entity).Events);

        foreach (var @event in events)
        {
            await publisherFunc(@event);
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

        return new InterceptionResult<int>();

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