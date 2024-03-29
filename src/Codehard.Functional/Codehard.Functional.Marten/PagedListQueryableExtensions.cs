using LanguageExt;

using static LanguageExt.Prelude;

// ReSharper disable once CheckNamespace
namespace Marten.Pagination;

public static class PagedListQueryableExtensions
{
    /// <summary>
    /// Asynchronously converts an IQueryable&lt;T&gt; into a paginated list within an Aff monad.
    /// </summary>
    /// <typeparam name="T">The type of elements in the IQueryable.</typeparam>
    /// <param name="queryable">The IQueryable to be paginated.</param>
    /// <param name="pageNumber">The one-based index of the page to retrieve.</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="token">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>An Aff&lt;IPagedList&lt;T&gt;&gt; representing the asynchronous operation. 
    /// The Aff monad wraps the result, which is the paginated list of elements.</returns>
    public static Aff<IPagedList<T>> ToPagedListAff<T>(
        this IQueryable<T> queryable,
        int pageNumber,
        int pageSize,
        CancellationToken token = default)
    {
        return Aff(async () => await queryable.ToPagedListAsync(pageNumber, pageSize, token));
    }
}