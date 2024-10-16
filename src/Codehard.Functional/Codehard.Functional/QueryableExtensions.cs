using System.Linq.Expressions;
using LanguageExt;
using static LanguageExt.Prelude;

// ReSharper disable once CheckNamespace
namespace System.Linq;

/// <summary>
/// Extension methods for querying IQueryable collections with optional conditions.
/// </summary>
public static class QueryableExtensions
{
    /// <summary>
    /// Filters an IQueryable based on a condition, applying a predicate when the condition is true,
    /// and an optional alternative predicate when the condition is false.
    /// </summary>
    /// <typeparam name="T">The type of elements in the IQueryable.</typeparam>
    /// <param name="queryable">The source IQueryable collection.</param>
    /// <param name="condition">The condition to check.</param>
    /// <param name="ifTrue">The predicate to apply when the condition is true.</param>
    /// <param name="ifFalse">An optional predicate to apply when the condition is false. If not provided, the original collection is returned.</param>
    /// <returns>An IQueryable collection filtered based on the specified condition and predicates.</returns>
    public static IQueryable<T> WhereIf<T>(
        this IQueryable<T> queryable,
        bool condition,
        Expression<Func<T, bool>> ifTrue,
        Expression<Func<T, bool>>? ifFalse = default)
        => condition ? queryable.Where(ifTrue) : ifFalse != null ? queryable.Where(ifFalse) : queryable;
    
    /// <summary>
    /// Filters an IQueryable based on an optional value, applying a predicate constructed from the value when it is available.
    /// </summary>
    /// <typeparam name="T1">The type of elements in the IQueryable.</typeparam>
    /// <typeparam name="T2">The type of the optional value.</typeparam>
    /// <param name="queryable">The source IQueryable collection.</param>
    /// <param name="valOpt">An Option containing the optional value.</param>
    /// <param name="predicateConstructor">A function that constructs the predicate from the optional value.</param>
    /// <returns>An IQueryable collection filtered based on the optional value and predicate.</returns>
    public static IQueryable<T1> WhereOptional<T1, T2>(
        this IQueryable<T1> queryable,
        Option<T2> valOpt,
        Func<T2, Expression<Func<T1, bool>>> predicateConstructor)
        => valOpt.Match(
            Some: val => queryable.Where(predicateConstructor(val)),
            None: queryable);
    
    /// <summary>
    /// Filters an IQueryable based on an optional predicate.
    /// </summary>
    /// <typeparam name="T">The type of elements in the IQueryable.</typeparam>
    /// <param name="queryable">The source IQueryable collection.</param>
    /// <param name="predicateOpt">An Option containing the optional predicate.</param>
    /// <returns>An IQueryable collection filtered based on the optional predicate.</returns>
    public static IQueryable<T> WhereOptional<T>(
        this IQueryable<T> queryable,
        Option<Expression<Func<T, bool>>> predicateOpt)
        => predicateOpt.Match(
            Some: queryable.Where,
            None: queryable);

    /// <summary>
    /// Skips a specified number of elements from the beginning of the IQueryable based on an optional count.
    /// </summary>
    /// <typeparam name="T">The type of elements in the IQueryable.</typeparam>
    /// <param name="queryable">The source IQueryable collection.</param>
    /// <param name="countOpt">An Option containing the optional count of elements to skip.</param>
    /// <returns>An IQueryable collection with the specified number of elements skipped if the count is provided; otherwise, the original collection is returned.</returns>
    public static IQueryable<T> SkipOptional<T>(
        this IQueryable<T> queryable,
        Option<int> countOpt)
        => countOpt.Match(
            Some: queryable.Skip,
            None: queryable);
    
    /// <summary>
    /// Checks if all elements in the IQueryable satisfy a given predicate, returning None if the IQueryable is empty.
    /// </summary>
    /// <typeparam name="TSource">The type of elements in the IQueryable.</typeparam>
    /// <param name="source">The source IQueryable collection.</param>
    /// <param name="predicate">The predicate to check.</param>
    /// <returns>An Option containing true if all elements satisfy the predicate, or None if the IQueryable is empty.</returns>
    public static Option<bool> AllIfAny<TSource>(
        this IQueryable<TSource> source,
        Expression<Func<TSource, bool>> predicate)
    {
        return
            source.Any()
                ? Some(source.All(predicate))
                : None;
    }
}