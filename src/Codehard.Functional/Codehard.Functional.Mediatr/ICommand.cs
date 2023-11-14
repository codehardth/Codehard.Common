using LanguageExt;

// ReSharper disable once CheckNamespace
namespace MediatR;

public interface ICommand<TCommandResult> : IRequest<Fin<TCommandResult>>
{
}