using System.Collections;
using System.Collections.Immutable;
using Codehard.Infrastructure.EntityFramework.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Codehard.Infrastructure.EntityFramework;

public sealed class PaginatedQuery<T> : IPaginatedQuery<T>
{
    private readonly int count;
    private readonly int page;
    private readonly int size;

    private readonly IReadOnlyList<T> source;

    private PaginatedQuery(IReadOnlyList<T> source, int count, int page, int size)
    {
        this.count = count;
        this.page = page;
        this.size = size;

        this.source = source;
    }

    public int CurrentPage => this.page;

    public int TotalPages => (int)Math.Ceiling(this.count / (double)this.size);

    public int PageSize => this.size;

    public int TotalCount => this.count;

    public bool HasPrevious => this.page > 1;

    public bool HasNext => this.page < this.TotalPages;

    public int Count => this.source.Count;

    public T this[int index] => this.source[index];

    public static IPaginatedQuery<T> Create(
        IQueryable<T> query,
        int page,
        int size)
    {
        var count = query.Count();
        var items = query.Skip(size * (page - 1)).Take(size).ToImmutableList();

        return new PaginatedQuery<T>(items, count, page, size);
    }

    public static IPaginatedQuery<T> Create(
        IEnumerable<T> source,
        int count,
        int page,
        int size)
    {
        return new PaginatedQuery<T>(source.ToImmutableArray(), count, page, size);
    }

    public static async Task<IPaginatedQuery<T>> CreateAsync(
        IQueryable<T> query,
        int page,
        int size,
        CancellationToken cancellationToken = default)
    {
        var count = await query.CountAsync(cancellationToken);
        var items = await query.Skip(size * (page - 1)).Take(size).ToListAsync(cancellationToken);

        return new PaginatedQuery<T>(items, count, page, size);
    }

    public IEnumerator<T> GetEnumerator()
    {
        return this.source.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}