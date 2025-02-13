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
    /// <returns>An <see cref="LanguageExt.Eff{TCommandResult}"/> representing the asynchronous operation.</returns>
    public static Eff<TCommandResult> SendCommandEff<TCommand, TCommandResult>(
        this ISender sender,
        TCommand command)
        where TCommand : ICommand<TCommandResult>
    {
        Eff<Fin<TCommandResult>> eff =
            liftIO(async env => await sender.Send(command, env.Token));
            
        return eff.Bind(x => x.ToEff());
    }
    
    /// <summary>
    /// Sends a query asynchronously within an Eff monad.
    /// </summary>
    /// <typeparam name="TQuery">The type of the query.</typeparam>
    /// <typeparam name="TQueryResult">The type of the query result.</typeparam>
    /// <param name="sender">The sender to use for sending the query.</param>
    /// <param name="query">The query to send.</param>
    /// <returns>An <see cref="LanguageExt.Eff{TQueryResult}"/> representing the asynchronous operation.</returns>
    public static Eff<TQueryResult> SendQueryEff<TQuery, TQueryResult>(
        this ISender sender,
        TQuery query)
        where TQuery : IQuery<TQueryResult>
    {
        Eff<Fin<TQueryResult>> eff =
            liftIO(async env => await sender.Send(query, env.Token));
        
        return eff.Bind(x => x.ToEff());
    }
}