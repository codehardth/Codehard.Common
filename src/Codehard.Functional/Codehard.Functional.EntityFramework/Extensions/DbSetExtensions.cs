using LanguageExt;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using static LanguageExt.Prelude;

// ReSharper disable once CheckNamespace
namespace Microsoft.EntityFrameworkCore;

/// <summary>
/// Provides extension methods for the DbSet class.
/// </summary>
public static class DbSetExtensions
{
    /// <summary>
    /// Adds an entity to the DbSet in an effectful manner.
    /// </summary>
    /// <param name="dbSet">The DbSet instance.</param>
    /// <param name="entity">The entity to be added.</param>
    /// <typeparam name="TEntity">The type of the entity to be added.</typeparam>
    /// <returns>An Eff&lt;EntityEntry&lt;TEntity&gt;&gt; representing the effectful operation. The result is an EntityEntry indicating the operation has been performed.</returns>
    public static Eff<EntityEntry<TEntity>> AddEff<TEntity>(
        this DbSet<TEntity> dbSet, TEntity entity)
        where TEntity : class
    {
        return liftEff(() => dbSet.Add(entity));
    }
    
    /// <summary>
    /// Adds a range of entities to the DbSet in an effectful manner.
    /// </summary>
    /// <param name="dbSet">The DbSet instance.</param>
    /// <param name="entities">The entities to be added.</param>
    /// <typeparam name="TEntity">The type of the entities to be added.</typeparam>
    /// <returns>An Eff&lt;Unit&gt; representing the effectful operation. The result is a Unit indicating the operation has been performed.</returns>
    public static Eff<Unit> AddRangeEff<TEntity>(
        this DbSet<TEntity> dbSet, params TEntity[] entities)
        where TEntity : class
    {
        return liftEff(() =>
        {
            dbSet.AddRange(entities);

            return unit;
        });
    }
    
    /// <summary>
    /// Adds a range of entities to the DbSet in an effectful manner.
    /// </summary>
    /// <param name="dbSet">The DbSet instance.</param>
    /// <param name="entities">The entities to be added.</param>
    /// <typeparam name="TEntity">The type of the entities to be added.</typeparam>
    /// <returns>An Eff&lt;Unit&gt; representing the effectful operation. The result is a Unit indicating the operation has been performed.</returns>
    public static Eff<Unit> AddRangeEff<TEntity>(
        this DbSet<TEntity> dbSet, IEnumerable<TEntity> entities)
        where TEntity : class
    {
        return liftEff(() =>
        {
            dbSet.AddRange(entities);

            return unit;
        });
    }
    
    /// <summary>
    /// Updates an entity in the DbSet in an effectful manner.
    /// </summary>
    /// <param name="dbSet">The DbSet instance.</param>
    /// <param name="entity">The entity to be updated.</param>
    /// <typeparam name="TEntity">The type of the entity to be updated.</typeparam>
    /// <returns>An Eff&lt;EntityEntry&lt;TEntity&gt;&gt; representing the effectful operation. The result is an EntityEntry indicating the operation has been performed.</returns>
    public static Eff<EntityEntry<TEntity>> UpdateEff<TEntity>(
        this DbSet<TEntity> dbSet, TEntity entity)
        where TEntity : class
    {
        return liftEff(() => dbSet.Update(entity));
    }
    
    /// <summary>
    /// Updates a range of entities in the DbSet in an effectful manner.
    /// </summary>
    /// <param name="dbSet">The DbSet instance.</param>
    /// <param name="entities">The entities to be updated.</param>
    /// <typeparam name="TEntity">The type of the entities to be updated.</typeparam>
    /// <returns>An Eff&lt;Unit&gt; representing the effectful operation. The result is a Unit indicating the operation has been performed.</returns>
    public static Eff<Unit> UpdateRangeEff<TEntity>(
        this DbSet<TEntity> dbSet, params TEntity[] entities)
        where TEntity : class
    {
        return liftEff(() =>
        {
            dbSet.UpdateRange(entities);

            return unit;
        });
    }
    
    /// <summary>
    /// Updates a range of entities in the DbSet in an effectful manner.
    /// </summary>
    /// <param name="dbSet">The DbSet instance.</param>
    /// <param name="entities">The entities to be updated.</param>
    /// <typeparam name="TEntity">The type of the entities to be updated.</typeparam>
    /// <returns>An Eff&lt;Unit&gt; representing the effectful operation. The result is a Unit indicating the operation has been performed.</returns>
    public static Eff<Unit> UpdateRangeEff<TEntity>(
        this DbSet<TEntity> dbSet, IEnumerable<TEntity> entities)
        where TEntity : class
    {
        return liftEff(() =>
        {
            dbSet.UpdateRange(entities);

            return unit;
        });
    }
    
    /// <summary>
    /// Removes an entity from the DbSet in an effectful manner.
    /// </summary>
    /// <param name="dbSet">The DbSet instance.</param>
    /// <param name="entity">The entity to be removed.</param>
    /// <typeparam name="TEntity">The type of the entity to be removed.</typeparam>
    /// <returns>An Eff&lt;EntityEntry&lt;TEntity&gt;&gt; representing the effectful operation. The result is an EntityEntry indicating the operation has been performed.</returns>
    public static Eff<EntityEntry<TEntity>> RemoveEff<TEntity>(
        this DbSet<TEntity> dbSet, TEntity entity)
        where TEntity : class
    {
        return liftEff(() => dbSet.Remove(entity));
    }
    
    /// <summary>
    /// Removes a range of entities from the DbSet in an effectful manner.
    /// </summary>
    /// <param name="dbSet">The DbSet instance.</param>
    /// <param name="entities">The entities to be removed.</param>
    /// <typeparam name="TEntity">The type of the entities to be removed.</typeparam>
    /// <returns>An Eff&lt;Unit&gt; representing the effectful operation. The result is a Unit indicating the operation has been performed.</returns>
    public static Eff<Unit> RemoveRangeEff<TEntity>(
        this DbSet<TEntity> dbSet, params TEntity[] entities)
        where TEntity : class
    {
        return liftEff(() =>
        {
            dbSet.RemoveRange(entities);

            return unit;
        });
    }
    
    /// <summary>
    /// Removes a range of entities from the DbSet in an effectful manner.
    /// </summary>
    /// <param name="dbSet">The DbSet instance.</param>
    /// <param name="entities">The entities to be removed.</param>
    /// <typeparam name="TEntity">The type of the entities to be removed.</typeparam>
    /// <returns>An Eff&lt;Unit&gt; representing the effectful operation. The result is a Unit indicating the operation has been performed.</returns>
    public static Eff<Unit> RemoveRangeEff<TEntity>(
        this DbSet<TEntity> dbSet, IEnumerable<TEntity> entities)
        where TEntity : class
    {
        return liftEff(() =>
        {
            dbSet.RemoveRange(entities);

            return unit;
        });
    }
}