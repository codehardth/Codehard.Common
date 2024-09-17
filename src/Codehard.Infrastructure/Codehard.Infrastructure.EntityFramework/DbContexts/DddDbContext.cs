using Codehard.Common.DomainModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Codehard.Infrastructure.EntityFramework.DbContexts;

/// <summary>
/// Base class for DbContexts that enforce the use of aggregate roots.
/// </summary>
public class DddDbContext : DbContext
{
    /// <summary>
    /// Create a new instance of <see cref="DddDbContext"/>.
    /// </summary>
    /// <param name="options"></param>
    protected DddDbContext(DbContextOptions options) : base(options) { }

    /// <summary>
    /// Create a DbSet for an aggregate root entity.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public new DbSet<TEntity> Set<TEntity>() where TEntity : class
    {
        if (!typeof(IAggregateRoot<>).IsAssignableFrom(typeof(TEntity)))
        {
            throw new InvalidOperationException($"Cannot create DbSet for {typeof(TEntity).Name} as it is not an aggregate root.");
        }
        return base.Set<TEntity>();
    }
    
    /// <summary>
    /// Adds the specified aggregate root entity to the context.
    /// </summary>
    /// <param name="entity"></param>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public override EntityEntry<TEntity> Add<TEntity>(TEntity entity)
    {
        if (!typeof(IAggregateRoot<>).IsAssignableFrom(typeof(TEntity)))
        {
            throw new InvalidOperationException($"Cannot add {typeof(TEntity).Name} directly as it is not an aggregate root.");
        }
        return base.Add(entity);
    }
    
    /// <summary>
    /// Adds the given aggregate root entity to the context.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public override EntityEntry Add(object entity)
    {
        if (!typeof(IAggregateRoot<>).IsAssignableFrom(entity.GetType()))
        {
            throw new InvalidOperationException($"Cannot add {entity.GetType().Name} directly as it is not an aggregate root.");
        }
        return base.Add(entity);
    }
    
    /// <summary>
    /// Asynchronously adds the specified aggregate root entity to the context.
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public override ValueTask<EntityEntry> AddAsync(
        object entity,
        CancellationToken cancellationToken = default)
    {
        if (!typeof(IAggregateRoot<>).IsAssignableFrom(entity.GetType()))
        {
            throw new InvalidOperationException($"Cannot add {entity.GetType().Name} directly as it is not an aggregate root.");
        }
        
        return base.AddAsync(entity, cancellationToken);
    }
    
    /// <summary>
    /// Asynchronously adds the specified aggregate root entity to the context.
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public override ValueTask<EntityEntry<TEntity>> AddAsync<TEntity>(
        TEntity entity,
        CancellationToken cancellationToken = default)
    {
        if (!typeof(IAggregateRoot<>).IsAssignableFrom(typeof(TEntity)))
        {
            throw new InvalidOperationException($"Cannot add {typeof(TEntity).Name} directly as it is not an aggregate root.");
        }
        return base.AddAsync(entity, cancellationToken);
    }
    
    /// <summary>
    /// Updates the given aggregate root entity in the context.
    /// </summary>
    /// <param name="entity"></param>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public override EntityEntry<TEntity> Update<TEntity>(TEntity entity)
    {
        if (!typeof(IAggregateRoot<>).IsAssignableFrom(typeof(TEntity)))
        {
            throw new InvalidOperationException($"Cannot update {typeof(TEntity).Name} directly as it is not an aggregate root.");
        }
        return base.Update(entity);
    }
    
    /// <summary>
    /// Updates the given aggregate root entity in the context.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public override EntityEntry Update(object entity)
    {
        if (!typeof(IAggregateRoot<>).IsAssignableFrom(entity.GetType()))
        {
            throw new InvalidOperationException($"Cannot update {entity.GetType().Name} directly as it is not an aggregate root.");
        }
        return base.Update(entity);
    }

    /// <summary>
    /// Removes the given aggregate root entity from the context.
    /// </summary>
    /// <param name="entity"></param>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public override EntityEntry<TEntity> Remove<TEntity>(TEntity entity)
    {
        if (!typeof(IAggregateRoot<>).IsAssignableFrom(typeof(TEntity)))
        {
            throw new InvalidOperationException($"Cannot remove {typeof(TEntity).Name} directly as it is not an aggregate root.");
        }
        
        return base.Remove(entity);
    }

    /// <summary>
    /// Removes the given aggregate root entity from the context.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public override EntityEntry Remove(object entity)
    {
        if (!typeof(IAggregateRoot<>).IsAssignableFrom(entity.GetType()))
        {
            throw new InvalidOperationException($"Cannot remove {entity.GetType().Name} directly as it is not an aggregate root.");
        }
        return base.Remove(entity);
    }
    
    /// <summary>
    /// Adds the given aggregate root entities to the context.
    /// </summary>
    /// <param name="entities"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public override void AddRange(params object[] entities)
    {
        foreach (var entity in entities)
        {
            if (!typeof(IAggregateRoot<>).IsAssignableFrom(entity.GetType()))
            {
                throw new InvalidOperationException($"Cannot add {entity.GetType().Name} directly as it is not an aggregate root.");
            }
        }
        base.AddRange(entities);
    }

    /// <summary>
    /// Adds the given aggregate root entities to the context.
    /// </summary>
    /// <param name="entities"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public override void AddRange(IEnumerable<object> entities)
    {
        foreach (var entity in entities)
        {
            if (!typeof(IAggregateRoot<>).IsAssignableFrom(entity.GetType()))
            {
                throw new InvalidOperationException($"Cannot add {entity.GetType().Name} directly as it is not an aggregate root.");
            }
        }
        base.AddRange(entities);
    }
    
    /// <summary>
    /// Asynchronously adds the given aggregate root entities to the context.
    /// </summary>
    /// <param name="entities"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public override Task AddRangeAsync(params object[] entities)
    {
        foreach (var entity in entities)
        {
            if (!typeof(IAggregateRoot<>).IsAssignableFrom(entity.GetType()))
            {
                throw new InvalidOperationException($"Cannot add {entity.GetType().Name} directly as it is not an aggregate root.");
            }
        }
        return base.AddRangeAsync(entities);
    }
    
    /// <summary>
    /// Asynchronously adds the given aggregate root entities to the context.
    /// </summary>
    /// <param name="entities"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public override Task AddRangeAsync(IEnumerable<object> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            if (!typeof(IAggregateRoot<>).IsAssignableFrom(entity.GetType()))
            {
                throw new InvalidOperationException($"Cannot add {entity.GetType().Name} directly as it is not an aggregate root.");
            }
        }
        return base.AddRangeAsync(entities, cancellationToken);
    }

    /// <summary>
    /// Update the given aggregate root entities to the context.
    /// </summary>
    /// <param name="entities"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public override void UpdateRange(params object[] entities)
    {
        foreach (var entity in entities)
        {
            if (!typeof(IAggregateRoot<>).IsAssignableFrom(entity.GetType()))
            {
                throw new InvalidOperationException($"Cannot update {entity.GetType().Name} directly as it is not an aggregate root.");
            }
        }
        base.UpdateRange(entities);
    }

    /// <summary>
    /// Update the given aggregate root entities to the context.
    /// </summary>
    /// <param name="entities"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public override void UpdateRange(IEnumerable<object> entities)
    {
        foreach (var entity in entities)
        {
            if (!typeof(IAggregateRoot<>).IsAssignableFrom(entity.GetType()))
            {
                throw new InvalidOperationException($"Cannot update {entity.GetType().Name} directly as it is not an aggregate root.");
            }
        }
        base.UpdateRange(entities);
    }

    /// <summary>
    /// Remove the given aggregate root entities to the context.
    /// </summary>
    /// <param name="entities"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public override void RemoveRange(params object[] entities)
    {
        foreach (var entity in entities)
        {
            if (!typeof(IAggregateRoot<>).IsAssignableFrom(entity.GetType()))
            {
                throw new InvalidOperationException($"Cannot remove {entity.GetType().Name} directly as it is not an aggregate root.");
            }
        }
        base.RemoveRange(entities);
    }

    /// <summary>
    /// Remove the given aggregate root entities to the context.
    /// </summary>
    /// <param name="entities"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public override void RemoveRange(IEnumerable<object> entities)
    {
        foreach (var entity in entities)
        {
            if (!typeof(IAggregateRoot<>).IsAssignableFrom(entity.GetType()))
            {
                throw new InvalidOperationException($"Cannot remove {entity.GetType().Name} directly as it is not an aggregate root.");
            }
        }
        base.RemoveRange(entities);
    }
}