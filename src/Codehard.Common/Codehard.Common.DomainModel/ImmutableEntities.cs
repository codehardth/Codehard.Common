namespace Codehard.Common.DomainModel;

public abstract record ImmutableEntityWithIntegerKey(IntegerKey Id) : ImmutableEntity<IntegerKey>(Id);

public abstract record ImmutableEntityWithLongKey(LongKey Id) : ImmutableEntity<LongKey>(Id);

public abstract record ImmutableEntityWithStringKey(StringKey Id) : ImmutableEntity<StringKey>(Id);

public abstract record ImmutableEntityWithGuidKey(GuidKey Id) : ImmutableEntity<GuidKey>(Id);