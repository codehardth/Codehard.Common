namespace Codehard.Common.DomainModel;

public class EntityWithIntegerKey : Entity<IntegerKey>
{
    public override IntegerKey Id { get; protected init; }
}

public class EntityWithLongKey : Entity<LongKey>
{
    public override LongKey Id { get; protected init; }
}

public class EntityWithStringKey : Entity<StringKey>
{
    public override StringKey Id { get; protected init; }
}

public class EntityWithGuidKey : Entity<GuidKey>
{
    public override GuidKey Id { get; protected init; }
}
