using LanguageExt;

// ReSharper disable once CheckNamespace
namespace MediatR;

public interface IQueryHandler<in TQuery, TQueryResult> : IRequestHandler<TQuery, Fin<TQueryResult>>
    where TQuery : IQuery<TQueryResult>
{
}