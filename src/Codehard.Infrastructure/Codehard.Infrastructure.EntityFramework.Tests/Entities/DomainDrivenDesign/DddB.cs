using Codehard.Common.DomainModel;

namespace Codehard.Infrastructure.EntityFramework.Tests.Entities.DomainDrivenDesign;

public class DddB : Entity<Guid>
{
    public override Guid Id { get; init; }
    
    public DddA A { get; private set; } = null!;
    
    private readonly List<DddC> cs = new();
    
    public IReadOnlyCollection<DddC> Cs => cs.AsReadOnly();
    
    public void AddC(DddC c)
    {
        cs.Add(c);
    }
}