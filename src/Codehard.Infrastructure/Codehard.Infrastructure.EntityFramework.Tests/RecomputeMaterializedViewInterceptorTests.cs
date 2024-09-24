using System.Reflection;
using Codehard.Infrastructure.EntityFramework.Extensions;
using Codehard.Infrastructure.EntityFramework.Interceptors;
using Codehard.Infrastructure.EntityFramework.Processors;
using Codehard.Infrastructure.EntityFramework.Tests.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Codehard.Infrastructure.EntityFramework.Tests;

public class RecomputeMaterializedViewInterceptorTests
{
    public class ProcessorStub : RecomputeMaterializedViewProcessor
    {
        public int RecomputeTimes { get; private set; }

        public ProcessorStub()
            : base(string.Empty)
        {
        }

        protected override string GetRefreshMaterializedViewQuery(string materializedViewName)
        {
            this.RecomputeTimes++;

            return string.Empty;
        }
    }

    [Fact]
    public async Task WhenExpectedEventOccur_ShouldProcess()
    {
        // Arrange
        var processor = new ProcessorStub();

        var assembly = Assembly.GetExecutingAssembly();
        var options = new DbContextOptionsBuilder<TestDbContext>()
                      .UseSqlite(CreateInMemoryDatabase())
                      .AddInterceptors(new DomainEventInterceptor<RootCreatedEvent>(processor))
                      .Options;

        var loggerMock = new Mock<ILogger<TestDbContext>>();
        var logger = loggerMock.Object;
        await using var context = new TestDbContext(
            options,
            builder => builder.ApplyConfigurationsFromAssemblyFor<TestDbContext>(assembly),
            logger);
        await context.Database.EnsureCreatedAsync();

        // Act
        var root = new Root(Guid.NewGuid(), "root", new List<Child>
        {
            new Child(0, "latest value"),
        });

        context.Add(root);

        await context.SaveChangesAsync();

        // Assert
        Assert.Equal(1, processor.RecomputeTimes);
    }

    [Fact]
    public async Task WhenExpectedEventOccur_WithMultipleProcessors_ShouldProcessAll()
    {
        // Arrange
        var processor1 = new ProcessorStub();
        var processor2 = new ProcessorStub();

        var assembly = Assembly.GetExecutingAssembly();
        var options = new DbContextOptionsBuilder<TestDbContext>()
                      .UseSqlite(CreateInMemoryDatabase())
                      .AddInterceptors(new DomainEventInterceptor<RootCreatedEvent>(processor1, processor2))
                      .Options;

        var loggerMock = new Mock<ILogger<TestDbContext>>();
        var logger = loggerMock.Object;
        await using var context = new TestDbContext(
            options,
            builder => builder.ApplyConfigurationsFromAssemblyFor<TestDbContext>(assembly),
            logger);
        await context.Database.EnsureCreatedAsync();

        // Act
        var root = new Root(Guid.NewGuid(), "root", new List<Child>
        {
            new Child(0, "latest value"),
        });

        context.Add(root);

        await context.SaveChangesAsync();

        // Assert
        Assert.Equal(1, processor1.RecomputeTimes);
        Assert.Equal(1, processor2.RecomputeTimes);
    }
    
    [Fact]
    public async Task WhenNoExpectedEventOccur_Should_Not_Process()
    {
        // Arrange
        var processor = new ProcessorStub();

        var assembly = Assembly.GetExecutingAssembly();
        var options = new DbContextOptionsBuilder<TestDbContext>()
                      .UseSqlite(CreateInMemoryDatabase())
                      .AddInterceptors(new DomainEventInterceptor<RootCreatedEvent>(processor))
                      .Options;

        var loggerMock = new Mock<ILogger<TestDbContext>>();
        var logger = loggerMock.Object;
        await using var context = new TestDbContext(
            options,
            builder => builder.ApplyConfigurationsFromAssemblyFor<TestDbContext>(assembly),
            logger);
        await context.Database.EnsureCreatedAsync();

        // Act
        await context.SaveChangesAsync();

        // Assert
        Assert.Equal(0, processor.RecomputeTimes);
    }

    private static SqliteConnection CreateInMemoryDatabase()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();
        return connection;
    }
}