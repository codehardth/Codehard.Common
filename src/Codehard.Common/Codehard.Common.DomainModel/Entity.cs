using System.Runtime.CompilerServices;
using Codehard.Common.DomainModel.Extensions;

namespace Codehard.Common.DomainModel;

/// <summary>
/// Base class for entities with a generic key.
/// </summary>
/// <typeparam name="TKey">The type of the entity's key.</typeparam>
public abstract class Entity<TKey>
    : IEntity<TKey>
    where TKey : IEntityKey
{
    private readonly Action<object, string> lazyLoader;
    private readonly List<IDomainEvent<TKey>> events = new();

    protected Entity()
    {
    }

    protected Entity(Action<object, string> lazyLoader)
    {
        this.lazyLoader = lazyLoader;
    }

    /// <summary>
    /// Gets the unique identifier for the entity.
    /// </summary>
    public abstract TKey Id { get; protected init; }

    /// <summary>
    /// Gets a read-only collection of notifications associated with the entity.
    /// </summary>
    public IReadOnlyCollection<IDomainEvent> Events => this.events.AsReadOnly();

    /// <summary>
    /// Loads a navigation property of a related entity.
    /// </summary>
    /// <typeparam name="TRelated">The type of the related entity.</typeparam>
    /// <param name="refInstance">The reference to the related entity.</param>
    /// <param name="navigationName">The name of the navigation property (optional).</param>
    /// <returns>The loaded related entity.</returns>
    protected TRelated LoadNavigationProperty<TRelated>(
        TRelated refInstance,
        [CallerMemberName] string navigationName = default!)
        where TRelated : class
        => this.lazyLoader.Load(this, ref refInstance, navigationName);

    /// <summary>
    /// Loads a collection of related entities for a navigation property.
    /// </summary>
    /// <typeparam name="TRelated">The type of the related entities.</typeparam>
    /// <param name="collection">The collection of related entities.</param>
    /// <param name="navigationName">The name of the navigation property (optional).</param>
    /// <returns>The loaded collection of related entities.</returns>
    protected IReadOnlyCollection<TRelated> LoadNavigationPropertyCollection<TRelated>(
        List<TRelated> collection,
        [CallerMemberName] string navigationName = default!)
        => this.lazyLoader.Load(this, ref collection, navigationName);

    /// <summary>
    /// Determines whether the current entity is equal to another object.
    /// </summary>
    /// <param name="o">The object to compare with the current entity.</param>
    /// <returns>True if the objects are equal; otherwise, false.</returns>
    public override bool Equals(object? o)
    {
        return o is Entity<TKey> e && e.Id.Equals(this.Id);
    }

    /// <summary>
    /// Gets the hash code for the current entity.
    /// </summary>
    /// <returns>The hash code for the entity's unique identifier.</returns>
    public override int GetHashCode()
    {
        return this.Id.GetHashCode();
    }

    /// <summary>
    /// Adds a domain event to the entity's list of events.
    /// </summary>
    /// <param name="event">The domain event to add.</param>
    protected void AddDomainEvent(IDomainEvent<TKey> @event)
    {
        this.events.Add(@event);
    }

    /// <summary>
    /// Removes a domain event from the entity's list of events.
    /// </summary>
    /// <param name="event">The domain event to remove.</param>
    protected void RemoveDomainEvent(IDomainEvent<TKey> @event)
    {
        this.events.Remove(@event);
    }
}