namespace Codehard.Common.DomainModel;

/// <summary>
/// An abstract base class for immutable entities with a generic key.
/// </summary>
/// <typeparam name="TKey">The type of the entity's key.</typeparam>
public abstract record ImmutableEntity<TKey>(TKey Id)
    : IEntity<TKey>
    where TKey : struct
{
    private readonly List<IDomainEvent<TKey>> events = new();

    /// <summary>
    /// Gets a read-only collection of domain events associated with the entity.
    /// </summary>
    public IReadOnlyCollection<IDomainEvent> Events => this.events.AsReadOnly();

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
