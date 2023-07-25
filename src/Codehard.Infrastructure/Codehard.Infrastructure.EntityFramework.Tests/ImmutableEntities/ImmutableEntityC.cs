namespace Codehard.Infrastructure.EntityFramework.Tests.ImmutableEntities;

public record ImmutableEntityC
{
    public Guid Id { get; private init; }

    public string Value { get; private init; } = string.Empty;
}