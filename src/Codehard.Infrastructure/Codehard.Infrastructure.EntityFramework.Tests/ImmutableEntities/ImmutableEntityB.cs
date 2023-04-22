namespace Codehard.Infrastructure.EntityFramework.Tests.ImmutableEntities;

public record ImmutableEntityB
{
    public Guid Id { get; private init; }

    public string Value { get; private init; }
    
    public ImmutableEntityA A { get; private init; }
    
    public List<ImmutableEntityC> Cs { get; private init; }
    
    public ImmutableEntityB UpdateScalar(string value)
    {
        return this with { Value = value };
    }
    
    public static ImmutableEntityB Create(
        string value)
    {
        return new()
        {
            Id = Guid.NewGuid(),
            Value = value,
        };
    }
}