using LanguageExt;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using static LanguageExt.Prelude;

// ReSharper disable once CheckNamespace
namespace Microsoft.EntityFrameworkCore;

/// <summary>
/// Provides extension methods for the DbContext class.
/// </summary>
public static class DbContextExtensions
{
    /// <summary>
    /// Asynchronously saves all changes made in this context to the database.
    /// </summary>
    /// <param name="dbContext">The DbContext instance.</param>
    /// <param name="ct">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous save operation. The task result contains the number of state entries written to the database.</returns>
    public static Aff<int> SaveChangesAff(
        this DbContext dbContext, CancellationToken ct = default)
    {
        return
            Aff(async () => await dbContext.SaveChangesAsync(ct));
    }
    
    /// <summary>
    /// Synchronously saves all changes made in this context to the database.
    /// </summary>
    /// <param name="dbContext">The DbContext instance.</param>
    /// <returns>An Eff&lt;int&gt; that represents the synchronous save operation. The result contains the number of state entries written to the database.</returns>
    public static Eff<int> SaveChangesEff(this DbContext dbContext)
    {
        return
            Eff(() => dbContext.SaveChanges());
    }
    
    /// <summary>
    /// Finds an entity with the given primary key values and wraps the result in an Option.
    /// </summary>
    /// <param name="dbContext">The DbContext instance.</param>
    /// <param name="keyValues">The values of the primary key for the entity to be found.</param>
    /// <typeparam name="TEntity">The type of the entity to be found.</typeparam>
    /// <returns>An Eff&lt;Option&lt;TEntity&gt;&gt; that represents the synchronous find operation. The result contains the entity found, or None if not found.</returns>
    public static Eff<Option<TEntity>> FindEff<TEntity>(
        this DbContext dbContext, params object[] keyValues)
        where TEntity : class
    {
        return
            Eff(() => Optional(dbContext.Find<TEntity>(keyValues)));
    }
    
    /// <summary>
    /// Asynchronously finds an entity with the given primary key values and wraps the result in an Option.
    /// </summary>
    /// <param name="dbContext">The DbContext instance.</param>
    /// <param name="keyValues">The values of the primary key for the entity to be found.</param>
    /// <typeparam name="TEntity">The type of the entity to be found.</typeparam>
    /// <returns>An Aff&lt;Option&lt;TEntity&gt;&gt; that represents the asynchronous find operation. The result contains the entity found, or None if not found.</returns>
    public static Aff<Option<TEntity>> FindAff<TEntity>(
        this DbContext dbContext, params object?[]? keyValues)
        where TEntity : class
    {
        return
            Aff(async () => Optional(await dbContext.FindAsync<TEntity>(keyValues)));
    }
    
    /// <summary>
    /// Asynchronously finds an entity with the given primary key values and wraps the result in an Option.
    /// </summary>
    /// <param name="dbContext">The DbContext instance.</param>
    /// <param name="keyValues">The values of the primary key for the entity to be found.</param>
    /// <param name="ct">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <typeparam name="TEntity">The type of the entity to be found.</typeparam>
    /// <returns>An Aff&lt;Option&lt;TEntity&gt;&gt; that represents the asynchronous find operation. The result contains the entity found, or None if not found.</returns>
    public static Aff<Option<TEntity>> FindAff<TEntity>(
        this DbContext dbContext, object?[]? keyValues, CancellationToken ct)
        where TEntity : class
    {
        return
            Aff(async () => Optional(await dbContext.FindAsync<TEntity>(keyValues, ct)));
    }

    public static Eff<EntityEntry<TEntity>> AddEff<TEntity>(
        this DbContext dbContext, TEntity entity)
        where TEntity : class
    {
        return
            Eff(() => dbContext.Add(entity));
    }
    
    public static Eff<Unit> AddRangeEff<TEntity>(
        this DbContext dbContext, params TEntity[] entities)
        where TEntity : class
    {
        return Eff(() =>
        {
            dbContext.AddRange(entities);

            return unit;
        });
    }
    
    public static Eff<EntityEntry<TEntity>> UpdateEff<TEntity>(
        this DbContext dbContext, TEntity entity)
        where TEntity : class
    {
        return
            Eff(() => dbContext.Update(entity));
    }
    
    public static Eff<Unit> UpdateRangeEff<TEntity>(
        this DbContext dbContext, params TEntity[] entities)
        where TEntity : class
    {
        return Eff(() =>
        {
            dbContext.UpdateRange(entities);

            return unit;
        });
    }
    
    public static Eff<EntityEntry<TEntity>> RemoveEff<TEntity>(
        this DbContext dbContext, TEntity entity)
        where TEntity : class
    {
        return
            Eff(() => dbContext.Remove(entity));
    }
    
    public static Eff<Unit> RemoveRangeEff<TEntity>(
        this DbContext dbContext, params TEntity[] entities)
        where TEntity : class
    {
        return Eff(() =>
        {
            dbContext.RemoveRange(entities);

            return unit;
        });
    }
}