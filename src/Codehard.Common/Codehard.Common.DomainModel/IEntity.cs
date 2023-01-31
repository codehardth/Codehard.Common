namespace Codehard.Common.DomainModel;

public interface IEntity<out TId>
    where TId : struct
{
    TId Id { get; }
}