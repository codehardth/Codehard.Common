namespace Codehard.Common.Extensions;

/// <summary>
/// Provides extension methods for <see cref="IEnumerable{T}"/>.
/// </summary>
public static class EnumerableExtensions
{
    /// <summary>
    /// Performs a left outer join on two sequences based on the specified key selectors and result selector.
    /// </summary>
    /// <typeparam name="TLeft">The type of elements in the left sequence.</typeparam>
    /// <typeparam name="TRight">The type of elements in the right sequence.</typeparam>
    /// <typeparam name="TKey">The type of the keys returned by the key selector functions.</typeparam>
    /// <typeparam name="TResult">The type of the result elements.</typeparam>
    /// <param name="left">The left sequence to join.</param>
    /// <param name="right">The right sequence to join.</param>
    /// <param name="leftKeySelector">A function to extract the join key from each element of the left sequence.</param>
    /// <param name="rightKeySelector">A function to extract the join key from each element of the right sequence.</param>
    /// <param name="resultSelector">A function to create a result element from an element from the left sequence and an optional matching element from the right sequence.</param>
    /// <returns>An <see cref="IEnumerable{TResult}"/> that contains the result elements of the left outer join operation.</returns>
    public static IEnumerable<TResult> LeftJoin<TLeft, TRight, TKey, TResult>(
        this IEnumerable<TLeft> left,
        IEnumerable<TRight> right,
        Func<TLeft, TKey> leftKeySelector,
        Func<TRight, TKey> rightKeySelector,
        Func<TLeft, TRight?, TResult> resultSelector)
    {
        return
            from l in left
            join r in right
                on leftKeySelector(l) equals rightKeySelector(r)
                into a
            from nr in a.DefaultIfEmpty()
            select resultSelector(l, nr);
    }

    /// <summary>
    /// Performs a right outer join on two sequences based on the specified key selectors and result selector.
    /// </summary>
    /// <typeparam name="TLeft">The type of elements in the left sequence.</typeparam>
    /// <typeparam name="TRight">The type of elements in the right sequence.</typeparam>
    /// <typeparam name="TKey">The type of the keys returned by the key selector functions.</typeparam>
    /// <typeparam name="TResult">The type of the result elements.</typeparam>
    /// <param name="left">The left sequence to join.</param>
    /// <param name="right">The right sequence to join.</param>
    /// <param name="leftKeySelector">A function to extract the join key from each element of the left sequence.</param>
    /// <param name="rightKeySelector">A function to extract the join key from each element of the right sequence.</param>
    /// <param name="resultSelector">A function to create a result element from an optional matching element from the left sequence and an element from the right sequence.</param>
    /// <returns>An <see cref="IEnumerable{TResult}"/> that contains the result elements of the right outer join operation.</returns>
    public static IEnumerable<TResult> RightJoin<TLeft, TRight, TKey, TResult>(
        this IEnumerable<TLeft> left,
        IEnumerable<TRight> right,
        Func<TLeft, TKey> leftKeySelector,
        Func<TRight, TKey> rightKeySelector,
        Func<TLeft?, TRight, TResult> resultSelector)
    {
        return
            from r in right
            join l in left
                on rightKeySelector(r) equals leftKeySelector(l)
                into a
            from nl in a.DefaultIfEmpty()
            select resultSelector(nl, r);
    }

    /// <summary>
    /// Cast a source into an array if it was already an array. Otherwise allocate it using ToArray.
    /// </summary>
    /// <param name="source"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T[] AsArray<T>(this IEnumerable<T> source) =>
        source as T[] ?? source.ToArray();

    /// <summary>
    /// Cast a source into a list if it was already a list. Otherwise allocate it using ToList.
    /// </summary>
    /// <param name="source"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static List<T> AsList<T>(this IEnumerable<T> source) =>
        source as List<T> ?? source.ToList();
    
    /// <summary>
    /// Filters the collection based on a condition when a specified condition is true,
    /// and an optional alternative condition when the specified condition is false.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="source">The source IEnumerable collection.</param>
    /// <param name="condition">The condition to check.</param>
    /// <param name="ifTrue">The condition to apply when the main condition is true.</param>
    /// <param name="ifFalse">An optional condition to apply when the main condition is false. If not provided, the original collection is returned.</param>
    /// <returns>The filtered collection based on the specified conditions.</returns>
    public static IEnumerable<T> WhereIf<T>(
        this IEnumerable<T> source,
        bool condition,
        Func<T, bool> ifTrue,
        Func<T, bool>? ifFalse = default)
        => condition 
            ? source.Where(ifTrue) 
            : ifFalse != null ? source.Where(ifFalse) : source;

    /// <summary>
    /// Filters the source collection, selecting only those elements that are not null.
    /// </summary>
    /// <typeparam name="TSource">The type of elements in the source collection.</typeparam>
    /// <typeparam name="TResult">The type of the result elements, which must be a reference type.</typeparam>
    /// <param name="source">The source collection to filter.</param>
    /// <param name="selector">A function to transform each element of the source collection into a TResult.</param>
    /// <returns>A collection of TResult elements that are not null.</returns>
    public static IEnumerable<TResult> SelectOnlyNotNull<TSource, TResult>(
        IEnumerable<TSource> source,
        Func<TSource, TResult?> selector)
        where TResult : class
        => source.Select(selector)
                 .Where(res => res != null)!;
    
    /// <summary>
    /// Filters the source collection, selecting only those elements that are not null.
    /// </summary>
    /// <typeparam name="TSource">The type of elements in the source collection.</typeparam>
    /// <typeparam name="TResult">The type of the result elements, which must be a value type.</typeparam>
    /// <param name="source">The source collection to filter.</param>
    /// <param name="selector">A function to transform each element of the source collection into a TResult.</param>
    /// <returns>A collection of TResult elements that are not null.</returns>
    public static IEnumerable<TResult> SelectOnlyNotNull<TSource, TResult>(
        IEnumerable<TSource> source,
        Func<TSource, TResult?> selector)
        where TResult : struct
        => source.Select(selector)
                 .Where(res => res != null)
                 .Select(res => res!.Value);
}