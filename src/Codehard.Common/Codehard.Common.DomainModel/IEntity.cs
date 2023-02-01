using System.Text.Json.Serialization;
using Codehard.Common.DomainModel.Converters;

namespace Codehard.Common.DomainModel;

public interface IEntityKey
{
}

public interface IEntity<TKey>
    where TKey : IEntityKey
{
    TKey Id { get; }
}

[JsonConverter(typeof(IntegerKeyJsonConverter))]
public record struct IntegerKey(int Value) : IEntityKey;

[JsonConverter(typeof(LongKeyJsonConverter))]
public record struct LongKey(long Value) : IEntityKey;

[JsonConverter(typeof(StringKeyJsonConverter))]
public record struct StringKey(string Value) : IEntityKey;

[JsonConverter(typeof(GuidKeyJsonConverter))]
public record struct GuidKey(Guid Value) : IEntityKey;