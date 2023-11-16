using LanguageExt;

// ReSharper disable once CheckNamespace
namespace MediatR;

public interface ICommandHandler<in TCommand, TCommandResult> : IRequestHandler<TCommand, Fin<TCommandResult>>
    where TCommand : ICommand<TCommandResult>
{
}