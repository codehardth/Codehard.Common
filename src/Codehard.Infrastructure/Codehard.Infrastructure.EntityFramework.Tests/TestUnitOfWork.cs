using Codehard.Common.DomainModel;
using Codehard.Infrastructure.EntityFramework.Tests.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Codehard.Infrastructure.EntityFramework.Tests;

public class TestUnitOfWork : UnitOfWork
{
    private readonly ILogger<TestUnitOfWork> logger;

    public TestUnitOfWork(
        ILogger<TestUnitOfWork> logger,
        TestDbContext dbContext)
        : base(dbContext)
    {
        this.logger = logger;
    }

    public DbSet<EntityA> Entities => this.DbContext.Set<EntityA>();

    protected override Task PublishDomainEventAsync(IDomainEvent domainEvent)
    {
        this.logger.LogInformation(domainEvent.ToString());

        return Task.CompletedTask;
    }
}