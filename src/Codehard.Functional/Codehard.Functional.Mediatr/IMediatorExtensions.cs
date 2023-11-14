using LanguageExt;

using static LanguageExt.Prelude;

// ReSharper disable once CheckNamespace
namespace MediatR;

public static class IMediatorExtensions
{
    public static Aff<TCommandResult> SendCommandAff<TCommand, TCommandResult>(
        this IMediator mediator,
        TCommand command)
        where TCommand : ICommand<TCommandResult>
    {
        return
            Aff(async () =>
                    await mediator.Send(command)
                                  .Map(x => x.ToAff()))
                .Flatten();
    }
    
    public static Aff<TQueryResult> SendQueryAff<TQuery, TQueryResult>(
        this IMediator mediator,
        TQuery query)
        where TQuery : IQuery<TQueryResult>
    {
        return
            Aff(async () =>
                    await mediator.Send(query)
                                  .Map(x => x.ToAff()))
                .Flatten();
    }
}