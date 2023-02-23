using System.Linq.Expressions;

namespace Infrastructure.Test;

public static class Extensions
{
    public static IQueryable<T> Filter<T>(this IQueryable<T> source, Expression<Func<T, bool>> predicate)
        where T : class
    {
        var expr = (Expression<Func<T, bool>>)new OptionExpressionVisitor().Visit(predicate);
        return source.Where(expr);
    }
}