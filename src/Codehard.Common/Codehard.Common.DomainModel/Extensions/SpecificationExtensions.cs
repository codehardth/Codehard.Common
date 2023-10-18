namespace Codehard.Common.DomainModel.Extensions;

/// <summary>
/// Provides extension methods for working with specifications.
/// </summary>
public static class SpecificationExtensions
{
    /// <summary>
    /// Apply a specification to an <see cref="IEnumerable{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="source">The source collection to apply the specification to.</param>
    /// <param name="specification">The specification to apply.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> containing elements that satisfy the specification.</returns>
    public static IEnumerable<T> Apply<T>(this IEnumerable<T> source, ISpecification<T> specification)
    {
        return source.Where(specification.IsSatisfiedBy);
    }

    /// <summary>
    /// Apply a specification to an <see cref="IQueryable{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of elements in the queryable collection.</typeparam>
    /// <param name="query">The queryable collection to apply the specification to.</param>
    /// <param name="specification">The expression-based specification to apply.</param>
    /// <returns>An <see cref="IQueryable{T}"/> containing elements that satisfy the specification.</returns>
    public static IQueryable<T> Apply<T>(this IQueryable<T> query, IExpressionSpecification<T> specification)
    {
        return query.Where(specification.Expression);
    }
}