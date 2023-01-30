using System.Linq.Expressions;

namespace Codehard.Infrastructure.EntityFramework.Interfaces;

public interface IRepository<T> where T : class
{
    /// <summary>
    /// Get an entity using its identity in an asynchronous manner.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    ValueTask<T?> GetByIdAsync(object id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get an entity using its identities in an asynchronous manner.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    ValueTask<T?> GetByIdAsync(object[] id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Add an entity.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    T Add(T entity);

    /// <summary>
    /// Add an entities.
    /// </summary>
    /// <param name="entities"></param>
    void AddRange(IEnumerable<T> entities);

    /// <summary>
    /// Remove an entity.
    /// </summary>
    /// <param name="entity"></param>
    void Remove(T entity);

    /// <summary>
    /// Remove an entities.
    /// </summary>
    /// <param name="entities"></param>
    void RemoveRange(IEnumerable<T> entities);

    /// <summary>
    /// Update an entity.
    /// </summary>
    /// <param name="entity"></param>
    void Update(T entity);

    /// <summary>
    /// Update an entities.
    /// </summary>
    /// <param name="entities"></param>
    void UpdateRange(IEnumerable<T> entities);

    /// <summary>
    /// Determines whether any entity satisfies a condition.
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    bool Any(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Returns the number of entities in a repository.
    /// </summary>
    /// <returns></returns>
    int Count();

    /// <summary>
    /// Returns the number of entities in a repository that satisfies a condition.
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    int Count(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Saves all changes made in this repository context to the database.
    /// </summary>
    /// <returns></returns>
    int SaveChanges();

    /// <summary>
    /// Add an entity in an asynchronous manner.
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Add an entities in an asynchronous manner.
    /// </summary>
    /// <param name="entities"></param>
    /// <param name="cancellationToken"></param>
    Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

    /// <summary>
    /// Remove an entity in an asynchronous manner.
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="cancellationToken"></param>
    Task RemoveAsync(T entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Remove an entities in an asynchronous manner.
    /// </summary>
    /// <param name="entities"></param>
    /// <param name="cancellationToken"></param>
    Task RemoveRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update an entity in an asynchronous manner.
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="cancellationToken"></param>
    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update an entities in an asynchronous manner.
    /// </summary>
    /// <param name="entities"></param>
    /// <param name="cancellationToken"></param>
    Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

    /// <summary>
    /// Determines whether any entity satisfies a condition in an asynchronous manner.
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the number of entities in a repository in an asynchronous manner.
    /// </summary>
    /// <returns></returns>
    Task<int> CountAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the number of entities in a repository that satisfies a condition in an asynchronous manner.
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Saves all changes made in this repository context to the database in an asynchronous manner.
    /// </summary>
    /// <returns></returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}