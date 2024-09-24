using Codehard.Common.DomainModel;

namespace Codehard.Infrastructure.EntityFramework.Tests.Entities;

public record RootCreatedEvent(Guid Id, DateTimeOffset Timestamp) : IDomainEvent<Guid>;

public class Root : Entity<Guid>, IAggregateRoot<Guid>
{
    public Root()
    {
    }

    public Root(Guid id, string value, List<Child> children)
    {
        Id = id;
        Value = value;
        Children = children;
        AddDomainEvent(new RootCreatedEvent(id, DateTimeOffset.Now));
    }

    public override Guid Id { get; init; }

    public string Value { get; set; }

    public virtual List<Child> Children { get; set; }
}

public class Child : Entity<int>
{
    public Child()
    {
    }

    public Child(int id, string value)
    {
        Id = id;
        Value = value;
    }

    public override int Id { get; init; }

    public string Value { get; set; }
}

public class MaterializedRoot : Entity<int>
{
    public MaterializedRoot()
    {
    }

    public MaterializedRoot(Guid id, string value, string lastestChildValue)
    {
        this.RootId = id;
        Value = value;
        LastestChildValue = lastestChildValue;
    }

    public override int Id { get; init; }

    public Guid RootId { get; set; }

    public string Value { get; set; }

    public string LastestChildValue { get; set; }
}

public class CompletelyNonRelated : Entity<int>
{
    public CompletelyNonRelated(int id, DateTime createdAt)
    {
        this.Id = id;
        this.CreatedAt = createdAt;
    }

    public override int Id { get; init; }

    public DateTime CreatedAt { get; set; }
}