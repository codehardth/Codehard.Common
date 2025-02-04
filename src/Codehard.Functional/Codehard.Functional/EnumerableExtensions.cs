// ReSharper disable once CheckNamespace

namespace System.Linq;

/// <summary>
/// Extension methods for working with IEnumerable collections.
/// </summary>
public static class EnumerableExtensions
{
    /// <summary>
    /// Get the item at the first of the list or None if the list is empty.
    /// </summary>
    public static Option<T> FirstOrNone<T>(this IEnumerable<T> source)
    {
        return source.FirstOrDefault();
    }

    /// <summary>
    /// Get the item at the first of the list or None if the list is empty.
    /// </summary>
    public static Option<T> FirstOrNone<T>(this IEnumerable<T> source, Func<T, bool> predicate)
    {
        return
            source
                .Select(Prelude.Optional)
                .FirstOrDefault(
                    iOpt => iOpt.Filter(predicate).IsSome,
                    Option<T>.None);
    }
    
    /// <summary>
    /// Returns the first element of a sequence,
    /// or returns an ExceptionalError if the sequence contains no elements.
    /// </summary>
    /// <typeparam name="T">The type of the elements of the source sequence.</typeparam>
    /// <param name="source">The sequence to return the first element of.</param>
    /// <returns>An Eff monad that represents the operation of retrieving the first element from the sequence.</returns>
    public static Eff<T> FirstEff<T>(this IEnumerable<T> source)
    {
        return liftEff(source.First);
    }
    
    /// <summary>
    /// Returns the first element of a sequence that satisfies a specified condition,
    /// or throws an exception if no such element is found.
    /// </summary>
    /// <typeparam name="T">The type of the elements of the source sequence.</typeparam>
    /// <param name="source">The sequence to return the first element of.</param>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <returns>An Eff monad that represents the operation of retrieving the first element from the sequence that satisfies the condition.</returns>
    public static Eff<T> FirstEff<T>(this IEnumerable<T> source, Func<T, bool> predicate)
    {
        return liftEff(() => source.First(predicate));
    }

    /// <summary>
    /// Returns the only element of a sequence;
    /// or returns an ExceptionalError if there is not exactly one element in the sequence.
    /// </summary>
    /// <typeparam name="T">The type of the elements of the source sequence.</typeparam>
    /// <param name="source">The sequence to return the single element of.</param>
    /// <returns>An Eff monad that represents the operation of retrieving the single element from the sequence.</returns>
    public static Eff<T> SingleEff<T>(this IEnumerable<T> source)
    {
        return liftEff(source.Single);
    }
    
    /// <summary>
    /// Returns the only element of a sequence, or a None value if the sequence is empty;
    /// this method returns an ExceptionalError if there is more than one element in the sequence.
    /// </summary>
    /// <typeparam name="T">The type of the elements of the source sequence.</typeparam>
    /// <param name="source">The sequence to return the single element of.</param>
    /// <returns>An Eff monad that represents the operation of retrieving the single element from the sequence, or None if the sequence is empty.</returns>
    public static Eff<Option<T>> SingleOrNoneEff<T>(this IEnumerable<T> source)
    {
        return
            liftEff(() =>
                source
                    .Select(Prelude.Optional)
                    .SingleOrDefault(Option<T>.None));
    }
    
    /// <summary>
    /// Returns the only element of a sequence that satisfies a specified condition,
    /// </summary>
    public static Eff<T> SingleEff<T>(this IEnumerable<T> source, Func<T, bool> predicate)
    {
        return liftEff(() => source.Single(predicate));
    }

    /// <summary>
    /// Returns the only element of a sequence that satisfies a specified condition or a None value if no such element exists;
    /// this method return an ExceptionalError if more than one element satisfies the condition.
    /// </summary>
    public static Eff<Option<T>> SingleOrNoneEff<T>(this IEnumerable<T> source, Func<T, bool> predicate)
    {
        return
            liftEff(() =>
                source
                    .Select(Prelude.Optional)
                    .SingleOrDefault(
                        iOpt => iOpt.Filter(predicate).IsSome,
                        Option<T>.None));
    }

    /// <summary>
    /// Splits the collection into two collections, containing the elements for which the given predicate returns true and false respectively.
    /// </summary>
    public static (IEnumerable<T> Matched, IEnumerable<T> Unmatched) Partition<T>(
        this IEnumerable<T> source,
        Func<T, bool> predicate)
    {
        if (source.IsNullOrEmpty()) return (Enumerable.Empty<T>(), Enumerable.Empty<T>());

        var matched = source.Where(predicate);
        var unmatched = source.Except(matched);

        return (matched, unmatched);
    }

    /// <summary>
    /// Check if the collection is null or empty.
    /// </summary>
    public static bool IsNullOrEmpty<T>(this IEnumerable<T>? source)
    {
        return source == null || !source.Any();
    }

    /// <summary>
    /// If sequence has no element, returns None instead of true.
    /// </summary>
    public static Option<bool> AllIfAny<TSource>(
        this IEnumerable<TSource> source,
        Func<TSource, bool> predicate)
    {
        return
            source.Any()
                ? Some(source.All(predicate))
                : None;
    }

    /// <summary>
    /// Filters the collection based on an optional flag and a function that generates a predicate based on the flag.
    /// </summary>
    /// <typeparam name="T1">The type of elements in the collection.</typeparam>
    /// <typeparam name="T2">The type of the optional flag.</typeparam>
    /// <param name="source">The source IEnumerable collection.</param>
    /// <param name="flagOpt">An Option containing the optional flag.</param>
    /// <param name="predicateConstructor">A function that generates a predicate based on the flag.</param>
    /// <returns>The filtered collection based on the flag and the generated predicate.</returns>
    public static IEnumerable<T1> WhereOptional<T1, T2>(
        this IEnumerable<T1> source,
        Option<T2> flagOpt,
        Func<T2, Func<T1, bool>> predicateConstructor)
    {
        return flagOpt.Match(
            val => source.Where(predicateConstructor(val)),
            source);
    }

    /// <summary>
    /// Filters the collection based on an optional predicate.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="source">The source IEnumerable collection.</param>
    /// <param name="predicateOpt">An Option containing the optional predicate.</param>
    /// <returns>The filtered collection based on the optional predicate.</returns>
    public static IEnumerable<T> WhereOptional<T>(
        this IEnumerable<T> source,
        Option<Func<T, bool>> predicateOpt)
    {
        return predicateOpt.Match(
            source.Where,
            source);
    }

    /// <summary>
    /// Skips a specified number of elements from the beginning of the collection based on an optional count.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="source">The source IEnumerable collection.</param>
    /// <param name="countOpt">An Option containing the optional count of elements to skip.</param>
    /// <returns>The collection with the specified number of elements skipped if the count is provided; otherwise, the original collection is returned.</returns>
    public static IEnumerable<T> SkipOptional<T>(
        this IEnumerable<T> source,
        Option<int> countOpt)
    {
        return countOpt.Match(
            source.Skip,
            source);
    }
}