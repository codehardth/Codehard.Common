using System.Linq.Expressions;

namespace Codehard.Common.DomainModel;

/// <summary>
/// Represents a specification that is based on an expression.
/// </summary>
/// <typeparam name="T">The type of object that the specification operates on.</typeparam>
public interface IExpressionSpecification<T> : ISpecification<T>
{
    /// <summary>
    /// Gets the expression associated with the specification.
    /// </summary>
    public Expression<Func<T, bool>> Expression { get; }
}