using System.Reflection;
using LanguageExt;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

using Codehard.Functional.EntityFramework.Tests.Entities;
using Codehard.Infrastructure.EntityFramework.Extensions;
using static Codehard.Functional.EntityFramework.Extensions.QueryablePrelude;

namespace Codehard.Functional.EntityFramework.Tests;

public class QueryableExtensionsTests
{
    private static SqliteConnection CreateInMemoryDatabase()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();
        return connection;
    }
    
    [Fact]
    public async Task ToListEff_ShouldReturnList()
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
        var entity = new EntityA
        {
            Id = entityId,
        };
        
        context.As.Add(entity);
        await context.SaveChangesAsync();

        var queryable = context.As.AsQueryable();

        // Act
        var result =
            queryable
                .ToListEff()
                .RunAsync(EnvIO.New(token: CancellationToken.None));

        // Assert
        var list = await result;
        
        var resultEntity = list.ThrowIfFail();
        
        Assert.NotNull(resultEntity);
        Assert.Single(resultEntity);
    }
    
    [Fact]
    public async Task FilterRtAndToListEff_ShouldReturnList()
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
        var entity = new EntityA
        {
            Id = entityId,
        };
        
        context.As.Add(entity);
        await context.SaveChangesAsync();

        // Act
        var workflow =
            from @as in From<EntityA>()
            from list in
                @as.Where(a => a.Id == entityId)
                   .ToListEff()
            select list;

        // Assert
        var listResult = await workflow.RunAsync(context);
        var resultEntity = listResult.ThrowIfFail();
        
        Assert.NotNull(resultEntity);
        Assert.Single(resultEntity);
    }
}
