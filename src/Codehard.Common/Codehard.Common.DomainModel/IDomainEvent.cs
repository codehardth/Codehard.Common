namespace Codehard.Common.DomainModel;

/// <summary>
/// Represents an interface for domain events, providing information about when the event occurred.
/// </summary>
public interface IDomainEvent
{
    /// <summary>
    /// Gets the timestamp indicating when the domain event occurred.
    /// </summary>
    DateTimeOffset Timestamp { get; }
}

/// <summary>
/// Generic interface for domain event notifications with a key of type TKey.
/// </summary>
/// <typeparam name="TKey">The type of the entity's key associated with the domain event.</typeparam>
public interface IDomainEvent<out TKey> : IDomainEvent
    where TKey : struct
{
    /// <summary>
    /// Gets the unique identifier of the entity associated with the domain event.
    /// </summary>
    TKey Id { get; }
}