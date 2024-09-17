using System.Reflection;
using Codehard.Infrastructure.EntityFramework.Extensions;
using Codehard.Infrastructure.EntityFramework.Tests.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Codehard.Infrastructure.EntityFramework.Tests;

public class EntityToMaterializedViewInterceptorTests
{
    [Fact]
    public async Task WhenAddedEntity_ThenAddedMaterializedViewEntity()
    {
        // Arrange
        var assembly = Assembly.GetExecutingAssembly();
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseSqlite(CreateInMemoryDatabase())
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
        var mvRoot = context.ChangeTracker.Entries<MaterializedRoot>().Single().Entity;
        Assert.Equal(mvRoot.RootId, root.Id);
        Assert.Equal(mvRoot.Value, root.Value);
        Assert.Equal(mvRoot.LastestChildValue, root.Children.Last().Value);
    }

    private static SqliteConnection CreateInMemoryDatabase()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();
        return connection;
    }
}