using System.Linq.Expressions;
using Codehard.Infrastructure.EntityFramework.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Codehard.Infrastructure.EntityFramework;

public abstract class EntityFrameworkRepositoryBase<T> : IRepository<T>
    where T : class
{
    private readonly DbContext dbContext;

    protected EntityFrameworkRepositoryBase(DbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    protected abstract DbSet<T> Set { get; }

    public ValueTask<T?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
    {
        return this.GetByIdAsync(new[] { id }, cancellationToken);
    }

    public ValueTask<T?> GetByIdAsync(object[] id, CancellationToken cancellationToken = default)
    {
        return this.Set.FindAsync(id, cancellationToken);
    }

    public T Add(T entity)
    {
        var entityEntry = this.Set.Add(entity);

        return entityEntry.Entity;
    }

    public void AddRange(IEnumerable<T> entities)
    {
        this.Set.AddRange(entities);
    }

    public void Remove(T entity)
    {
        this.Set.Remove(entity);
    }

    public void RemoveRange(IEnumerable<T> entities)
    {
        this.Set.RemoveRange(entities);
    }

    public void Update(T entity)
    {
        this.Set.Update(entity);
    }

    public void UpdateRange(IEnumerable<T> entities)
    {
        this.Set.UpdateRange(entities);
    }

    public bool Any(Expression<Func<T, bool>> predicate)
    {
        return this.Set.Any(predicate);
    }

    public int Count()
    {
        return this.Set.Count();
    }

    public int Count(Expression<Func<T, bool>> predicate)
    {
        return this.Set.Count(predicate);
    }

    public int SaveChanges()
    {
        return this.dbContext.SaveChanges();
    }

    public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        var entityEntry = await this.Set.AddAsync(entity, cancellationToken);

        return entityEntry.Entity;
    }

    public Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        return this.Set.AddRangeAsync(entities, cancellationToken);
    }

    public Task RemoveAsync(T entity, CancellationToken cancellationToken = default)
    {
        this.Set.Remove(entity);

        return Task.CompletedTask;
    }

    public Task RemoveRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        this.Set.RemoveRange(entities);

        return Task.CompletedTask;
    }

    public Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        this.Set.Update(entity);

        return Task.CompletedTask;
    }

    public Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        this.Set.UpdateRange(entities);

        return Task.CompletedTask;
    }

    public Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return this.Set.AnyAsync(predicate, cancellationToken);
    }

    public Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return this.Set.CountAsync(cancellationToken);
    }

    public Task<int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return this.Set.CountAsync(predicate, cancellationToken);
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return this.dbContext.SaveChangesAsync(cancellationToken);
    }
}