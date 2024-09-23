using System.Reflection;
using Codehard.Common.DomainModel;
using Codehard.Infrastructure.EntityFramework.Extensions;
using Codehard.Infrastructure.EntityFramework.Interceptors;
using Codehard.Infrastructure.EntityFramework.Tests.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Codehard.Infrastructure.EntityFramework.Tests;

public class RecomputeMaterializedViewInterceptorTests
{
    public class InterceptorStub<TEvent> : RecomputeMaterializedViewInterceptor<TEvent>
        where TEvent : IDomainEvent
    {
        public int RecomputeTimes { get; private set; }

        public InterceptorStub()
            : base(string.Empty)
        {
        }

        protected override Task RecomputeAsync(DbContext dbContext)
        {
            this.RecomputeTimes++;
            return Task.CompletedTask;
        }
    }

    [Fact]
    public async Task WhenExpectedEventOccur_ShouldRecompute()
    {
        // Arrange
        var interceptor = new InterceptorStub<RootCreatedEvent>();

        var assembly = Assembly.GetExecutingAssembly();
        var options = new DbContextOptionsBuilder<TestDbContext>()
                      .UseSqlite(CreateInMemoryDatabase())
                      .AddInterceptors(interceptor)
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
        Assert.Equal(1, interceptor.RecomputeTimes);
    }

    private static SqliteConnection CreateInMemoryDatabase()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();
        return connection;
    }
}