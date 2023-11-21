using LanguageExt;

// ReSharper disable once CheckNamespace
namespace MediatR;

/// <summary>
/// Defines a query handler
/// </summary>
public interface IQueryHandler<in TQuery, TQueryResult> : IRequestHandler<TQuery, Fin<TQueryResult>>
    where TQuery : IQuery<TQueryResult>
{
}