namespace Codehard.Common.DomainModel;

/// <summary>
/// Generic interface for aggregate roots with a key of type TKey.
/// </summary>
/// <typeparam name="TKey">The type of the entity's key.</typeparam>
public interface IAggregateRoot<TKey> : IEntity<TKey>
    where TKey : struct
{
}