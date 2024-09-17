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
    public async Task WhenAddedEntity_WithOnlyBaseEntity_ThenAddedMaterializedViewEntity()
    {
        // Arrange
        var assembly = Assembly.GetExecutingAssembly();
        var options = new DbContextOptionsBuilder<TestDbContext>()
                      .UseSqlite(CreateInMemoryDatabase())
                      .AddEntityToMaterializedViewInterceptor<TestDbContext, Root, MaterializedRoot>(
                          static root => new MaterializedRoot(root.Id, root.Value, root.Children.Last().Value))
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

    [Fact]
    public async Task WhenAddedEntity_WithOneDependentEntity_ThenAddedMaterializedViewEntity()
    {
        // Arrange
        var assembly = Assembly.GetExecutingAssembly();
        var options = new DbContextOptionsBuilder<TestDbContext>()
                      .UseSqlite(CreateInMemoryDatabase())
                      .AddEntityToMaterializedViewInterceptor<TestDbContext, Root, Child, MaterializedRoot>(
                          static (root, child) => new MaterializedRoot(root.Id, root.Value, child.Value))
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

    [Fact]
    public async Task WhenAddedEntity_WithOneDependentEntity_WithNoRelation_ThenAddedMaterializedViewEntity()
    {
        // Arrange
        var assembly = Assembly.GetExecutingAssembly();
        var options = new DbContextOptionsBuilder<TestDbContext>()
                      .UseSqlite(CreateInMemoryDatabase())
                      .AddEntityToMaterializedViewInterceptor<TestDbContext, Root, CompletelyNonRelated,
                          MaterializedRoot>(
                          static (root, nr) => new MaterializedRoot(root.Id, root.Value, nr.Id.ToString()))
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
        var nr = new CompletelyNonRelated(int.MaxValue, DateTime.Now);

        context.Add(root);
        context.Add(nr);

        await context.SaveChangesAsync();

        // Assert
        var mvRoot = context.ChangeTracker.Entries<MaterializedRoot>().Single().Entity;
        Assert.Equal(mvRoot.RootId, root.Id);
        Assert.Equal(mvRoot.Value, root.Value);
        Assert.Equal(mvRoot.LastestChildValue, nr.Id.ToString());
    }

    [Fact]
    public async Task WhenAddedEntity_WithTwoDependentEntities_ThenAddedMaterializedViewEntity()
    {
        // Arrange
        var assembly = Assembly.GetExecutingAssembly();
        var options = new DbContextOptionsBuilder<TestDbContext>()
                      .UseSqlite(CreateInMemoryDatabase())
                      .AddEntityToMaterializedViewInterceptor<TestDbContext, Root, Child, CompletelyNonRelated,
                          MaterializedRoot>(
                          static (root, child, nr) =>
                              new MaterializedRoot(root.Id, root.Value, child.Value + nr.Id))
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
        var nr = new CompletelyNonRelated(int.MaxValue, DateTime.Now);

        context.Add(root);
        context.Add(nr);

        await context.SaveChangesAsync();

        // Assert
        var mvRoot = context.ChangeTracker.Entries<MaterializedRoot>().Single().Entity;
        Assert.Equal(mvRoot.RootId, root.Id);
        Assert.Equal(mvRoot.Value, root.Value);
        Assert.Equal(mvRoot.LastestChildValue, root.Children.Last().Value + nr.Id);
    }

    private static SqliteConnection CreateInMemoryDatabase()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();
        return connection;
    }
}