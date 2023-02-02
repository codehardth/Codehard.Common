namespace Codehard.Common.DomainModel;

public abstract record ImmutableEntity<TKey>(TKey Id) : IEntity<TKey>
    where TKey : IEntityKey
{
}