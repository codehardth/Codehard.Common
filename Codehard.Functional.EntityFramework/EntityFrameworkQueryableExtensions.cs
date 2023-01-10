using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Codehard.Functional.EntityFramework;

public static class EntityFrameworkQueryableExtensions
{
    /// <summary>
    /// If sequence has no element, returns None instead of true.
    /// </summary>
    public static async Task<Option<bool>> AllIfAnyAsync<TSource>(
        this IQueryable<TSource> source,
        Expression<Func<TSource, bool>> predicate)
    {
        var isAny = await source.AnyAsync();

        return
            await (
                isAny
                    ? source.AllAsync(predicate).Map(Some)
                    : Option<bool>.None.AsTask());
    }
}