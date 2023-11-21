using LanguageExt;

// ReSharper disable once CheckNamespace
namespace MediatR;

/// <summary>
/// Defines a query with a return value
/// </summary>
public interface IQuery<TQueryResult> : IRequest<Fin<TQueryResult>>
{
}