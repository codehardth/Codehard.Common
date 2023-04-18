using System.Collections;
using System.Collections.Immutable;
using Codehard.Common.DomainModel;
using Microsoft.EntityFrameworkCore;

namespace Codehard.Infrastructure.EntityFramework;

/// <inheritdoc />
public sealed class PaginatedList<T> : IPaginatedList<T>
{
    private readonly int count;
    private readonly int page;
    private readonly int size;

    private readonly IReadOnlyList<T> source;

    private PaginatedList(IReadOnlyList<T> source, int count, int page, int size)
    {
        this.count = count;
        this.page = page;
        this.size = size;

        this.source = source;
    }

    /// <inheritdoc />
    public int CurrentPage => this.page;

    /// <inheritdoc />
    public int TotalPages => (int)Math.Ceiling(this.count / (double)this.size);

    /// <inheritdoc />
    public int PageSize => this.size;

    /// <inheritdoc />
    public int TotalCount => this.count;

    /// <inheritdoc />
    public bool HasPrevious => this.page > 1;

    /// <inheritdoc />
    public bool HasNext => this.page < this.TotalPages;

    /// <inheritdoc />
    public int Count => this.source.Count;

    /// <inheritdoc />
    public T this[int index] => this.source[index];
    
    /// <summary>
    /// Creates a new instance of the <see cref="PaginatedList{T}"/> class.
    /// </summary>
    /// <param name="query">The queryable source of items.</param>
    /// <param name="page">The current page number.</param>
    /// <param name="size">The size of each page.</param>
    /// <returns>A new instance of the <see cref="PaginatedList{T}"/> class.</returns>
    public static IPaginatedList<T> Create(
        IQueryable<T> query,
        int page,
        int size)
    {
        var count = query.Count();
        var items = query.Skip(size * (page - 1)).Take(size).ToImmutableList();

        return new PaginatedList<T>(items, count, page, size);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="PaginatedList{T}"/> class.
    /// </summary>
    /// <param name="source">The source list of items.</param>
    /// <param name="count">The total number of items in the source list.</param>
    /// <param name="page">The current page number.</param>
    /// <param name="size">The size of each page.</param>
    /// <returns>A new instance of the <see cref="PaginatedList{T}"/> class.</returns>
    public static IPaginatedList<T> Create(
        IEnumerable<T> source,
        int count,
        int page,
        int size)
    {
        return new PaginatedList<T>(source.ToImmutableArray(), count, page, size);
    }

    /// <summary>
    /// Creates a new instance of <see cref="PaginatedList{T}"/> with the specified page size and page number.
    /// </summary>
    /// <param name="query">The queryable data to paginate.</param>
    /// <param name="page">The page number to retrieve. Must be greater than or equal to one.</param>
    /// <param name="size">The number of items to include in each page. Must be greater than zero.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation if necessary.</param>
    /// <returns>A new instance of <see cref="PaginatedList{T}"/> with the requested page and page size.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="page"/> is less than one or <paramref name="size"/> is less than or equal to zero.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="query"/> is null.</exception>
    public static async Task<IPaginatedList<T>> CreateAsync(
        IQueryable<T> query,
        int page,
        int size,
        CancellationToken cancellationToken = default)
    {
        var count = await query.CountAsync(cancellationToken);
        var items = await query.Skip(size * (page - 1)).Take(size).ToListAsync(cancellationToken);

        return new PaginatedList<T>(items, count, page, size);
    }

    /// <inheritdoc />
    public IEnumerator<T> GetEnumerator()
    {
        return this.source.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}