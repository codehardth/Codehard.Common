using Codehard.Infrastructure.EntityFramework.Tests.Entities.DomainDrivenDesign;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Codehard.Infrastructure.EntityFramework.Tests;

public class DddEntityTests
{
    [Fact]
    public async Task WhenSaveAggregateRoot_UsingDddDbContext_ThenSaveAllEntities()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<TestDddDbContext>()
            .UseSqlite(CreateInMemoryDatabase())
            .Options;
        
        await using var context = new TestDddDbContext(options);
        
        await context.Database.EnsureCreatedAsync();
        
        var a = new DddA { Id = Guid.NewGuid() };
        var b1 = new DddB { Id = Guid.NewGuid() };
        var c1 = new DddC { Id = Guid.NewGuid() };
        
        a.AddB(b1);
        b1.AddC(c1);
        context.As.Add(a);
        
        await context.SaveChangesAsync();
        
        context.ChangeTracker.Clear();
        
        // Act
        var savedA = await context.As
            .Include(dddA => dddA.Bs)
            .ThenInclude(dddB => dddB.Cs)
            .SingleAsync(dddA => dddA.Id == a.Id);
        
        // Assert
        Assert.NotNull(savedA);
        Assert.NotEmpty(savedA.Bs);
        Assert.NotEmpty(savedA.Bs.First().Cs);
    }
    
    [Fact]
    public async Task WhenAddNonAggregateRootDirectly_UsingDddDbContextSet_ThenExceptionIsThrown()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<TestDddDbContext>()
            .UseSqlite(CreateInMemoryDatabase())
            .Options;
        
        await using var context = new TestDddDbContext(options);
        
        await context.Database.EnsureCreatedAsync();
        
        var b1 = new DddB { Id = Guid.NewGuid() };
        var c1 = new DddC { Id = Guid.NewGuid() };
        
        b1.AddC(c1);

        InvalidOperationException? exception = null;
        
        // Act
        try
        {
            context.Set<DddB>().Add(b1);
        }
        catch (InvalidOperationException e)
        {
            exception = e;
        }
        
        // Assert
        Assert.NotNull(exception);
    }
    
    [Fact]
    public async Task WhenAddNonAggregateRootDirectly_UsingDddDbContext_ThenExceptionIsThrown()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<TestDddDbContext>()
            .UseSqlite(CreateInMemoryDatabase())
            .Options;
        
        await using var context = new TestDddDbContext(options);
        
        await context.Database.EnsureCreatedAsync();
        
        var b1 = new DddB { Id = Guid.NewGuid() };
        var c1 = new DddC { Id = Guid.NewGuid() };
        
        b1.AddC(c1);

        InvalidOperationException? exception = null;
        
        // Act
        try
        {
            context.Add(b1);
        }
        catch (InvalidOperationException e)
        {
            exception = e;
        }
        
        // Assert
        Assert.NotNull(exception);
    }
    
    [Fact]
    public async Task WhenAddNonAggregateRootDirectly_UsingDddDbContextGenericAsync_ThenExceptionIsThrown()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<TestDddDbContext>()
            .UseSqlite(CreateInMemoryDatabase())
            .Options;
        
        await using var context = new TestDddDbContext(options);
        
        await context.Database.EnsureCreatedAsync();
        
        var b1 = new DddB { Id = Guid.NewGuid() };
        var c1 = new DddC { Id = Guid.NewGuid() };
        
        b1.AddC(c1);

        InvalidOperationException? exception = null;
        
        // Act
        try
        {
            await context.AddAsync(b1);
        }
        catch (InvalidOperationException e)
        {
            exception = e;
        }
        
        // Assert
        Assert.NotNull(exception);
    }
    
    [Fact]
    public async Task WhenAddNonAggregateRootDirectly_UsingDddDbContextAsync_ThenExceptionIsThrown()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<TestDddDbContext>()
            .UseSqlite(CreateInMemoryDatabase())
            .Options;
        
        await using var context = new TestDddDbContext(options);
        
        await context.Database.EnsureCreatedAsync();
        
        var b1 = new DddB { Id = Guid.NewGuid() };
        var c1 = new DddC { Id = Guid.NewGuid() };
        
        b1.AddC(c1);

        InvalidOperationException? exception = null;
        
        // Act
        try
        {
            await context.AddAsync((object)b1);
        }
        catch (InvalidOperationException e)
        {
            exception = e;
        }
        
        // Assert
        Assert.NotNull(exception);
    }
    
    private static SqliteConnection CreateInMemoryDatabase()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();
        return connection;
    }
}