using Codehard.Common.DomainModel;

namespace Codehard.Infrastructure.EntityFramework.Tests.Entities.DomainDrivenDesign;

public class DddA : Entity<Guid>, IAggregateRoot<Guid>
{
    public override Guid Id { get; init; }
    
    private readonly List<DddB> bs = new();
    
    public IReadOnlyCollection<DddB> Bs => bs.AsReadOnly();
    
    public void AddB(DddB b)
    {
        bs.Add(b);
    }
}