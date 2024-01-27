using Codehard.Functional.EntityFramework.Tests.Entities;
using Microsoft.EntityFrameworkCore;

namespace Codehard.Functional.EntityFramework.Tests;

public class TestDbContext : DbContext
{
    private readonly Action<ModelBuilder> builder;

    public TestDbContext(
        DbContextOptions<TestDbContext> options,
        Action<ModelBuilder> builder)
        : base(options)
    {
        this.builder = builder;
    }

    public DbSet<EntityA> As { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        this.builder(modelBuilder);

        base.OnModelCreating(modelBuilder);
    }
}