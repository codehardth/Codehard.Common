using System.Linq.Expressions;

namespace Codehard.Common.DomainModel;

/// <summary>
/// An implementation for expression based specification.
/// </summary>
/// <typeparam name="T"></typeparam>1
public abstract class ExpressionSpecification<T> : IExpressionSpecification<T>
{
    /// <inheritdoc />
    public Expression<Func<T, bool>> Expression { get; }

    private Func<T, bool>? expressionFunc;
    private Func<T, bool> ExpressionFunc => expressionFunc ??= Expression.Compile();

    /// <summary>
    /// </summary>
    /// <param name="expression"></param>
    protected ExpressionSpecification(Expression<Func<T, bool>> expression)
    {
        this.Expression = expression;
    }

    /// <inheritdoc />
    public bool IsSatisfiedBy(T obj)
        => this.ExpressionFunc(obj);
}