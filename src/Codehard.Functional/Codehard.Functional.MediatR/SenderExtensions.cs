using LanguageExt;

using static LanguageExt.Prelude;

// ReSharper disable once CheckNamespace
namespace MediatR;

public static class SenderExtensions
{
    public static Aff<TCommandResult> SendCommandAff<TCommand, TCommandResult>(
        this ISender sender,
        TCommand command)
        where TCommand : ICommand<TCommandResult>
    {
        return
            Aff(async () =>
                    await sender.Send(command)
                                .Map(x => x.ToAff()))
                .Flatten();
    }
    
    public static Aff<TQueryResult> SendQueryAff<TQuery, TQueryResult>(
        this ISender sender,
        TQuery query)
        where TQuery : IQuery<TQueryResult>
    {
        return
            Aff(async () =>
                    await sender.Send(query)
                                .Map(x => x.ToAff()))
                .Flatten();
    }
}