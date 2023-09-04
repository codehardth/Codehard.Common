namespace Codehard.Common.DomainModel;

public interface IEntityKey
{
}

public interface IEntity<TKey>
    where TKey : IEntityKey
{
    TKey Id { get; }
}