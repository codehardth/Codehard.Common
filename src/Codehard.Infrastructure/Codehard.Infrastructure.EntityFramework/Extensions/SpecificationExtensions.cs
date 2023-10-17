using Microsoft.EntityFrameworkCore;

namespace Codehard.Common.DomainModel.Extensions;

public static class SpecificationExtensions
{
    /// <summary>
    /// Apply a specification to <see cref="DbSet{TEntity}"/>.
    /// </summary>
    /// <param name="dbSet"></param>
    /// <param name="specification"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IQueryable<T> Apply<T>(this DbSet<T> dbSet, ISpecification<T> specification) where T : class
    {
        return specification switch
        {
            IRawQuerySpecification<T> rawQuery => dbSet.FromSqlRaw(rawQuery.Query),
            IExpressionSpecification<T> expression => dbSet.Where(expression.Expression),
            _ => throw new NotSupportedException(),
        };
    }
}