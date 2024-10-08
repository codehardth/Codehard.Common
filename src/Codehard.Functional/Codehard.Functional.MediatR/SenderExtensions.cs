using LanguageExt;

using static LanguageExt.Prelude;

// ReSharper disable once CheckNamespace
namespace MediatR;

/// <summary>
/// Provides extension methods for the <see cref="ISender"/> interface.
/// </summary>
public static class SenderExtensions
{
    /// <summary>
    /// Sends a command asynchronously within an Eff monad.
    /// </summary>
    /// <typeparam name="TCommand">The type of the command.</typeparam>
    /// <typeparam name="TCommandResult">The type of the command result.</typeparam>
    /// <param name="sender">The sender to use for sending the command.</param>
    /// <param name="command">The command to send.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>An <see cref="Eff{TCommandResult}"/> representing the asynchronous operation.</returns>
    public static Eff<TCommandResult> SendCommandEff<TCommand, TCommandResult>(
        this ISender sender,
        TCommand command,
        CancellationToken cancellationToken = default)
        where TCommand : ICommand<TCommandResult>
    {
        return
            liftEff(async () =>
                    await sender.Send(command, cancellationToken)
                                .Map(x => x.ToEff()))
                .Flatten();
    }
    
    /// <summary>
    /// Sends a query asynchronously within an Eff monad.
    /// </summary>
    /// <typeparam name="TQuery">The type of the query.</typeparam>
    /// <typeparam name="TQueryResult">The type of the query result.</typeparam>
    /// <param name="sender">The sender to use for sending the query.</param>
    /// <param name="query">The query to send.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>An <see cref="Eff{TQueryResult}"/> representing the asynchronous operation.</returns>
    public static Eff<TQueryResult> SendQueryEff<TQuery, TQueryResult>(
        this ISender sender,
        TQuery query,
        CancellationToken cancellationToken = default)
        where TQuery : IQuery<TQueryResult>
    {
        return
            liftEff(async () =>
                    await sender.Send(query, cancellationToken)
                                .Map(x => x.ToEff()))
                .Flatten();
    }
}