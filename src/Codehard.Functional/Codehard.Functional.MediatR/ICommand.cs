using LanguageExt;

// ReSharper disable once CheckNamespace
namespace MediatR;

/// <summary>
/// Defines a command with a return value
/// </summary>
public interface ICommand<TCommandResult> : IRequest<Fin<TCommandResult>>
{
}