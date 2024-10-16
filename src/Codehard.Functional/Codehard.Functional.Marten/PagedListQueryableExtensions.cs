using LanguageExt;

using static LanguageExt.Prelude;

// ReSharper disable once CheckNamespace
namespace Marten.Pagination;

/// <summary>
/// Provides extension methods for converting an IQueryable&lt;T&gt; into a paginated list within an Eff monad.
/// </summary>
public static class PagedListQueryableExtensions
{
    /// <summary>
    /// Asynchronously converts an IQueryable&lt;T&gt; into a paginated list within an Eff monad.
    /// </summary>
    /// <typeparam name="T">The type of elements in the IQueryable.</typeparam>
    /// <param name="queryable">The IQueryable to be paginated.</param>
    /// <param name="pageNumber">The one-based index of the page to retrieve.</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <returns>An Eff&lt;IPagedList&lt;T&gt;&gt; representing the asynchronous operation. 
    /// The Eff monad wraps the result, which is the paginated list of elements.</returns>
    public static Eff<IPagedList<T>> ToPagedListEff<T>(
        this IQueryable<T> queryable,
        int pageNumber,
        int pageSize)
    {
        return liftIO(env => queryable.ToPagedListAsync(pageNumber, pageSize, env.Token));
    }
}