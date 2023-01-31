namespace Codehard.Common.DomainModel;

public interface IAggregateRoot<out TId> : IEntity<TId>
    where TId : struct
{
}