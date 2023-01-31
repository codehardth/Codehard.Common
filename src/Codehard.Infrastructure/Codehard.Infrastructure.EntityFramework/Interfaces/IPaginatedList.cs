namespace Codehard.Infrastructure.EntityFramework.Interfaces;

public interface IPaginatedList<out T> : IReadOnlyList<T>
{
    int CurrentPage { get; }

    int TotalPages { get; }

    int PageSize { get; }

    int TotalCount { get; }

    bool HasPrevious { get; }

    bool HasNext { get; }
}