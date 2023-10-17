namespace Codehard.Common.DomainModel;

public abstract class RawQuerySpecification<T> : IRawQuerySpecification<T>
{
    public string Query { get; }

    protected RawQuerySpecification(string query)
    {
        this.Query = query;
    }

    public bool IsSatisfiedBy(T obj)
        => throw new NotSupportedException();
}