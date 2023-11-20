using System.Collections;
using System.Collections.Concurrent;
using System.Reflection;
using Codehard.Common.DomainModel;
using Microsoft.EntityFrameworkCore;
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

    public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
    {
        var context = eventData.Context;

        if (context is null)
        {
            return result;
        }

        var publisherFunc = GetPublisherFunction(context);

        if (publisherFunc is null)
        {
            return result;
        }

        var entities = RetrieveDomainEntities(context);
        var events = RetrieveDomainEvents(entities);

        foreach (var @event in events)
        {
            publisherFunc(@event).Wait();
        }

        ClearExistingEvents(entities);

        return result;
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

        var publisherFunc = GetPublisherFunction(context);

        if (publisherFunc is null)
        {
            return result;
        }

        var entities = RetrieveDomainEntities(context);
        var events = RetrieveDomainEvents(entities);

        foreach (var @event in events)
        {
            await publisherFunc(@event);
        }

        ClearExistingEvents(entities);

        return result;
    }

    private PublishDomainEventDelegate? GetPublisherFunction(DbContext context)
    {
        var publisherFunc =
            this.publisher ??
            (context is IDomainEventDbContext domainEventDbContext
                ? domainEventDbContext.PublishDomainEventAsync
                : null);

        return publisherFunc;
    }

    private static IEntity[] RetrieveDomainEntities(DbContext context)
    {
        // Retrieve domain entities from the change tracker.
        var domainEntities =
            context.ChangeTracker
                .Entries()
                .Select(e => e.Entity)
                .OfType<IEntity>()
                .ToArray();

        return domainEntities;
    }

    private static IEnumerable<IDomainEvent> RetrieveDomainEvents(IEnumerable<IEntity> entities)
    {
        return entities.SelectMany(e => e.Events).OrderBy(e => e.Timestamp);
    }

    private static void ClearExistingEvents(IEnumerable<IEntity> entities)
    {
        // Clear all existing events
        foreach (var entity in entities)
        {
            var type = entity.GetType();
            var eventsFieldInfo = GetEventsFieldInfo(type);

            if (eventsFieldInfo.GetValue(entity) is ICollection collection)
            {
                switch (collection)
                {
                    case Array arr:
                    {
                        Array.Clear(arr);

                        break;
                    }
                    case IList list:
                    {
                        list.Clear();

                        break;
                    }
                    case IDictionary dictionary:
                    {
                        dictionary.Clear();

                        break;
                    }
                    default:
                        var instance = InstanceActivatorFactory.Instance.CreateInstance(eventsFieldInfo.FieldType);
                        eventsFieldInfo.SetValue(entity, instance);

                        break;
                }
            }
        }
    }

    private static FieldInfo GetEventsFieldInfo(Type type)
    {
        if (FieldInfoCache.TryGetValue(type, out var fi))
        {
            return fi;
        }

        var fields = TraverseBackingFields(type);

        var fieldInfo =
            fields.FirstOrDefault(f =>
                f.Name is "events" or "_events" && f.FieldType.IsAssignableTo(typeof(ICollection)))
            ?? throw new FieldAccessException(
                "Unable to find a backing field 'events' or '_events' (if it is implemented, make sure it is assignable to ICollection and is a non-public field of an instance).");

        FieldInfoCache.AddOrUpdate(
            type,
            fieldInfo,
            (_, _) => fieldInfo);

        return fieldInfo;
    }

    private static IEnumerable<FieldInfo> TraverseBackingFields(Type type)
    {
        var currentType = type;

        while (currentType is not null)
        {
            var fields = currentType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic);

            foreach (var field in fields)
            {
                yield return field;
            }

            currentType = currentType.BaseType;
        }
    }
}