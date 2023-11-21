using LanguageExt;

// ReSharper disable once CheckNamespace
namespace MediatR;

/// <summary>
/// Defines a command handler
/// </summary>
public interface ICommandHandler<in TCommand, TCommandResult> : IRequestHandler<TCommand, Fin<TCommandResult>>
    where TCommand : ICommand<TCommandResult>
{
}