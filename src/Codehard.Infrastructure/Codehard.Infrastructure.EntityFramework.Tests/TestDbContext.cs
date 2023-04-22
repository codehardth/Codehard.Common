using Codehard.Infrastructure.EntityFramework.Tests.Entities;
using Codehard.Infrastructure.EntityFramework.Tests.ImmutableEntities;
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
    
    public DbSet<ImmutableEntityA> ImmAs { get; set; }

    public DbSet<ImmutableEntityB> ImmBs { get; set; }

    public DbSet<ImmutableEntityC> ImmCs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        this.action(modelBuilder);

        base.OnModelCreating(modelBuilder);
    }
}