using System.Reflection;
using Codehard.Infrastructure.EntityFramework.Extensions;
using Codehard.Infrastructure.EntityFramework.Tests.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Codehard.Infrastructure.EntityFramework.Tests;

public class MoneyTypeTests
{
    private static SqliteConnection CreateInMemoryDatabase()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();
        return connection;
    }
    
    [Fact]
    public async void WhenAddNewEntity_ShouldPersistedToDb()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseSqlite(CreateInMemoryDatabase())
            .Options;
        
        var assembly = Assembly.GetExecutingAssembly();
        
        var loggerMock = new Mock<ILogger<TestDbContext>>();
        var logger = loggerMock.Object;
        
        await using var context = new TestDbContext(
            options,
            builder => builder.ApplyConfigurationsFromAssemblyFor<TestDbContext>(assembly),
            logger);
        
        await context.Database.EnsureCreatedAsync();
        
        // Act
        var newEntity = EntityA.Create();
        
        context.As.Add(newEntity);
        context.SaveChanges();
        context.Entry(newEntity).State = EntityState.Detached;

        // Assert
        var actual =
            context.As
                .Include(entityA => entityA.NullableMoney)
                .Include(entityA => entityA.Money)
                .First();
        
        Assert.Null(actual.NullableMoney);
        Assert.NotNull(actual.Money);
        Assert.Equal(0, actual.Money.Amount);
    }
}