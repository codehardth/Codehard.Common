using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore;

namespace Codehard.EntityFramework;

public interface IPaginatedQuery<out T> : IReadOnlyList<T>
{
    int CurrentPage { get; }

    int TotalPages { get; }

    int PageSize { get; }

    int TotalCount { get; }

    bool HasPrevious { get; }

    bool HasNext { get; }
}

public sealed class PaginatedQuery<T> : List<T>, IPaginatedQuery<T>
{
    private readonly int count;
    private readonly int page;
    private readonly int size;

    private PaginatedQuery(IEnumerable<T> items, int count, int page, int size)
    {
        this.count = count;
        this.page = page;
        this.size = size;

        this.AddRange(items);
    }

    public int CurrentPage => this.page;

    public int TotalPages => (int)Math.Ceiling(this.count / (double)this.size);

    public int PageSize => this.size;

    public int TotalCount => this.count;

    public bool HasPrevious => this.page > 1;

    public bool HasNext => this.page < this.TotalPages;

    public static IPaginatedQuery<T> Create(IQueryable<T> query, int page, int size)
    {
        var count = query.Count();
        var items = query.Skip(size * (page - 1)).Take(size).ToImmutableList();

        return new PaginatedQuery<T>(items, count, page, size);
    }

    public static IPaginatedQuery<T> Create(IEnumerable<T> source, int count, int page, int size)
    {
        return new PaginatedQuery<T>(source, count, page, size);
    }

    public static async Task<IPaginatedQuery<T>> CreateAsync(IQueryable<T> query, int page, int size, CancellationToken cancellationToken = default)
    {
        var count = await query.CountAsync(cancellationToken);
        var items = await query.Skip(size * (page - 1)).Take(size).ToListAsync(cancellationToken);

        return new PaginatedQuery<T>(items, count, page, size);
    }
}