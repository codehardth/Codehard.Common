using System.Linq.Expressions;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

using static LanguageExt.Prelude;

namespace Codehard.Functional.EntityFramework;

public static class QueryableExtensions
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
    
    public static Task<Option<TSource>> SingleOrNoneAsync<TSource>(
        this IQueryable<TSource> source,
        CancellationToken ct = default)
    {
        return
            source.SingleOrDefaultAsync(ct)
                  .Map(Optional);
    }

    public static Aff<Option<TSource>> SingleOrNoneAff<TSource>(
        this IQueryable<TSource> source,
        CancellationToken ct = default)
    {
        return
            Aff(async () => await source.SingleOrNoneAsync(ct));
    }
    
    public static Task<Option<TSource>> SingleOrNoneAsync<TSource>(
        this IQueryable<TSource> source,
        Expression<Func<TSource, bool>> predicate,
        CancellationToken ct = default)
    {
        return
            source.SingleOrDefaultAsync(predicate, ct)
                .Map(Optional);
    }
    
    public static Task<Option<T>> FirstOrNoneAsync<T>(
        this IQueryable<T> source, CancellationToken ct = default)
    {
        return source.FirstOrDefaultAsync(ct).Map(Optional);
    }
    
    public static Aff<Option<T>> FirstOrNoneAff<T>(
        this IQueryable<T> source, CancellationToken ct = default)
    {
        return 
            Aff(async () => await source.FirstOrNoneAsync(ct));
    }
    
    public static Task<Option<T>> FirstOrNoneAsync<T>(
        this IQueryable<T> source,
        Expression<Func<T, bool>> predicate,
        CancellationToken ct = default)
    {
        return source
            .FirstOrDefaultAsync(
                predicate,
                ct)
            .Map(Optional);
    }
}