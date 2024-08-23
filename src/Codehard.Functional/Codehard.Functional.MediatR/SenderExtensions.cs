using LanguageExt;

using static LanguageExt.Prelude;

// ReSharper disable once CheckNamespace
namespace MediatR;

public static class SenderExtensions
{
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