using System.Linq.Expressions;

namespace Codehard.Common.DomainModel;

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IExpressionSpecification<T> : ISpecification<T>
{
    /// <summary>
    /// </summary>
    public Expression<Func<T, bool>> Expression { get; }
}