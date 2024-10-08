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
    public static Eff<int> SaveChangesAsyncEff(
        this DbContext dbContext, CancellationToken ct = default)
    {
        return liftEff(() => dbContext.SaveChangesAsync(ct));
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
        return liftEff(() => Optional(dbContext.Find<TEntity>(keyValues)));
    }
    
    /// <summary>
    /// Asynchronously finds an entity with the given primary key values and wraps the result in an Option.
    /// </summary>
    /// <param name="dbContext">The DbContext instance.</param>
    /// <param name="keyValues">The values of the primary key for the entity to be found.</param>
    /// <typeparam name="TEntity">The type of the entity to be found.</typeparam>
    /// <returns>An Eff&lt;Option&lt;TEntity&gt;&gt; that represents the asynchronous find operation. The result contains the entity found, or None if not found.</returns>
    public static Eff<Option<TEntity>> FindAsyncEff<TEntity>(
        this DbContext dbContext, params object?[]? keyValues)
        where TEntity : class
    {
        return liftEff(async () => Optional(await dbContext.FindAsync<TEntity>(keyValues)));
    }
    
    /// <summary>
    /// Asynchronously finds an entity with the given primary key values and wraps the result in an Option.
    /// </summary>
    /// <param name="dbContext">The DbContext instance.</param>
    /// <param name="keyValues">The values of the primary key for the entity to be found.</param>
    /// <param name="ct">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <typeparam name="TEntity">The type of the entity to be found.</typeparam>
    /// <returns>An Eff&lt;Option&lt;TEntity&gt;&gt; that represents the asynchronous find operation. The result contains the entity found, or None if not found.</returns>
    public static Eff<Option<TEntity>> FindAsyncEff<TEntity>(
        this DbContext dbContext, object?[]? keyValues, CancellationToken ct)
        where TEntity : class
    {
        return liftEff(async () => Optional(await dbContext.FindAsync<TEntity>(keyValues, ct)));
    }

    public static Eff<EntityEntry<TEntity>> AddEff<TEntity>(
        this DbContext dbContext, TEntity entity)
        where TEntity : class
    {
        return liftEff(() => dbContext.Add(entity));
    }
    
    public static Eff<Unit> AddRangeEff<TEntity>(
        this DbContext dbContext, params TEntity[] entities)
        where TEntity : class
    {
        return liftEff(() =>
        {
            dbContext.AddRange(entities);

            return unit;
        });
    }

    /// <summary>
    /// Adds a range of entities to the DbContext in an effectful manner.
    /// </summary>
    /// <param name="dbContext">The DbContext instance.</param>
    /// <param name="entities">The entities to be added.</param>
    /// <typeparam name="TEntity">The type of the entities to be added.</typeparam>
    /// <returns>An Eff&lt;Unit&gt; representing the effectful operation. The result is a Unit indicating the operation has been performed.</returns>
    public static Eff<Unit> AddRangeEff<TEntity>(
        this DbContext dbContext, IEnumerable<TEntity> entities)
        where TEntity : class
    {
        return liftEff(() =>
        {
            dbContext.AddRange(entities);

            return unit;
        });
    }
    
    /// <summary>
    /// Updates an entity in the DbContext in an effectful manner.
    /// </summary>
    /// <param name="dbContext">The DbContext instance.</param>
    /// <param name="entity">The entity to be updated.</param>
    /// <typeparam name="TEntity">The type of the entity to be updated.</typeparam>
    /// <returns>An Eff&lt;EntityEntry&lt;TEntity&gt;&gt; representing the effectful operation. The result is an EntityEntry indicating the operation has been performed.</returns>
    public static Eff<EntityEntry<TEntity>> UpdateEff<TEntity>(
        this DbContext dbContext, TEntity entity)
        where TEntity : class
    {
        return
            liftEff(() => dbContext.Update(entity));
    }
    
    /// <summary>
    /// Updates a range of entities in the DbContext in an effectful manner.
    /// </summary>
    /// <param name="dbContext">The DbContext instance.</param>
    /// <param name="entities">The entities to be updated.</param>
    /// <typeparam name="TEntity">The type of the entities to be updated.</typeparam>
    /// <returns>An Eff&lt;Unit&gt; representing the effectful operation. The result is a Unit indicating the operation has been performed.</returns>
    public static Eff<Unit> UpdateRangeEff<TEntity>(
        this DbContext dbContext, params TEntity[] entities)
        where TEntity : class
    {
        return liftEff(() =>
        {
            dbContext.UpdateRange(entities);

            return unit;
        });
    }
    
    /// <summary>
    /// Updates a range of entities in the DbContext in an effectful manner.
    /// </summary>
    /// <param name="dbContext">The DbContext instance.</param>
    /// <param name="entities">The entities to be updated.</param>
    /// <typeparam name="TEntity">The type of the entities to be updated.</typeparam>
    /// <returns>An Eff&lt;Unit&gt; representing the effectful operation. The result is a Unit indicating the operation has been performed.</returns>
    public static Eff<Unit> UpdateRangeEff<TEntity>(
        this DbContext dbContext, IEnumerable<TEntity> entities)
        where TEntity : class
    {
        return liftEff(() =>
        {
            dbContext.UpdateRange(entities);

            return unit;
        });
    }
    
    /// <summary>
    /// Removes an entity from the DbContext in an effectful manner.
    /// </summary>
    /// <param name="dbContext">The DbContext instance.</param>
    /// <param name="entity">The entity to be removed.</param>
    /// <typeparam name="TEntity">The type of the entity to be removed.</typeparam>
    /// <returns>An Eff&lt;EntityEntry&lt;TEntity&gt;&gt; representing the effectful operation. The result is an EntityEntry indicating the operation has been performed.</returns>
    public static Eff<EntityEntry<TEntity>> RemoveEff<TEntity>(
        this DbContext dbContext, TEntity entity)
        where TEntity : class
    {
        return
            liftEff(() => dbContext.Remove(entity));
    }
    
    /// <summary>
    /// Removes a range of entities from the DbContext in an effectful manner.
    /// </summary>
    /// <param name="dbContext">The DbContext instance.</param>
    /// <param name="entities">The entities to be removed.</param>
    /// <typeparam name="TEntity">The type of the entities to be removed.</typeparam>
    /// <returns>An Eff&lt;Unit&gt; representing the effectful operation. The result is a Unit indicating the operation has been performed.</returns>
    public static Eff<Unit> RemoveRangeEff<TEntity>(
        this DbContext dbContext, params TEntity[] entities)
        where TEntity : class
    {
        return liftEff(() =>
        {
            dbContext.RemoveRange(entities);

            return unit;
        });
    }
    
    /// <summary>
    /// Removes a range of entities from the DbContext in an effectful manner.
    /// </summary>
    /// <param name="dbContext">The DbContext instance.</param>
    /// <param name="entities">The entities to be removed.</param>
    /// <typeparam name="TEntity">The type of the entities to be removed.</typeparam>
    /// <returns>An Eff&lt;Unit&gt; representing the effectful operation. The result is a Unit indicating the operation has been performed.</returns>
    public static Eff<Unit> RemoveRangeEff<TEntity>(
        this DbContext dbContext, IEnumerable<TEntity> entities)
        where TEntity : class
    {
        return liftEff(() =>
        {
            dbContext.RemoveRange(entities);

            return unit;
        });
    }
}