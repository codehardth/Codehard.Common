using Codehard.Common.DomainModel;

namespace Codehard.Infrastructure.EntityFramework.Tests.Entities;

public class EntityCreatedEvent : IDomainEvent<EntityAKey>
{
    public EntityCreatedEvent(EntityAKey id, DateTimeOffset timestamp)
    {
        Id = id;
        Timestamp = timestamp;
    }

    public EntityAKey Id { get; }

    public DateTimeOffset Timestamp { get; }
}

public record ValueChangedEvent(EntityAKey Id, string NewValue, DateTimeOffset Timestamp)
    : IDomainEvent<EntityAKey>;

public struct EntityAKey
{
    public Guid Value { get; set; }
}

public class EntityA : Entity<EntityAKey>
{
    public override EntityAKey Id { get; protected init; }

    public string Value { get; set; } = string.Empty;

    public void UpdateValue(string newValue)
    {
        this.Value = newValue;

        this.AddDomainEvent(new ValueChangedEvent(Id, newValue, DateTimeOffset.UtcNow));
    }

    public static EntityA Create()
    {
        var entity = new EntityA
        {
            Id = new EntityAKey
            {
                Value = Guid.NewGuid(),
            },
        };

        entity.AddDomainEvent(new EntityCreatedEvent(entity.Id, DateTimeOffset.UtcNow));

        return entity;
    }
}