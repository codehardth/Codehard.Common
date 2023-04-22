using System.Reflection;
using Codehard.Infrastructure.EntityFramework.Extensions;
using Codehard.Infrastructure.EntityFramework.Tests.ImmutableEntities;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Codehard.Infrastructure.EntityFramework.Tests;

public class NonTrackingEntitiesChangedUpdateTests
{
    private static SqliteConnection CreateInMemoryDatabase()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();
        return connection;
    }
    
    [Fact]
    public void WhenAddNewEntity_ShouldPersistedToDb()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseSqlite(CreateInMemoryDatabase())
            .Options;
        
        var assembly = Assembly.GetExecutingAssembly();
        
        using var context = new TestDbContext(
            options,
            builder => builder.ApplyConfigurationsFromAssemblyFor<TestDbContext>(assembly));
        
        context.Database.EnsureCreated();
        
        // Act
        var newEntity = ImmutableEntityA.Create(
            "text",
            new[]
            {
                ImmutableEntityB.Create("text"),
            });
        
        context.ImmAs.Add(newEntity);
        context.SaveChanges();
        context.Entry(newEntity).State = EntityState.Detached;

        // Assert
        var actual = context.ImmAs.Count();
        Assert.Equal(1, actual);
    }
    
    [Fact]
    public void WhenUpdateScalarPropertyOfEntity_ShouldPersistedToDb()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseSqlite(CreateInMemoryDatabase())
            .Options;
        
        var assembly = Assembly.GetExecutingAssembly();
        
        using var context = new TestDbContext(
            options,
            builder => builder.ApplyConfigurationsFromAssemblyFor<TestDbContext>(assembly));
        
        context.Database.EnsureCreated();
        
        var newEntity = ImmutableEntityA.Create(
            "text",
            new[]
            {
                ImmutableEntityB.Create("text"),
            });
        
        context.ImmAs.Add(newEntity);
        context.SaveChanges();
        context.Entry(newEntity).State = EntityState.Detached;
        context.ChangeTracker.Clear();
        
        // Act
        var modifiedEntity = newEntity.UpdateScalar("new text");
        
        context.UpdateIfChanged(newEntity, modifiedEntity);
        context.SaveChanges();
        context.Entry(newEntity).State = EntityState.Detached;
        context.ChangeTracker.Clear();

        // Assert
        var actual = 
            context.ImmAs
                   .AsNoTracking()
                   .SingleOrDefault(entity => entity.Id == newEntity.Id);
        
        Assert.NotNull(actual);
        Assert.Equal("new text", actual.Value);
    }
    
    [Fact]
    public void WhenAddNewItemToCollectionOfEntity_ShouldPersistedToDb()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseSqlite(CreateInMemoryDatabase())
            .Options;
        
        var assembly = Assembly.GetExecutingAssembly();
        
        using var context = new TestDbContext(
            options,
            builder => builder.ApplyConfigurationsFromAssemblyFor<TestDbContext>(assembly));
        
        context.Database.EnsureCreated();
        
        var newEntity = ImmutableEntityA.Create(
            "text of A",
            new[]
            {
                ImmutableEntityB.Create("text of B1"),
            });
        
        context.ImmAs.Add(newEntity);
        context.SaveChanges();
        context.Entry(newEntity).State = EntityState.Detached;
        context.ChangeTracker.Clear();
        
        // Act
        var modifiedEntity = newEntity.ReplaceCollection(
            new []
            {
                ImmutableEntityB.Create("text of B2"),
            });
        
        context.UpdateIfChanged(newEntity, modifiedEntity);
        context.SaveChanges();
        context.Entry(newEntity).State = EntityState.Detached;
        context.ChangeTracker.Clear();

        // Assert
        var actual = 
            context.ImmAs
                .AsNoTracking()
                .Include(entity => entity.Bs)
                .SingleOrDefault(entity => entity.Id == newEntity.Id);
        
        Assert.NotNull(actual);
        Assert.Single(actual.Bs);       
        Assert.Equal("text of B2", actual.Bs.First().Value);
    }
    
    [Fact]
    public void WhenModifyItemInCollectionOfEntity_ShouldPersistedToDb()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseSqlite(CreateInMemoryDatabase())
            .Options;
        
        var assembly = Assembly.GetExecutingAssembly();
        
        using var context = new TestDbContext(
            options,
            builder => builder.ApplyConfigurationsFromAssemblyFor<TestDbContext>(assembly));
        
        context.Database.EnsureCreated();
        
        var newEntity = ImmutableEntityA.Create(
            "text of A",
            new[]
            {
                ImmutableEntityB.Create("text of B1"),
            });
        
        context.ImmAs.Add(newEntity);
        context.SaveChanges();
        context.Entry(newEntity).State = EntityState.Detached;
        context.ChangeTracker.Clear();
        
        // Act
        var modifiedEntity = newEntity.ModifyItemInCollection(
            "text of B2", 0);
        
        context.UpdateIfChanged(newEntity, modifiedEntity);
        context.SaveChanges();
        context.Entry(newEntity).State = EntityState.Detached;
        context.ChangeTracker.Clear();

        // Assert
        var actual = 
            context.ImmAs
                .AsNoTracking()
                .Include(entity => entity.Bs)
                .SingleOrDefault(entity => entity.Id == newEntity.Id);
        
        Assert.NotNull(actual);
        Assert.Single(actual.Bs);       
        Assert.Equal("text of B2", actual.Bs.First().Value);
    }
}