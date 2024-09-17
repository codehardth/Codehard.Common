using Codehard.Common.DomainModel;
using Codehard.Infrastructure.EntityFramework.Interceptors;
using Codehard.Infrastructure.EntityFramework.Tests.Entities;
using Codehard.Infrastructure.EntityFramework.Tests.ImmutableEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Codehard.Infrastructure.EntityFramework.Tests;

public class TestDbContext : DbContext, IDomainEventDbContext
{
    private readonly Action<ModelBuilder> builder;
    private readonly ILogger<TestDbContext> logger;
    private readonly PublishDomainEventDelegate? publisher;

    public TestDbContext(
        DbContextOptions<TestDbContext> options,
        Action<ModelBuilder> builder) : base(options)
    {
        this.builder = builder;
    }

    public TestDbContext(
        DbContextOptions<TestDbContext> options,
        Action<ModelBuilder> builder,
        PublishDomainEventDelegate? publisher) : base(options)
    {
        this.builder = builder;
        this.publisher = publisher;
    }

    public TestDbContext(
        DbContextOptions<TestDbContext> options,
        Action<ModelBuilder> builder,
        ILogger<TestDbContext> logger) : base(options)
    {
        this.builder = builder;
        this.logger = logger;
    }

    public DbSet<EntityA> As { get; set; }

    public DbSet<EntityB> Bs { get; set; }

    public DbSet<EntityC> Cs { get; set; }

    public DbSet<ImmutableEntityA> ImmAs { get; set; }

    public DbSet<ImmutableEntityB> ImmBs { get; set; }

    public DbSet<ImmutableEntityC> ImmCs { get; set; }

    public DbSet<Root> Roots { get; set; }

    public DbSet<MaterializedRoot> MaterializedRoots { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        this.builder(modelBuilder);

        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddDomainEventPublisherInterceptor(this.publisher);
    }

    public Task PublishDomainEventAsync(IDomainEvent domainEvent)
    {
        this.logger.LogInformation(domainEvent.ToString());

        return Task.CompletedTask;
    }
}