namespace Codehard.Common.DomainModel;

/// <summary>
/// A specification with a raw query as a condition.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IRawQuerySpecification<in T> : ISpecification<T>
{
    /// <summary>
    /// Gets the raw query string.
    /// </summary>
    public string Query { get; }
}