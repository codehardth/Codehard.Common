using System.Linq.Expressions;

namespace Codehard.Common;

internal class ReplaceExpressionVisitor
    : ExpressionVisitor
{
    private readonly Expression oldValue;
    private readonly Expression newValue;

    public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
    {
        this.oldValue = oldValue;
        this.newValue = newValue;
    }

    public override Expression? Visit(Expression? node)
    {
        return node == this.oldValue ? this.newValue : base.Visit(node);
    }
}

public static class ExpressionExtensions
{
    /// <summary>
    /// Logical `AndAlso` 2 expressions
    /// This is the implementation of M.Gravell from (https://stackoverflow.com/questions/457316/combining-two-expressions-expressionfunct-bool)
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    public static Expression<Func<T, bool>> AndAlso<T>(
        this Expression<Func<T, bool>> expr1,
        Expression<Func<T, bool>> expr2)
    {
        var parameter = Expression.Parameter(typeof(T));

        var leftVisitor = new ReplaceExpressionVisitor(expr1.Parameters[0], parameter);
        var left = leftVisitor.Visit(expr1.Body);

        var rightVisitor = new ReplaceExpressionVisitor(expr2.Parameters[0], parameter);
        var right = rightVisitor.Visit(expr2.Body);

        if (left == null || right == null)
        {
            throw new InvalidOperationException($"Unable to chain given expression (either or both of expression is null).");
        }

        return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(left, right), parameter);
    }
}