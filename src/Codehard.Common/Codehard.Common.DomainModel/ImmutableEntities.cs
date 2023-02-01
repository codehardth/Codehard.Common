namespace Codehard.Common.DomainModel;

public record ImmutableEntityWithIntegerKey(IntegerKey Id) : IEntity<IntegerKey>
{
}

public record ImmutableEntityWithLongKey(LongKey Id) : IEntity<LongKey>
{
}

public record ImmutableEntityWithStringKey(StringKey Id) : IEntity<StringKey>
{
}

public record ImmutableEntityWithGuidKey(GuidKey Id) : IEntity<GuidKey>
{
}