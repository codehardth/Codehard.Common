using System.Linq.Expressions;
using Codehard.Infrastructure.EntityFramework.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Codehard.Infrastructure.EntityFramework;

/// <summary>
/// An abstract implementation of a repository for Entity Framework.
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class EntityFrameworkRepositoryBase<T> : IRepository<T>
    where T : class
{
    protected readonly DbContext dbContext;

    protected EntityFrameworkRepositoryBase(DbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    protected abstract DbSet<T> Set { get; }

    public virtual ValueTask<T?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
    {
        return this.GetByIdAsync(new[] { id }, cancellationToken);
    }

    public virtual ValueTask<T?> GetByIdAsync(object[] id, CancellationToken cancellationToken = default)
    {
        return this.Set.FindAsync(id, cancellationToken);
    }

    public virtual T Add(T entity)
    {
        var entityEntry = this.Set.Add(entity);

        return entityEntry.Entity;
    }

    public virtual void AddRange(IEnumerable<T> entities)
    {
        this.Set.AddRange(entities);
    }

    public virtual void Remove(T entity)
    {
        this.Set.Remove(entity);
    }

    public virtual void RemoveRange(IEnumerable<T> entities)
    {
        this.Set.RemoveRange(entities);
    }

    public virtual void Update(T entity)
    {
        this.Set.Update(entity);
    }

    public virtual void UpdateRange(IEnumerable<T> entities)
    {
        this.Set.UpdateRange(entities);
    }

    public virtual bool Any(Expression<Func<T, bool>> predicate)
    {
        return this.Set.Any(predicate);
    }

    public virtual int Count()
    {
        return this.Set.Count();
    }

    public virtual int Count(Expression<Func<T, bool>> predicate)
    {
        return this.Set.Count(predicate);
    }

    public virtual int SaveChanges()
    {
        return this.dbContext.SaveChanges();
    }

    public virtual async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        var entityEntry = await this.Set.AddAsync(entity, cancellationToken);

        return entityEntry.Entity;
    }

    public virtual Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        return this.Set.AddRangeAsync(entities, cancellationToken);
    }

    public virtual Task RemoveAsync(T entity, CancellationToken cancellationToken = default)
    {
        this.Set.Remove(entity);

        return Task.CompletedTask;
    }

    public virtual Task RemoveRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        this.Set.RemoveRange(entities);

        return Task.CompletedTask;
    }

    public virtual Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        this.Set.Update(entity);

        return Task.CompletedTask;
    }

    public virtual Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        this.Set.UpdateRange(entities);

        return Task.CompletedTask;
    }

    public virtual Task<bool> AnyAsync(Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return this.Set.AnyAsync(predicate, cancellationToken);
    }

    public virtual Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return this.Set.CountAsync(cancellationToken);
    }

    public virtual Task<int> CountAsync(Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return this.Set.CountAsync(predicate, cancellationToken);
    }

    public virtual Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return this.dbContext.SaveChangesAsync(cancellationToken);
    }
}