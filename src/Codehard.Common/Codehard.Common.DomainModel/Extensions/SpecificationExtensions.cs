namespace Codehard.Common.DomainModel.Extensions;

public static class SpecificationExtensions
{
    /// <summary>
    /// Apply a specification to <see cref="IEnumerable{T}"/>.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="specification"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IEnumerable<T> Apply<T>(this IEnumerable<T> source, ISpecification<T> specification)
    {
        return source.Where(specification.IsSatisfiedBy);
    }

    /// <summary>
    /// Apply a specification to <see cref="IQueryable{T}"/>.
    /// </summary>
    /// <param name="query"></param>
    /// <param name="specification"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IQueryable<T> Apply<T>(this IQueryable<T> query, IExpressionSpecification<T> specification)
    {
        return query.Where(specification.Expression);
    }
}