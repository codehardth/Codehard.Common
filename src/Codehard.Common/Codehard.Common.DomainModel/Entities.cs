namespace Codehard.Common.DomainModel;

public abstract class EntityWithIntegerKey : Entity<IntegerKey>
{
    protected EntityWithIntegerKey()
    {
    }

    protected EntityWithIntegerKey(Action<object, string> lazyLoader)
        : base(lazyLoader)
    {
    }

    public override IntegerKey Id { get; protected init; }
}

public abstract class EntityWithLongKey : Entity<LongKey>
{
    protected EntityWithLongKey()
    {
    }

    protected EntityWithLongKey(Action<object, string> lazyLoader)
        : base(lazyLoader)
    {
    }

    public override LongKey Id { get; protected init; }
}

public abstract class EntityWithStringKey : Entity<StringKey>
{
    protected EntityWithStringKey()
    {
    }

    protected EntityWithStringKey(Action<object, string> lazyLoader)
        : base(lazyLoader)
    {
    }

    public override StringKey Id { get; protected init; }
}

public abstract class EntityWithGuidKey : Entity<GuidKey>
{
    protected EntityWithGuidKey()
    {
    }

    protected EntityWithGuidKey(Action<object, string> lazyLoader)
        : base(lazyLoader)
    {
    }

    public override GuidKey Id { get; protected init; }
}