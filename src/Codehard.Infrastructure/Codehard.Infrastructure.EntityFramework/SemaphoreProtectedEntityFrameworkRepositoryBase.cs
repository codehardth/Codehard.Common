using System.Collections;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Codehard.Common.DomainModel;
using Codehard.Common.DomainModel.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Codehard.Infrastructure.EntityFramework;

/// <summary>
/// An abstract implementation of a repository for Entity Framework with semaphore protection for database operations.
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class SemaphoreProtectedEntityFrameworkRepositoryBase<T> : IRepository<T>
    where T : class
{
    /// <summary>
    /// The DbContext object used by the repository to interact with the database.
    /// </summary>
    protected readonly DbContext DbContext;

    /// <summary>
    /// Semaphore to protect concurrent database access
    /// </summary>
    private readonly SemaphoreSlim semaphore = new(1, 1);

    /// <summary>
    /// Initializes a new instance of the EntityFrameworkRepositoryBase class with the specified DbContext object.
    /// </summary>
    /// <param name="dbContext">The DbContext object to be used by the repository.</param>
    protected SemaphoreProtectedEntityFrameworkRepositoryBase(DbContext dbContext)
    {
        this.DbContext = dbContext;
    }

    /// <summary>
    /// Provides access to the DbSet of <typeparamref name="T"/> object used by the repository to interact with the database.
    /// </summary>
    protected virtual DbSet<T> Set => this.DbContext.Set<T>();

    /// <inheritdoc />
    public virtual async ValueTask<T?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
    {
        await semaphore.WaitAsync(cancellationToken);
        try
        {
            return await this.GetByIdAsync(new[] { id }, cancellationToken);
        }
        finally
        {
            semaphore.Release();
        }
    }

    /// <inheritdoc />
    public virtual async ValueTask<T?> GetByIdAsync(object[] id, CancellationToken cancellationToken = default)
    {
        await semaphore.WaitAsync(cancellationToken);
        try
        {
            return await this.Set.FindAsync(id, cancellationToken);
        }
        finally
        {
            semaphore.Release();
        }
    }

    /// <inheritdoc />
    public async IAsyncEnumerable<T> QueryAsync(
        ISpecification<T> specification,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await semaphore.WaitAsync(cancellationToken);
        try
        {
            var query = this.Set.Apply(specification);
            var source = query.AsAsyncEnumerable().WithCancellation(cancellationToken);

            await foreach (var e in source)
            {
                yield return e;
            }
        }
        finally
        {
            semaphore.Release();
        }
    }

    /// <inheritdoc />
    public virtual async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await semaphore.WaitAsync(cancellationToken);
        try
        {
            var entityEntry = await this.Set.AddAsync(entity, cancellationToken);
            return entityEntry.Entity;
        }
        finally
        {
            semaphore.Release();
        }
    }

    /// <inheritdoc />
    public virtual async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        await semaphore.WaitAsync(cancellationToken);
        try
        {
            await this.Set.AddRangeAsync(entities, cancellationToken);
        }
        finally
        {
            semaphore.Release();
        }
    }

    /// <inheritdoc />
    public virtual async Task RemoveAsync(T entity, CancellationToken cancellationToken = default)
    {
        await semaphore.WaitAsync(cancellationToken);
        try
        {
            this.Set.Remove(entity);
        }
        finally
        {
            semaphore.Release();
        }
    }

    /// <inheritdoc />
    public virtual async Task RemoveRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        await semaphore.WaitAsync(cancellationToken);
        try
        {
            this.Set.RemoveRange(entities);
        }
        finally
        {
            semaphore.Release();
        }
    }

    /// <inheritdoc />
    public virtual async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        await semaphore.WaitAsync(cancellationToken);
        try
        {
            this.Set.Update(entity);
        }
        finally
        {
            semaphore.Release();
        }
    }

    /// <inheritdoc />
    public virtual async Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        await semaphore.WaitAsync(cancellationToken);
        try
        {
            this.Set.UpdateRange(entities);
        }
        finally
        {
            semaphore.Release();
        }
    }

    /// <inheritdoc />
    public virtual async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await semaphore.WaitAsync(cancellationToken);
        try
        {
            return await this.DbContext.SaveChangesAsync(cancellationToken);
        }
        finally
        {
            semaphore.Release();
        }
    }

    /// <inheritdoc />
    public virtual T Add(T entity)
    {
        semaphore.Wait();
        try
        {
            var entityEntry = this.Set.Add(entity);
            return entityEntry.Entity;
        }
        finally
        {
            semaphore.Release();
        }
    }

    /// <inheritdoc />
    public virtual void AddRange(IEnumerable<T> entities)
    {
        semaphore.Wait();
        try
        {
            this.Set.AddRange(entities);
        }
        finally
        {
            semaphore.Release();
        }
    }

    /// <inheritdoc />
    public virtual void Remove(T entity)
    {
        semaphore.Wait();
        try
        {
            this.Set.Remove(entity);
        }
        finally
        {
            semaphore.Release();
        }
    }

    /// <inheritdoc />
    public virtual void RemoveRange(IEnumerable<T> entities)
    {
        semaphore.Wait();
        try
        {
            this.Set.RemoveRange(entities);
        }
        finally
        {
            semaphore.Release();
        }
    }

    /// <inheritdoc />
    public virtual void Update(T entity)
    {
        semaphore.Wait();
        try
        {
            this.Set.Update(entity);
        }
        finally
        {
            semaphore.Release();
        }
    }

    /// <inheritdoc />
    public virtual void UpdateRange(IEnumerable<T> entities)
    {
        semaphore.Wait();
        try
        {
            this.Set.UpdateRange(entities);
        }
        finally
        {
            semaphore.Release();
        }
    }

    /// <inheritdoc />
    public virtual int SaveChanges()
    {
        semaphore.Wait();
        try
        {
            return this.DbContext.SaveChanges();
        }
        finally
        {
            semaphore.Release();
        }
    }

    /// <inheritdoc />
    public IEnumerator<T> GetEnumerator()
    {
        semaphore.Wait();
        try
        {
            return this.Set.AsQueryable().GetEnumerator();
        }
        finally
        {
            semaphore.Release();
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }

    /// <inheritdoc />
    public Type ElementType => typeof(T);

    /// <inheritdoc />
    public Expression Expression => this.Set.AsQueryable().Expression;

    /// <inheritdoc />
    public IQueryProvider Provider => this.Set.AsQueryable().Provider;
}