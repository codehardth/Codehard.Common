using System.Reflection;
using Codehard.Functional.EntityFramework.Tests.Entities;
using Codehard.Infrastructure.EntityFramework.Extensions;
using LanguageExt;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Codehard.Functional.EntityFramework.Tests;

public class FunctionalDbContextTests
{
    private static SqliteConnection CreateInMemoryDatabase()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();
        return connection;
    }
    
    [Fact]
    public async Task ShouldGenerateDbContext()
    {
        // Arrange
        var assembly = Assembly.GetExecutingAssembly();
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseSqlite(CreateInMemoryDatabase())
            .Options;
        
        await using var context = new TestDbContext(
            options,
            builder => builder.ApplyConfigurationsFromAssemblyFor<TestDbContext>(assembly));
        await context.Database.EnsureCreatedAsync();

        var entityId = Guid.NewGuid();
        
        // Act
        var entity = new EntityA()
        {
            Id = entityId,
        };
        
        var effDbContext = new EffTestDbContext(context);

        _ = await effDbContext.AddAsync(entity)
            .Bind(_ => effDbContext.SaveChangesAsync())
            .RunUnit();

        // Assert
        var result =
            await effDbContext.FindAsync<EntityA>(new object[] { entityId })
                .Run();

        var entityOpt = result.ThrowIfFail();
        
        var entity2 = entityOpt.IfNone(() => throw new Exception("Entity not found"));
        
        Assert.Equal(entity.Id, entity2.Id);
    }
}