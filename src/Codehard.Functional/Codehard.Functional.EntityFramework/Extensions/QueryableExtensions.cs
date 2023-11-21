using System.Linq.Expressions;
using LanguageExt;

using static LanguageExt.Prelude;

// ReSharper disable once CheckNamespace
namespace Microsoft.EntityFrameworkCore;

public static class QueryableExtensions
{
    public static Aff<List<T>> ToListAff<T>(
        this IQueryable<T> source, CancellationToken ct = default)
    {
        return Aff(async () => await source.ToListAsync(ct));
    }
    
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
    
    /// <summary>
    /// Asynchronously returns the only element of a sequence, or a None value if the sequence is empty;
    /// this method returns an Option&lt;TSource&gt;.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the sequence.</typeparam>
    /// <param name="source">The IQueryable&lt;TSource&gt; to get the single element from.</param>
    /// <param name="ct">The CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>
    /// A Task&lt;Option&lt;TSource&gt;&gt; that represents the asynchronous operation. 
    /// The task result contains the only element of the sequence, or a None value if the sequence is empty.
    /// </returns>
    public static Task<Option<TSource>> SingleOrNoneAsync<TSource>(
        this IQueryable<TSource> source,
        CancellationToken ct = default)
    {
        return
            source.SingleOrDefaultAsync(ct)
                  .Map(Optional);
    }

    /// <summary>
    /// Asynchronously returns the only element of a sequence as an Option&lt;TSource&gt; within an Aff monad,
    /// or a None value if the sequence is empty.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the sequence.</typeparam>
    /// <param name="source">The IQueryable&lt;TSource&gt; to get the single element from.</param>
    /// <param name="ct">The CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>
    /// An Aff&lt;Option&lt;TSource&gt;&gt; that represents the asynchronous operation. 
    /// The Aff monad wraps the result, which is an Option&lt;TSource&gt; containing the only element of the sequence, or a None value if the sequence is empty.
    /// </returns>
    public static Aff<Option<TSource>> SingleOrNoneAff<TSource>(
        this IQueryable<TSource> source,
        CancellationToken ct = default)
    {
        return
            Aff(async () => await source.SingleOrNoneAsync(ct));
    }
    
    /// <summary>
    /// Asynchronously returns the only element of a sequence satisfying a specified condition,
    /// or a None value if no such element exists.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the sequence.</typeparam>
    /// <param name="source">The IQueryable&lt;TSource&gt; to get the single element from.</param>
    /// <param name="predicate">A lambda expression representing the condition to satisfy.</param>
    /// <param name="ct">The CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>
    /// A Task&lt;Option&lt;TSource&gt;&gt; that represents the asynchronous operation. 
    /// The task result contains the only element of the sequence satisfying the specified condition,
    /// or a None value if no such element exists.
    /// </returns>
    public static Task<Option<TSource>> SingleOrNoneAsync<TSource>(
        this IQueryable<TSource> source,
        Expression<Func<TSource, bool>> predicate,
        CancellationToken ct = default)
    {
        return
            source.SingleOrDefaultAsync(predicate, ct)
                  .Map(Optional);
    }
    
    /// <summary>
    /// Asynchronously returns the first element of a sequence as an Option&lt;T&gt; within a Task,
    /// or a None value if the sequence is empty.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    /// <param name="source">The IQueryable&lt;T&gt; to get the first element from.</param>
    /// <param name="ct">The CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>
    /// A Task&lt;Option&lt;T&gt;&gt; that represents the asynchronous operation. 
    /// The task result contains the first element of the sequence as an Option&lt;T&gt;,
    /// or a None value if the sequence is empty.
    /// </returns>
    public static Task<Option<T>> FirstOrNoneAsync<T>(
        this IQueryable<T> source, CancellationToken ct = default)
    {
        return source.FirstOrDefaultAsync(ct).Map(Optional);
    }
    
    /// <summary>
    /// Asynchronously returns the first element of a sequence as an Option&lt;T&gt; within an Aff monad,
    /// or a None value if the sequence is empty.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    /// <param name="source">The IQueryable&lt;T&gt; to get the first element from.</param>
    /// <param name="ct">The CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>
    /// An Aff&lt;Option&lt;T&gt;&gt; that represents the asynchronous operation. 
    /// The Aff monad wraps the result, which contains the first element of the sequence as an Option&lt;T&gt;,
    /// or a None value if the sequence is empty.
    /// </returns>
    public static Aff<Option<T>> FirstOrNoneAff<T>(
        this IQueryable<T> source, CancellationToken ct = default)
    {
        return 
            Aff(async () => await source.FirstOrNoneAsync(ct));
    }
    
    /// <summary>
    /// Asynchronously returns the first element of a sequence satisfying a specified condition as an Option&lt;T&gt; within a Task,
    /// or a None value if no such element exists.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    /// <param name="source">The IQueryable&lt;T&gt; to get the first element from.</param>
    /// <param name="predicate">A lambda expression representing the condition to satisfy.</param>
    /// <param name="ct">The CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>
    /// A Task&lt;Option&lt;T&gt;&gt; that represents the asynchronous operation. 
    /// The task result contains the first element of the sequence satisfying the specified condition as an Option&lt;T&gt;,
    /// or a None value if no such element exists.
    /// </returns>
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