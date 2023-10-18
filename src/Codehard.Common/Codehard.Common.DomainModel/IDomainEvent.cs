namespace Codehard.Common.DomainModel;

/// <summary>
/// Marker interface for domain event notifications.
/// </summary>
public interface IDomainEvent
{
}

/// <summary>
/// Generic interface for domain event notifications with a key of type TKey.
/// </summary>
/// <typeparam name="TKey">The type of the entity's key associated with the domain event.</typeparam>
public interface IDomainEvent<TKey> : IDomainEvent
    where TKey : IEntityKey
{
    /// <summary>
    /// Gets the unique identifier of the entity associated with the domain event.
    /// </summary>
    TKey Id { get; }
}