using System.Linq.Expressions;

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

    /// <summary>
    /// Logically AndAlso the <paramref name="spec1"/> and <paramref name="spec2"/>.
    /// </summary>
    /// <param name="spec1">The first specification.</param>
    /// <param name="spec2">The second specification.</param>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <returns>A new specification that represents the logical AndAlso of the two specifications.</returns>
    public static IExpressionSpecification<T> And<T>(
        this IExpressionSpecification<T> spec1,
        IExpressionSpecification<T> spec2)
    {
        if (spec1 == spec2)
        {
            return spec1;
        }

        var expr1 = spec1.Expression;
        var expr2 = spec2.Expression;

        var param = expr1.Parameters[0];

        var newExpression =
            ReferenceEquals(param, expr2.Parameters[0])
                ? Expression.Lambda<Func<T, bool>>(
                    Expression.AndAlso(expr1.Body, expr2.Body), param)
                : Expression.Lambda<Func<T, bool>>(
                    Expression.AndAlso(
                        expr1.Body,
                        Expression.Invoke(expr2, param)), param);

        return new ExpressionSpecification<T>(newExpression);
    }

    /// <summary>
    /// Logically OrElse the <paramref name="spec1"/> and <paramref name="spec2"/>.
    /// </summary>
    /// <param name="spec1"></param>
    /// <param name="spec2"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IExpressionSpecification<T> Or<T>(
        this IExpressionSpecification<T> spec1,
        IExpressionSpecification<T> spec2)
    {
        if (spec1 == spec2)
        {
            return spec1;
        }

        var expr1 = spec1.Expression;
        var expr2 = spec2.Expression;

        var param = expr1.Parameters[0];

        var newExpression =
            ReferenceEquals(param, expr2.Parameters[0])
                ? Expression.Lambda<Func<T, bool>>(
                    Expression.OrElse(expr1.Body, expr2.Body), param)
                : Expression.Lambda<Func<T, bool>>(
                    Expression.OrElse(
                        expr1.Body,
                        Expression.Invoke(expr2, param)), param);

        return new ExpressionSpecification<T>(newExpression);
    }

    /// <summary>
    /// Logically Not the <paramref name="spec"/>.
    /// </summary>
    /// <param name="spec"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IExpressionSpecification<T> Not<T>(
        this IExpressionSpecification<T> spec)
    {
        var expr = spec.Expression;

        return new ExpressionSpecification<T>(
            Expression.Lambda<Func<T, bool>>(
                Expression.Not(expr.Body), expr.Parameters[0]));
    }
}