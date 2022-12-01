using LanguageExt.SomeHelp;

namespace Codehard.Functional;

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
            .Map(Optional)
            .FirstOrDefault(
                iOpt =>  iOpt.Filter(predicate).IsSome,
                Option<T>.None);
    }
    
    /// <summary>
    /// Returns the only element of a sequence,
    /// or a None value if the sequence is empty;
    /// this method return an ExceptionalError if there is more than one element in the sequence.
    /// </summary>
    public static Eff<Option<T>> SingleOrNoneOrFailEff<T>(this IEnumerable<T> source)
    {
        return Eff(() => source
            .Map(Optional)
            .SingleOrDefault(Option<T>.None));
    }
    
    /// <summary>
    /// Returns the only element of a sequence,
    /// or a None value if the sequence is empty;
    /// this method return an ExceptionalError if there is more than one element in the sequence.
    /// </summary>
    public static Fin<Option<T>> SingleOrNoneOrFailFin<T>(this IEnumerable<T> source)
    {
        return SingleOrNoneOrFailEff(source).Run();
    }
    
    /// <summary>
    /// Returns the only element of a sequence,
    /// or a None value if the sequence is empty;
    /// or a None if there is more than one element in the sequence.
    /// </summary>
    public static Option<T> SingleOrNone<T>(this IEnumerable<T> source)
    {
        return SingleOrNoneOrFailFin(source)
            .Match(
                Succ: i => i,
                Fail: _ => Option<T>.None);
    }

    /// <summary>
    /// Returns the only element of a sequence that satisfies a specified condition or a None value if no such element exists;
    /// this method return an ExceptionalError if more than one element satisfies the condition.
    /// </summary>
    public static Eff<Option<T>> SingleOrNoneOrFailEff<T>(this IEnumerable<T> source, Func<T, bool> predicate)
    {
        return Eff(() => source
            .Map(Optional)
            .SingleOrDefault(
                iOpt =>  iOpt.Filter(predicate).IsSome,
                Option<T>.None));
    }
    
    /// <summary>
    /// Returns the only element of a sequence that satisfies a specified condition or a None value if no such element exists;
    /// this method return an ExceptionalError if more than one element satisfies the condition.
    /// </summary>
    public static Fin<Option<T>> SingleOrNoneOrFailFin<T>(this IEnumerable<T> source, Func<T, bool> predicate)
    {
        return SingleOrNoneOrFailEff(source, predicate).Run();
    }
    
    /// <summary>
    /// Returns the only element of a sequence that satisfies a specified condition or a None value if no such element exists;
    /// this method return a None if more than one element satisfies the condition.
    /// </summary>
    public static Option<T> SingleOrNone<T>(this IEnumerable<T> source, Func<T, bool> predicate)
    {
        return SingleOrNoneOrFailFin(source, predicate)
            .Match(
                Succ: i => i,
                Fail: _ => Option<T>.None);;
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
}