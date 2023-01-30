namespace Codehard.Common.Extensions;

public static class EnumerableExtensions
{
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