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
        return source.HeadOrNone();
    }

    /// <summary>
    /// Get the item at the first of the list or None if the list is empty.
    /// </summary>
    public static Option<T> FirstOrNone<T>(this IEnumerable<T> source, Func<T, bool> predicate)
    {
        return source
            .Map(Prelude.Optional)
            .FirstOrDefault(
                iOpt =>  iOpt.Filter(predicate).IsSome,
                Option<T>.None);
    }
    
    /// <summary>
    /// Returns the only element of a sequence,
    /// or a None value if the sequence is empty;
    /// this method return an ExceptionalError if there is more than one element in the sequence.
    /// </summary>
    public static Eff<Option<T>> SingleEff<T>(this IEnumerable<T> source)
    {
        return Eff(() => source
            .Map(Prelude.Optional)
            .SingleOrDefault(Option<T>.None));
    }
    
    /// <summary>
    /// Returns the only element of a sequence,
    /// or a None value if the sequence is empty;
    /// this method return an ExceptionalError if there is more than one element in the sequence.
    /// </summary>
    public static Fin<Option<T>> SingleFin<T>(this IEnumerable<T> source)
    {
        return SingleEff(source).Run();
    }
    
    /// <summary>
    /// Returns the only element of a sequence,
    /// or a None value if the sequence is empty;
    /// this method throw an ExceptionalError if there is more than one element in the sequence.
    /// </summary>
    public static Option<T> SingleOrThrow<T>(this IEnumerable<T> source)
    {
        return SingleFin(source).ThrowIfFail();
    }
    
    /// <summary>
    /// Returns the only element of a sequence,
    /// or a None value if the sequence is empty;
    /// or a None if there is more than one element in the sequence.
    /// </summary>
    public static Option<T> SingleOrNone<T>(this IEnumerable<T> source)
    {
        return SingleFin(source)
            .Match(
                Succ: i => i,
                Fail: _ => Option<T>.None);
    }

    /// <summary>
    /// Returns the only element of a sequence that satisfies a specified condition or a None value if no such element exists;
    /// this method return an ExceptionalError if more than one element satisfies the condition.
    /// </summary>
    public static Eff<Option<T>> SingleEff<T>(this IEnumerable<T> source, Func<T, bool> predicate)
    {
        return Eff(() => source
            .Map(Prelude.Optional)
            .SingleOrDefault(
                iOpt =>  iOpt.Filter(predicate).IsSome,
                Option<T>.None));
    }
    
    /// <summary>
    /// Returns the only element of a sequence that satisfies a specified condition or a None value if no such element exists;
    /// this method return an ExceptionalError if more than one element satisfies the condition.
    /// </summary>
    public static Fin<Option<T>> SingleFin<T>(this IEnumerable<T> source, Func<T, bool> predicate)
    {
        return SingleEff(source, predicate).Run();
    }
    
    /// <summary>
    /// Returns the only element of a sequence that satisfies a specified condition or a None value if no such element exists;
    /// this method throw exception if more than one element satisfies the condition.
    /// </summary>
    public static Option<T> SingleOrThrow<T>(this IEnumerable<T> source, Func<T, bool> predicate)
    {
        return SingleFin(source, predicate)
            .ThrowIfFail();
    }
    
    /// <summary>
    /// Returns the only element of a sequence that satisfies a specified condition or a None value if no such element exists;
    /// this method return a None if more than one element satisfies the condition.
    /// </summary>
    public static Option<T> SingleOrNone<T>(this IEnumerable<T> source, Func<T, bool> predicate)
    {
        return SingleFin(source, predicate)
            .Match(
                Succ: i => i,
                Fail: _ => Option<T>.None);
    }

    /// <summary>
    /// Splits the collection into two collections, containing the elements for which the given predicate returns true and false respectively.
    /// </summary>
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
        => flagOpt.Match(
            Some: val => source.Where(predicateConstructor(val)),
            None: source);
    
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
        => predicateOpt.Match(
            Some: source.Where,
            None: source);

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
        => countOpt.Match(
            Some: source.Skip,
            None: source);
}