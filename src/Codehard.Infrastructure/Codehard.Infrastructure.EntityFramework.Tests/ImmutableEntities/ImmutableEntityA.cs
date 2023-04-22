namespace Codehard.Infrastructure.EntityFramework.Tests.ImmutableEntities;

public record ImmutableEntityA
{
    public Guid Id { get; private init; }

    public string Value { get; private init; }
    
    public List<ImmutableEntityB> Bs { get; private init; }
    
    public ImmutableEntityA UpdateScalar(string value)
    {
        return this with { Value = value };
    }
    
    public ImmutableEntityA ReplaceCollection(
        IEnumerable<ImmutableEntityB> bs)
    {
        return this with { Bs = bs.ToList() };
    }
    
    public ImmutableEntityA ModifyItemInCollection(
        string newValue, int index)
    {
        return
            this with
            {
                Bs = 
                    Bs.Select((b, i) =>
                            i == index 
                                ? b.UpdateScalar(newValue) 
                                : b)
                      .ToList()
            };
    }
    
    public static ImmutableEntityA Create(
        string value, IEnumerable<ImmutableEntityB> bs)
    {
        return new()
        {
            Id = Guid.NewGuid(),
            Value = value,
            Bs = bs.ToList(),
        };
    }
}