using Codehard.Common.DomainModel;

namespace Codehard.Infrastructure.EntityFramework.Tests.Entities.DomainDrivenDesign;

public class DddC : Entity<Guid>
{
    public override Guid Id { get; init; }
    
    public DddB B { get; private set; } = null!;
}