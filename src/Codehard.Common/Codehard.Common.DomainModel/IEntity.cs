namespace Codehard.Common.DomainModel;

public record EntityKey
{
    public sealed record Integer(int Value) : EntityKey;

    public sealed record Long(long Value) : EntityKey;

    public sealed record String(string Value) : EntityKey;

    public sealed record GlobalUniqueIdentifier(Guid Value) : EntityKey;
}

public interface IEntity
{
    EntityKey Id { get; }
}