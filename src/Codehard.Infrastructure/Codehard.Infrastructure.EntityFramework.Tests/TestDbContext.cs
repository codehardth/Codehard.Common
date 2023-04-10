using Codehard.Infrastructure.EntityFramework.Tests.Entities;
using Microsoft.EntityFrameworkCore;

namespace Codehard.Infrastructure.EntityFramework.Tests;

public class TestDbContext : DbContext
{
    private readonly Action<ModelBuilder> action;

    public TestDbContext(DbContextOptions<TestDbContext> options, Action<ModelBuilder> action) : base(options)
    {
        this.action = action;
    }

    public DbSet<EntityA> As { get; set; }

    public DbSet<EntityB> Bs { get; set; }

    public DbSet<EntityC> Cs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        this.action(modelBuilder);
        base.OnModelCreating(modelBuilder);
    }
}