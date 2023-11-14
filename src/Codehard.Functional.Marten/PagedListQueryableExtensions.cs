// ReSharper disable once CheckNamespace

using LanguageExt;

using static LanguageExt.Prelude;

namespace Marten.Pagination;

public static class PagedListQueryableExtensions
{
    public static Aff<IPagedList<T>> ToPagedListAff<T>(
        this IQueryable<T> queryable,
        int pageNumber,
        int pageSize,
        CancellationToken token = default)
    {
        return Aff(async () => await queryable.ToPagedListAsync(pageNumber, pageSize, token));
    }
}