namespace Codehard.Common.DomainModel;

/// <summary>
/// A marker interface for aggregate root.
/// </summary>
public interface IAggregateRoot
{
}

/// <summary>
/// Generic interface for aggregate roots with a key of type TKey.
/// </summary>
/// <typeparam name="TKey">The type of the entity's key.</typeparam>
public interface IAggregateRoot<TKey> : IEntity<TKey>, IAggregateRoot
    where TKey : struct
{
}