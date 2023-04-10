using System.Reflection;
using Codehard.Infrastructure.EntityFramework.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;

namespace Codehard.Infrastructure.EntityFramework.Tests;

public class ModelBuilderExtensionsTests
{
    [Fact]
    public void WhenUseApplyConfigurationsFromAssemblyForSpecificContext_ShouldApplyCorrectly()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseSqlite(CreateInMemoryDatabase())
            .Options;
        var assembly = Assembly.GetExecutingAssembly();
        var expectedEntityTypes =
            assembly
                .GetTypes()
                .Count(t => t.BaseType is { IsGenericType: true } &&
                            t.BaseType.GetGenericTypeDefinition() ==
                            typeof(EntityTypeConfigurationBase<,>));

        // Act
        using var context = new TestDbContext(
            options,
            builder => builder.ApplyConfigurationsFromAssemblyFor<TestDbContext>(assembly));
        context.Database.EnsureCreated();

        // Assert
        var actualEntityTypes = context.Model.GetEntityTypes().Count();
        Assert.Equal(expectedEntityTypes, actualEntityTypes);

        static SqliteConnection CreateInMemoryDatabase()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            return connection;
        }
    }
}