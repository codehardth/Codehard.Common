using System.Linq.Expressions;

namespace Codehard.Functional;

public static class QueryableExtensions
{
    public static IQueryable<T> WhereIfTrue<T>(
        this IQueryable<T> queryable,
        bool condition,
        Expression<Func<T, bool>> ifTrue,
        Expression<Func<T, bool>>? ifFalse = default)
        => condition ? queryable.Where(ifTrue) : ifFalse != null ? queryable.Where(ifFalse) : queryable;
    
    public static IQueryable<T1> WhereOptional<T1, T2>(
        this IQueryable<T1> queryable,
        Option<T2> valOpt,
        Func<T2, Expression<Func<T1, bool>>> predicateConstructor)
        => valOpt.Match(
            Some: val => queryable.Where(predicateConstructor(val)),
            None: queryable);
    
    public static IQueryable<T> WhereOptional<T>(
        this IQueryable<T> queryable,
        Option<Expression<Func<T, bool>>> predicateOpt)
        => predicateOpt.Match(
            Some: queryable.Where,
            None: queryable);

    public static IQueryable<T> SkipOptional<T>(
        this IQueryable<T> queryable,
        Option<int> countOpt)
        => countOpt.Match(
            Some: queryable.Skip,
            None: queryable);
    
    /// <summary>
    /// If sequence has no element, returns None instead of true.
    /// </summary>
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