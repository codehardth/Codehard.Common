using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Codehard.Common.AspNetCore.ActionResults;

/// <summary>
/// Represents a paginated query result for use as an IActionResult in ASP.NET Core MVC.
/// </summary>
/// <typeparam name="T">The type of the data returned in the paginated query result.</typeparam>
public sealed class PaginatedQueryResult<T> : IActionResult
{
    /// <summary>
    /// Gets an empty paginated query result with no data and total records set to 0.
    /// </summary>
    public static readonly PaginatedQueryResult<T> Empty = new(Enumerable.Empty<T>(), 0);

    /// <summary>
    /// Initializes a new instance of the <see cref="PaginatedQueryResult{T}"/> class.
    /// </summary>
    /// <param name="data">The paginated data.</param>
    /// <param name="totalRecords">The total number of records.</param>
    public PaginatedQueryResult(IEnumerable<T> data, int totalRecords)
    {
        this.Data = data;
        this.TotalRecords = totalRecords;
    }

    /// <summary>
    /// Gets the paginated data.
    /// </summary>
    public IEnumerable<T> Data { get; }

    /// <summary>
    /// Gets the total number of records.
    /// </summary>
    public int TotalRecords { get; }

    /// <summary>
    /// Executes the result operation of the action method asynchronously. This method is called by MVC to process
    /// the result of an action method.
    /// </summary>
    /// <param name="context">The context in which the result is executed. The context information includes
    /// information about the action that was executed and request information.</param>
    /// <returns>A task that represents the asynchronous execute operation.</returns>
    public Task ExecuteResultAsync(ActionContext context)
    {
        return context.HttpContext.Response.WriteAsJsonAsync(this);
    }
}