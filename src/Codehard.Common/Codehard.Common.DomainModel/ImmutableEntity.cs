namespace Codehard.Common.DomainModel;

public record ImmutableEntity<TKey>(TKey Id) : IEntity<TKey>
    where TKey : IEntityKey
{
}