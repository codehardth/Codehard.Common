namespace Codehard.Common.DomainModel;

public interface IEntityKey
{
}

public interface IEntity<TKey>
    where TKey : IEntityKey
{
    TKey Id { get; }
}

public record struct IntegerKey(int Value) : IEntityKey;

public record struct LongKey(long Value) : IEntityKey;

public record struct StringKey(string Value) : IEntityKey;

public record struct GuidKey(Guid Value) : IEntityKey;