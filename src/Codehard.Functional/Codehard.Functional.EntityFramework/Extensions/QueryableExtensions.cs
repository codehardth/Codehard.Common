using System.Linq.Expressions;
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
    /// Asynchronously converts a sequence to a list within an Eff monad.
    /// </summary>
    /// <typeparam name="T">The type of the elements of the source sequence.</typeparam>
    /// <param name="source">An <see cref="IQueryable{T}"/> to create a list from.</param>
    /// <returns>
    /// An Eff monad that represents the asynchronous operation. The Eff monad wraps a <see cref="System.Collections.Generic.List{T}"/> that contains elements from the input sequence.
    /// </returns>
    public static Eff<List<T>> ToListEff<T>(this IQueryable<T> source)
    {
        return liftIO(env => source.ToListAsync(env.Token));
    }
    
    /// <summary>
    /// Asynchronously converts a sequence to an array within an Eff monad.
    /// </summary>
    /// <typeparam name="T">The type of the elements of the source sequence.</typeparam>
    /// <param name="source">An <see cref="IQueryable{T}"/> to create an array from.</param>
    /// <returns>
    /// An Eff monad that represents the asynchronous operation. The Eff monad wraps an array that contains elements from the input sequence.
    /// </returns>
    public static Eff<T[]> ToArrayEff<T>(this IQueryable<T> source)
    {
        return liftIO(env => source.ToArrayAsync(env.Token));
    }

    /// <summary>
    /// Asynchronously determines whether a sequence contains any elements within an Eff monad.
    /// </summary>
    /// <typeparam name="T">The type of the elements of the source sequence.</typeparam>
    /// <param name="source">An <see cref="IQueryable{T}"/> to check for emptiness.</param>
    /// <returns>
    /// An Eff monad that represents the asynchronous operation. The Eff monad wraps a boolean value that is true if the source sequence contains any elements; otherwise, false.
    /// </returns>
    public static Eff<bool> AnyEff<T>(this IQueryable<T> source)
    {
        return liftIO(env => source.AnyAsync(env.Token));
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
        this IQueryable<TSource> source, CancellationToken ct = default)
    {
        return
            source.SingleOrDefaultAsync(ct)
                  .Map(Optional);
    }

    /// <summary>
    /// Asynchronously returns the only element of a sequence as an Option&lt;TSource&gt; within an Eff monad,
    /// or a None value if the sequence is empty.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the sequence.</typeparam>
    /// <param name="source">The IQueryable&lt;TSource&gt; to get the single element from.</param>
    /// <returns>
    /// An Eff&lt;Option&lt;TSource&gt;&gt; that represents the asynchronous operation. 
    /// The Eff monad wraps the result, which is an Option&lt;TSource&gt; containing the only element of the sequence, or a None value if the sequence is empty.
    /// </returns>
    public static Eff<Option<TSource>> SingleOrNoneEff<TSource>(
        this IQueryable<TSource> source)
    {
        return liftIO(env => source.SingleOrNoneAsync(env.Token));
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
    /// Asynchronously returns the only element of a sequence satisfying a specified condition as an Option&lt;TSource&gt; within an Eff monad,
    /// or a None value if no such element exists.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the sequence.</typeparam>
    /// <param name="source">The IQueryable&lt;TSource&gt; to get the single element from.</param>
    /// <param name="predicate">A lambda expression representing the condition to satisfy.</param>
    /// <returns>
    /// An Eff&lt;Option&lt;TSource&gt;&gt; that represents the asynchronous operation.
    /// The Eff monad wraps the result, which is an Option&lt;TSource&gt; containing the only element of the sequence satisfying the specified condition,
    /// or a None value if no such element exists.
    /// </returns>
    public static Eff<Option<TSource>> SingleOrNoneEff<TSource>(
        this IQueryable<TSource> source,
        Expression<Func<TSource, bool>> predicate)
    {
        return
            liftIO(async env =>
                await source.SingleOrNoneAsync(predicate, env.Token));
    }
    
    /// <summary>
    /// Asynchronously returns the only element of a sequence satisfying a specified condition within an Eff monad.
    /// This method will returns fail if no such element exists or if more than one element satisfies the condition.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the sequence.</typeparam>
    /// <param name="source">The IQueryable&lt;TSource&gt; to get the single element from.</param>
    /// <param name="predicate">A lambda expression representing the condition to satisfy.</param>
    /// <returns>
    /// An Eff&lt;TSource&gt; that represents the asynchronous operation.
    /// The Eff monad wraps the result, which is the only element of the sequence satisfying the specified condition.
    /// </returns>
    public static Eff<TSource> SingleOrFailEff<TSource>(
        this IQueryable<TSource> source,
        Expression<Func<TSource, bool>> predicate)
    {
        return
            liftIO(async env =>
                await source.SingleAsync(predicate, env.Token));
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
    /// Asynchronously returns the first element of a sequence as an Option&lt;T&gt; within an Eff monad,
    /// or a None value if the sequence is empty.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    /// <param name="source">The IQueryable&lt;T&gt; to get the first element from.</param>
    /// <returns>
    /// An Eff&lt;Option&lt;T&gt;&gt; that represents the asynchronous operation. 
    /// The Eff monad wraps the result, which contains the first element of the sequence as an Option&lt;T&gt;,
    /// or a None value if the sequence is empty.
    /// </returns>
    public static Eff<Option<T>> FirstOrNoneEff<T>(this IQueryable<T> source)
    {
        return liftIO(env => source.FirstOrNoneAsync(env.Token));
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
            .FirstOrDefaultAsync(predicate, ct)
            .Map(Optional);
    }
    
    /// <summary>
    /// Asynchronously returns the first element of a sequence satisfying a specified condition as an Option&lt;T&gt; within an Eff monad,
    /// or a None value if no such element exists.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    /// <param name="source">The IQueryable&lt;T&gt; to get the first element from.</param>
    /// <param name="predicate">A lambda expression representing the condition to satisfy.</param>
    /// <returns>
    /// An Eff&lt;Option&lt;T&gt;&gt; that represents the asynchronous operation.
    /// The Eff monad wraps the result, which contains the first element of the sequence satisfying the specified condition as an Option&lt;T&gt;,
    /// or a None value if no such element exists.
    /// </returns>
    public static Eff<Option<T>> FirstOrNoneEff<T>(
        this IQueryable<T> source,
        Expression<Func<T, bool>> predicate)
    {
        return liftIO(env => source.FirstOrNoneAsync(predicate, env.Token));
    }
    
    /// <summary>
    /// Asynchronously counts the number of elements in a sequence within an Eff monad.
    /// </summary>
    /// <typeparam name="T">The type of the elements of the source sequence.</typeparam>
    /// <param name="source">An <see cref="IQueryable{T}"/> to count the elements of.</param>
    /// <returns>
    /// An Eff monad that represents the asynchronous operation. The Eff monad wraps an integer that represents the number of elements in the input sequence.
    /// </returns>
    public static Eff<int> CountEff<T>(this IQueryable<T> source)
    {
        return liftIO(env => source.CountAsync(env.Token));
    }
    
    /// <summary>
    /// Asynchronously counts the number of elements in a sequence that satisfy a specified condition within an Eff monad.
    /// </summary>
    /// <typeparam name="T">The type of the elements of the source sequence.</typeparam>
    /// <param name="source">An <see cref="IQueryable{T}"/> to count the elements of.</param>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <returns>
    /// An Eff monad that represents the asynchronous operation. The Eff monad wraps an integer that represents the number of elements in the input sequence that satisfy the condition.
    /// </returns>
    public static Eff<int> CountEff<T>(
        this IQueryable<T> source,
        Expression<Func<T, bool>> predicate)
    {
        return liftIO(env => source.CountAsync(predicate, env.Token));
    }
    
    /// <summary>
    /// Asynchronously counts the number of elements in a sequence within an Eff monad.
    /// </summary>
    /// <typeparam name="T">The type of the elements of the source sequence.</typeparam>
    /// <param name="source">An <see cref="IQueryable{T}"/> to count the elements of.</param>
    /// <returns>
    /// An Eff monad that represents the asynchronous operation. The Eff monad wraps a long integer that represents the number of elements in the input sequence.
    /// </returns>
    public static Eff<long> LongCountEff<T>(
        this IQueryable<T> source)
    {
        return liftIO(env => source.LongCountAsync(env.Token));
    }
    
    /// <summary>
    /// Asynchronously counts the number of elements in a sequence that satisfy a specified condition within an Eff monad.
    /// </summary>
    /// <typeparam name="T">The type of the elements of the source sequence.</typeparam>
    /// <param name="source">An <see cref="IQueryable{T}"/> to count the elements of.</param>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <returns>
    /// An Eff monad that represents the asynchronous operation. The Eff monad wraps a long integer that represents the number of elements in the input sequence that satisfy the condition.
    /// </returns>
    public static Eff<long> LongCountEff<T>(
        this IQueryable<T> source,
        Expression<Func<T, bool>> predicate)
    {
        return liftIO(env => source.LongCountAsync(predicate, env.Token));
    }
}