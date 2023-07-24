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
    public void WhenAddNewEntitiesWithSameReference_ShouldPersistedToDb()
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
        
        var entityA = ImmutableEntityA.Create(
            "text of A",
            Array.Empty<ImmutableEntityB>());
        
        context.ImmAs.Add(entityA);
        context.SaveChanges();
        context.Entry(entityA).State = EntityState.Detached;
        context.ChangeTracker.Clear();
        
        // Act
        var entityB1 =
            ImmutableEntityB
                .Create("text of B2-1")
                .UpdateReference(entityA);
        
        var entityB2 =
            ImmutableEntityB
                .Create("text of B2-2")
                .UpdateReference(entityA);

        context.AddRange(entityB1, entityB2);
        
        context.Entry(entityB1).Reference(nameof(ImmutableEntityB.A)).TargetEntry!.State = EntityState.Unchanged;

        context.SaveChanges();
        context.Entry(entityB1).State = EntityState.Detached;
        context.Entry(entityB2).State = EntityState.Detached;
        context.ChangeTracker.Clear();

        // Assert
        var actual = 
            context.ImmBs
                .Include(b => b.A)
                .AsNoTracking()
                .ToList();
        
        Assert.NotNull(actual);
        
        Assert.Equal(2, actual.Count);
        
        Assert.NotNull(actual[0].A);
        Assert.NotNull(actual[1].A);
        
        Assert.Equal(entityA.Id, actual[0].A.Id);
        Assert.Equal(entityA.Id, actual[1].A.Id);
    }
    
    [Fact]
    public void WhenAddNewEntitiesWithSameForeignKeyId_ShouldPersistedToDb()
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
        
        var entityA = ImmutableEntityA.Create(
            "text of A",
            Array.Empty<ImmutableEntityB>());
        
        context.ImmAs.Add(entityA);
        context.SaveChanges();
        context.Entry(entityA).State = EntityState.Detached;
        context.ChangeTracker.Clear();
        
        // Act
        var entityB1 =
            ImmutableEntityB
                .Create("text of B2-1")
                .UpdateForeignKey(entityA.Id);
        
        var entityB2 =
            ImmutableEntityB
                .Create("text of B2-2")
                .UpdateForeignKey(entityA.Id);

        context.AddRange(entityB1, entityB2);

        context.SaveChanges();
        context.Entry(entityB1).State = EntityState.Detached;
        context.Entry(entityB2).State = EntityState.Detached;
        context.ChangeTracker.Clear();

        // Assert
        var actual = 
            context.ImmBs
                .Include(b => b.A)
                .AsNoTracking()
                .ToList();
        
        Assert.NotNull(actual);
        
        Assert.Equal(2, actual.Count);
        
        Assert.NotNull(actual[0].A);
        Assert.NotNull(actual[1].A);
        
        Assert.Equal(entityA.Id, actual[0].A.Id);
        Assert.Equal(entityA.Id, actual[1].A.Id);
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