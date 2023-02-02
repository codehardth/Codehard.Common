namespace Codehard.Common.DomainModel;

public abstract class EntityWithIntegerKey : Entity<IntegerKey>
{
    public override IntegerKey Id { get; protected init; }
}

public abstract class EntityWithLongKey : Entity<LongKey>
{
    public override LongKey Id { get; protected init; }
}

public abstract class EntityWithStringKey : Entity<StringKey>
{
    public override StringKey Id { get; protected init; }
}

public abstract class EntityWithGuidKey : Entity<GuidKey>
{
    public override GuidKey Id { get; protected init; }
}
