using System.Reflection;
using Codehard.Functional.EntityFramework.Monads;
using Codehard.Functional.EntityFramework.Tests.Entities;
using Codehard.Infrastructure.EntityFramework.Extensions;
using LanguageExt;
using LanguageExt.Common;
using LanguageExt.Traits;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using static LanguageExt.Prelude;
using EntityARepo =
    Codehard.Functional.EntityFramework.Tests.Repository<Codehard.Functional.EntityFramework.Tests.TestDbContext,
        Codehard.Functional.EntityFramework.Tests.Entities.EntityA>;

namespace Codehard.Functional.EntityFramework.Tests;

static class DbError
{
    public enum Code
    {
        NotFound = 1,
    }

    public static readonly Error NotFound =
        Error.New((int)Code.NotFound, "Not found");
}

static class Repository<TContext, TEntity>
    where TContext : DbContext
    where TEntity : class
{
    public static K<Db, List<TEntity>> List()
        => from ctx in Db.Ctx<TContext>()
            from ps in ctx.Set<TEntity>().ToListAsync().ToDb()
            select ps;

    public static K<Db, EntityEntry<TEntity>> Add(TEntity entity)
        => from ctx in Db.Ctx<TContext>()
            from entry in ctx.AddAsync(entity).ToDb()
            select entry;

    public static K<Db, TEntity> Get(object id)
        => from ctx in Db.Ctx<TContext>()
            from p in liftIO(async () => await ctx.Set<TEntity>().FindAsync(id))
            from _ in guard(notnull(p), DbError.NotFound)
            select p;
}

public class DbMonadTests
{
    private static SqliteConnection CreateInMemoryDatabase()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();
        return connection;
    }

    [Fact]
    public async Task ShouldRunSuccessfully()
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
            Id = entityId
        };

        context.Set<EntityA>().Add(entity);
        await context.SaveChangesAsync();

        // Act

        var env = new DbEnv(context);

        var workflow =
            from entityA in EntityARepo.Get(entityId)
            from entityB in EntityARepo.Add(new EntityA
            {
                Id = Guid.NewGuid()
            })
            let compare = entityA.Id == entityB.Entity.Id
            from save in Db.Save()
            select compare;

        var res = workflow.Run().Run(env);

        var x = await res.RunAsync();

        // Assert
    }
}