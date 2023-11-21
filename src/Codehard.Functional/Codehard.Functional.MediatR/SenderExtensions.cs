using LanguageExt;

using static LanguageExt.Prelude;

// ReSharper disable once CheckNamespace
namespace MediatR;

public static class SenderExtensions
{
    public static Aff<TCommandResult> SendCommandAff<TCommand, TCommandResult>(
        this ISender sender,
        TCommand command,
        CancellationToken cancellationToken = default)
        where TCommand : ICommand<TCommandResult>
    {
        return
            Aff(async () =>
                    await sender.Send(command, cancellationToken)
                                .Map(x => x.ToAff()))
                .Flatten();
    }
    
    public static Aff<TQueryResult> SendQueryAff<TQuery, TQueryResult>(
        this ISender sender,
        TQuery query,
        CancellationToken cancellationToken = default)
        where TQuery : IQuery<TQueryResult>
    {
        return
            Aff(async () =>
                    await sender.Send(query, cancellationToken)
                                .Map(x => x.ToAff()))
                .Flatten();
    }
}