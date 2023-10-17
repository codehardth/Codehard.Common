namespace Codehard.Common.DomainModel;

/// <summary>
/// Defines a generic repository interface for entities of type T.
/// </summary>
/// <typeparam name="T">The type of entity to be stored in the repository.</typeparam>
public interface IRepository<T> : IQueryable<T>
    where T : class
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
    /// Queries a data using given <see cref="specification"/>.
    /// </summary>
    /// <param name="specification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    IAsyncEnumerable<T> QueryAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);

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
    /// Saves all changes made in this repository context to the database in an asynchronous manner.
    /// </summary>
    /// <returns></returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}