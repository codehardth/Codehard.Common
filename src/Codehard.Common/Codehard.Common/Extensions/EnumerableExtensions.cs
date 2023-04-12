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
}