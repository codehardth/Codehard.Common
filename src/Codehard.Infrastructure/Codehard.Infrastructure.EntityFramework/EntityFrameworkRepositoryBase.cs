using System.Collections;
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
    /// <summary>
    /// The DbContext object used by the repository to interact with the database.
    /// </summary>
    protected readonly DbContext dbContext;

    /// <summary>
    /// Initializes a new instance of the EntityFrameworkRepositoryBase class with the specified DbContext object.
    /// </summary>
    /// <param name="dbContext">The DbContext object to be used by the repository.</param>
    protected EntityFrameworkRepositoryBase(DbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    /// <summary>
    /// Provides access to the DbSet of <see cref="T"/> object used by the repository to interact with the database.
    /// </summary>
    protected virtual DbSet<T> Set => this.dbContext.Set<T>();

    /// <inheritdoc />
    public virtual ValueTask<T?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
    {
        return this.GetByIdAsync(new[] { id }, cancellationToken);
    }

    /// <inheritdoc />
    public virtual ValueTask<T?> GetByIdAsync(object[] id, CancellationToken cancellationToken = default)
    {
        return this.Set.FindAsync(id, cancellationToken);
    }

    /// <inheritdoc />
    public virtual T Add(T entity)
    {
        var entityEntry = this.Set.Add(entity);

        return entityEntry.Entity;
    }

    /// <inheritdoc />
    public virtual void AddRange(IEnumerable<T> entities)
    {
        this.Set.AddRange(entities);
    }

    /// <inheritdoc />
    public virtual void Remove(T entity)
    {
        this.Set.Remove(entity);
    }

    /// <inheritdoc />
    public virtual void RemoveRange(IEnumerable<T> entities)
    {
        this.Set.RemoveRange(entities);
    }

    /// <inheritdoc />
    public virtual void Update(T entity)
    {
        this.Set.Update(entity);
    }

    /// <inheritdoc />
    public virtual void UpdateRange(IEnumerable<T> entities)
    {
        this.Set.UpdateRange(entities);
    }

    /// <inheritdoc />
    public virtual bool Any(Expression<Func<T, bool>> predicate)
    {
        return this.Set.Any(predicate);
    }

    /// <inheritdoc />
    public virtual int Count()
    {
        return this.Set.Count();
    }

    /// <inheritdoc />
    public virtual int Count(Expression<Func<T, bool>> predicate)
    {
        return this.Set.Count(predicate);
    }

    /// <inheritdoc />
    public virtual int SaveChanges()
    {
        return this.dbContext.SaveChanges();
    }

    /// <inheritdoc />
    public virtual async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        var entityEntry = await this.Set.AddAsync(entity, cancellationToken);

        return entityEntry.Entity;
    }

    /// <inheritdoc />
    public virtual Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        return this.Set.AddRangeAsync(entities, cancellationToken);
    }

    /// <inheritdoc />
    public virtual Task RemoveAsync(T entity, CancellationToken cancellationToken = default)
    {
        this.Set.Remove(entity);

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public virtual Task RemoveRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        this.Set.RemoveRange(entities);

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public virtual Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        this.Set.Update(entity);

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public virtual Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        this.Set.UpdateRange(entities);

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public virtual Task<bool> AnyAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return this.Set.AnyAsync(predicate, cancellationToken);
    }

    /// <inheritdoc />
    public virtual Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return this.Set.CountAsync(cancellationToken);
    }

    /// <inheritdoc />
    public virtual Task<int> CountAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return this.Set.CountAsync(predicate, cancellationToken);
    }

    /// <inheritdoc />
    public virtual Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return this.dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>Returns an enumerator that iterates through the collection.</summary>
    /// <returns>An enumerator that can be used to iterate through the collection.</returns>
    public IEnumerator<T> GetEnumerator()
    {
        return this.Set.AsQueryable().GetEnumerator();
    }

    /// <summary>Returns an enumerator that iterates through a collection.</summary>
    /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }

    /// <summary>Gets the type of the element(s) that are returned when the expression tree associated with this instance of <see cref="T:System.Linq.IQueryable" /> is executed.</summary>
    /// <returns>A <see cref="T:System.Type" /> that represents the type of the element(s) that are returned when the expression tree associated with this object is executed.</returns>
    public Type ElementType => typeof(T);

    /// <summary>Gets the expression tree that is associated with the instance of <see cref="T:System.Linq.IQueryable" />.</summary>
    /// <returns>The <see cref="T:System.Linq.Expressions.Expression" /> that is associated with this instance of <see cref="T:System.Linq.IQueryable" />.</returns>
    public Expression Expression => this.Set.AsQueryable().Expression;

    /// <summary>Gets the query provider that is associated with this data source.</summary>
    /// <returns>The <see cref="T:System.Linq.IQueryProvider" /> that is associated with this data source.</returns>
    public IQueryProvider Provider => this.Set.AsQueryable().Provider;
}