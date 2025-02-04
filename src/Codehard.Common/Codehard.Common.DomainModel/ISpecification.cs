namespace Codehard.Common.DomainModel;

/// <summary>
/// An interface for a specification.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface ISpecification<in T>
{
    /// <summary>
    /// Verify if the <paramref name="obj"/> is satisfied by this specification.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    bool IsSatisfiedBy(T obj);
}