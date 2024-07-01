using Codehard.Infrastructure.EntityFramework.DbContexts;
using Codehard.Infrastructure.EntityFramework.Tests.Entities.DomainDrivenDesign;
using Microsoft.EntityFrameworkCore;

namespace Codehard.Infrastructure.EntityFramework.Tests;

public class TestDddDbContext : DddDbContext
{
    public TestDddDbContext(DbContextOptions options) : base(options)
    {
    }
    
    public DbSet<DddA> As { get; set; }
}