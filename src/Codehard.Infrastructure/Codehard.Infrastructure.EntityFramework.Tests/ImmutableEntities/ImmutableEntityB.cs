namespace Codehard.Infrastructure.EntityFramework.Tests.ImmutableEntities;

public record ImmutableEntityB
{
    public Guid Id { get; private init; }

    public string Value { get; private init; } = string.Empty;
    
    public ImmutableEntityA? A { get; private init; }
    
    public Guid AId { get; private init; }
    
    public List<ImmutableEntityC> Cs { get; private init; } = new();
    
    public ImmutableEntityB UpdateScalar(string value)
    {
        return this with { Value = value };
    }
    
    public ImmutableEntityB UpdateReference(ImmutableEntityA a)
    {
        return this with { A = a };
    }
    
    public ImmutableEntityB UpdateForeignKey(Guid aId)
    {
        return this with { AId = aId };
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