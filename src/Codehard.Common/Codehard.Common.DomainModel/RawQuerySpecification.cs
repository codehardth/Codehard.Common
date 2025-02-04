namespace Codehard.Common.DomainModel;

/// <summary>
/// Represents a raw query specification for entities of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of the entity.</typeparam>
public abstract class RawQuerySpecification<T> : IRawQuerySpecification<T>
{
    /// <summary>
    /// Gets the raw query string.
    /// </summary>
    public string Query { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="RawQuerySpecification{T}"/> class with the specified query.
    /// </summary>
    /// <param name="query">The raw query string.</param>
    protected RawQuerySpecification(string query)
    {
        this.Query = query;
    }

    /// <summary>
    /// Determines whether the specified object satisfies the specification.
    /// </summary>
    /// <param name="obj">The object to test.</param>
    /// <returns>Always throws a <see cref="NotSupportedException"/>.</returns>
    /// <exception cref="NotSupportedException">Thrown always as this method is not supported.</exception>
    public bool IsSatisfiedBy(T obj)
        => throw new NotSupportedException();
}