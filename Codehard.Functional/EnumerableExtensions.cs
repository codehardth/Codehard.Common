namespace Codehard.Functional;

public static class EnumerableExtensions
{
    /// <summary>
    /// Get the item at the first of the list or None if the list is empty.
    /// </summary>
    /// <param name="source"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Option<T> FirstOrNone<T>(this IEnumerable<T> source)
    {
        return source.HeadOrNone();
    }

    /// <summary>
    /// Splits the collection into two collections, containing the elements for which the given predicate returns true and false respectively.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="predicate"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static (IEnumerable<T> Matched, IEnumerable<T> Unmatched) Partition<T>(
        this IEnumerable<T> source,
        Func<T, bool> predicate)
    {
        if (source.IsNullOrEmpty())
        {
            return (Enumerable.Empty<T>(), Enumerable.Empty<T>());
        }

        var matched = source.Where(predicate);
        var unmatched = source.Except(matched);

        return (matched, unmatched);
    }

    /// <summary>
    /// Check if the collection is null or empty.
    /// </summary>
    /// <param name="source"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
    {
        return source == null || !source.Any();
    }
}