using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using LanguageExt;

using static LanguageExt.Prelude;

// ReSharper disable once CheckNamespace
namespace Microsoft.EntityFrameworkCore;

/// <summary>
/// Provides a set of static methods for querying data structures 
/// that implement <see cref="IQueryable"/> using LanguageExt and asynchronous operations.
/// </summary>
public static class QueryableExtensions
{
    /// <summary>
    /// Asynchronously converts a sequence to a list within an Aff monad.
    /// </summary>
    /// <typeparam name="T">The type of the elements of the source sequence.</typeparam>
    /// <param name="source">An <see cref="IQueryable{T}"/> to create a list from.</param>
    /// <param name="ct">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>
    /// An Aff monad that represents the asynchronous operation. The Aff monad wraps a <see cref="List{T}"/> that contains elements from the input sequence.
    /// </returns>
    public static Aff<List<T>> ToListAff<T>(
        this IQueryable<T> source, CancellationToken ct = default)
    {
        return Aff(async () => await source.ToListAsync(ct));
    }
    
    /// <summary>
    /// Asynchronously converts a sequence to an array within an Aff monad.
    /// </summary>
    /// <typeparam name="T">The type of the elements of the source sequence.</typeparam>
    /// <param name="source">An <see cref="IQueryable{T}"/> to create an array from.</param>
    /// <param name="ct">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>
    /// An Aff monad that represents the asynchronous operation. The Aff monad wraps an array that contains elements from the input sequence.
    /// </returns>
    public static Aff<T[]> ToArrayAff<T>(
        this IQueryable<T> source, CancellationToken ct = default)
    {
        return Aff(async () => await source.ToArrayAsync(ct));
    }

    /// <summary>
    /// Asynchronously determines whether a sequence contains any elements within an Aff monad.
    /// </summary>
    /// <typeparam name="T">The type of the elements of the source sequence.</typeparam>
    /// <param name="source">An <see cref="IQueryable{T}"/> to check for emptiness.</param>
    /// <param name="ct">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>
    /// An Aff monad that represents the asynchronous operation. The Aff monad wraps a boolean value that is true if the source sequence contains any elements; otherwise, false.
    /// </returns>
    public static Aff<bool> AnyAff<T>(
        this IQueryable<T> source, CancellationToken ct = default)
    {
        return Aff(async () => await source.AnyAsync(ct));
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
    /// Asynchronously returns the only element of a sequence satisfying a specified condition as an Option&lt;TSource&gt; within an Aff monad,
    /// or a None value if no such element exists.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the sequence.</typeparam>
    /// <param name="source">The IQueryable&lt;TSource&gt; to get the single element from.</param>
    /// <param name="predicate">A lambda expression representing the condition to satisfy.</param>
    /// <param name="ct">The CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>
    /// An Aff&lt;Option&lt;TSource&gt;&gt; that represents the asynchronous operation.
    /// The Aff monad wraps the result, which is an Option&lt;TSource&gt; containing the only element of the sequence satisfying the specified condition,
    /// or a None value if no such element exists.
    /// </returns>
    public static Aff<Option<TSource>> SingleOrNoneAff<TSource>(
        this IQueryable<TSource> source,
        Expression<Func<TSource, bool>> predicate,
        CancellationToken ct = default)
    {
        return
            Aff(async () =>
                await source.SingleOrNoneAsync(predicate, ct));
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
    
    /// <summary>
    /// Asynchronously returns the first element of a sequence satisfying a specified condition as an Option&lt;T&gt; within an Aff monad,
    /// or a None value if no such element exists.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    /// <param name="source">The IQueryable&lt;T&gt; to get the first element from.</param>
    /// <param name="predicate">A lambda expression representing the condition to satisfy.</param>
    /// <param name="ct">The CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>
    /// An Aff&lt;Option&lt;T&gt;&gt; that represents the asynchronous operation.
    /// The Aff monad wraps the result, which contains the first element of the sequence satisfying the specified condition as an Option&lt;T&gt;,
    /// or a None value if no such element exists.
    /// </returns>
    public static Aff<Option<T>> FirstOrNoneAff<T>(
        this IQueryable<T> source,
        Expression<Func<T, bool>> predicate,
        CancellationToken ct = default)
    {
        return 
            Aff(async () =>
                await source.FirstOrNoneAsync(predicate, ct));
    }
    
    /// <summary>
    /// Asynchronously counts the number of elements in a sequence within an Aff monad.
    /// </summary>
    /// <typeparam name="T">The type of the elements of the source sequence.</typeparam>
    /// <param name="source">An <see cref="IQueryable{T}"/> to count the elements of.</param>
    /// <param name="ct">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>
    /// An Aff monad that represents the asynchronous operation. The Aff monad wraps an integer that represents the number of elements in the input sequence.
    /// </returns>
    public static Aff<int> CountAff<T>(
        this IQueryable<T> source,
        CancellationToken ct = default)
    {
        return Aff(async () => await source.CountAsync(ct));
    }
    
    /// <summary>
    /// Asynchronously counts the number of elements in a sequence that satisfy a specified condition within an Aff monad.
    /// </summary>
    /// <typeparam name="T">The type of the elements of the source sequence.</typeparam>
    /// <param name="source">An <see cref="IQueryable{T}"/> to count the elements of.</param>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <param name="ct">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>
    /// An Aff monad that represents the asynchronous operation. The Aff monad wraps an integer that represents the number of elements in the input sequence that satisfy the condition.
    /// </returns>
    public static Aff<int> CountAff<T>(
        this IQueryable<T> source,
        Expression<Func<T, bool>> predicate,
        CancellationToken ct = default)
    {
        return Aff(async () => await source.CountAsync(predicate, ct));
    }
    
    /// <summary>
    /// Asynchronously counts the number of elements in a sequence within an Aff monad.
    /// </summary>
    /// <typeparam name="T">The type of the elements of the source sequence.</typeparam>
    /// <param name="source">An <see cref="IQueryable{T}"/> to count the elements of.</param>
    /// <param name="ct">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>
    /// An Aff monad that represents the asynchronous operation. The Aff monad wraps a long integer that represents the number of elements in the input sequence.
    /// </returns>
    public static Aff<long> LongCountAff<T>(
        this IQueryable<T> source,
        CancellationToken ct = default)
    {
        return Aff(async () => await source.LongCountAsync(ct));
    }
    
    /// <summary>
    /// Asynchronously counts the number of elements in a sequence that satisfy a specified condition within an Aff monad.
    /// </summary>
    /// <typeparam name="T">The type of the elements of the source sequence.</typeparam>
    /// <param name="source">An <see cref="IQueryable{T}"/> to count the elements of.</param>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <param name="ct">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>
    /// An Aff monad that represents the asynchronous operation. The Aff monad wraps a long integer that represents the number of elements in the input sequence that satisfy the condition.
    /// </returns>
    public static Aff<long> LongCountAff<T>(
        this IQueryable<T> source,
        Expression<Func<T, bool>> predicate,
        CancellationToken ct = default)
    {
        return Aff(async () => await source.LongCountAsync(predicate, ct));
    }
}