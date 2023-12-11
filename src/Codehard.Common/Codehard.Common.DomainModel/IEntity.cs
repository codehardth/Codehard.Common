namespace Codehard.Common.DomainModel;

[Obsolete]
public interface IEntityKey
{
}

/// <summary>
/// Generic interface for entities with a key of type TKey.
/// </summary>
/// <typeparam name="TKey">The type of the entity's key.</typeparam>
public interface IEntity<TKey> : IEntity
    where TKey : struct
{
    /// <summary>
    /// Gets the unique identifier for the entity.
    /// </summary>
    TKey Id { get; }
}

/// <summary>
/// Marker interface for entities.
/// </summary>
public interface IEntity
{
    /// <summary>
    /// Gets a read-only collection of domain events associated with the entity.
    /// </summary>
    IReadOnlyCollection<IDomainEvent> Events { get; }
}

[Obsolete]
public record struct IntegerKey(int Value) : IEntityKey;

[Obsolete]
public record struct LongKey(long Value) : IEntityKey;

[Obsolete]
public record struct StringKey(string Value) : IEntityKey;

[Obsolete]
public record struct GuidKey(Guid Value) : IEntityKey;