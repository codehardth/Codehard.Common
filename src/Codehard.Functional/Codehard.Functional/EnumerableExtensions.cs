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
    public static Eff<Option<T>> SingleEff<T>(this IEnumerable<T> source)
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
            .Map(Optional)
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
    
    public static IEnumerable<T> WhereIfTrue<T>(
        this IEnumerable<T> source,
        bool condition,
        Func<T, bool> ifTrue,
        Func<T, bool>? ifFalse = default)
        => condition 
            ? source.Where(ifTrue) 
            : ifFalse != null ? source.Where(ifFalse) : source;
    
    public static IEnumerable<T1> WhereOptional<T1, T2>(
        this IEnumerable<T1> source,
        Option<T2> flagOpt,
        Func<T2, Func<T1, bool>> predicateConstructor)
        => flagOpt.Match(
            Some: val => source.Where(predicateConstructor(val)),
            None: source);
    
    public static IEnumerable<T> WhereOptional<T>(
        this IEnumerable<T> source,
        Option<Func<T, bool>> predicateOpt)
        => predicateOpt.Match(
            Some: source.Where,
            None: source);

    public static IEnumerable<T> SkipOptional<T>(
        this IEnumerable<T> source,
        Option<int> countOpt)
        => countOpt.Match(
            Some: source.Skip,
            None: source);
}