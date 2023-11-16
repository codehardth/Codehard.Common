using LanguageExt;

// ReSharper disable once CheckNamespace
namespace MediatR;

public interface IQuery<TQueryResult> : IRequest<Fin<TQueryResult>>
{
}