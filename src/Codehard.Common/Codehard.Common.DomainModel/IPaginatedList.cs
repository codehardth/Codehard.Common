namespace Codehard.Common.DomainModel;

/// <summary>
/// Represents a paginated list of items.
/// </summary>
/// <typeparam name="T">The type of items in the list.</typeparam>
public interface IPaginatedList<out T> : IReadOnlyList<T>
{
    /// <summary>
    /// Gets the current page number.
    /// </summary>
    int CurrentPage { get; }

    /// <summary>
    /// Gets the total number of pages.
    /// </summary>
    int TotalPages { get; }

    /// <summary>
    /// Gets the number of items per page.
    /// </summary>
    int PageSize { get; }

    /// <summary>
    /// Gets the total number of items.
    /// </summary>
    int TotalCount { get; }

    /// <summary>
    /// Gets a value indicating whether there is a previous page.
    /// </summary>
    bool HasPrevious { get; }

    /// <summary>
    /// Gets a value indicating whether there is a next page.
    /// </summary>
    bool HasNext { get; }
}