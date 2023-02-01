namespace Codehard.Common.DomainModel;

public interface IAggregateRoot<TKey> 
    : IEntity<TKey> where TKey : IEntityKey
{
}