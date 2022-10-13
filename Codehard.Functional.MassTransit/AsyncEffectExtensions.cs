using Codehard.Functional.Logger;
using Codehard.Functional.MassTransit.Interfaces;
using LanguageExt.Common;
using MassTransit;
using Microsoft.Extensions.Logging;
using System.Diagnostics.Contracts;

namespace Codehard.Functional.MassTransit;

public static class AsyncEffectExtensions
{
    [Pure]
    public static AffWithConsumeContext<T> WithConsumeContext<T>(
        this Aff<T> aff,
        ConsumeContext consumerContext)
    {
        return new AffWithConsumeContext<T>(
            aff,
            consumerContext);
    }
}

public struct AffWithConsumeContext<TRes>
{
    private readonly Aff<TRes> aff;

    private readonly ConsumeContext consumeContext;

    public AffWithConsumeContext(Aff<TRes> aff, ConsumeContext consumeContext)
    {
        this.aff = aff;
        this.consumeContext = consumeContext;
    }

    public async ValueTask<Fin<Unit>> RunAndResponseAsync<TSuccResp, TFailResp>(
        Func<Error, object> errorRespMsgFunc,
        ILogger? logger = default)
        where TSuccResp : class, ICorrelatableMessage
        where TFailResp : class, IFaultMessage
    {
        var @this = this;

        var respAff =
            aff.MatchAff(
                Succ: res =>
                    @this.consumeContext.RespondAsync<TSuccResp>(res)
                         .ToUnit()
                         .ToAff(),
                Fail: err =>
                    @this.consumeContext.RespondAsync<TFailResp>(errorRespMsgFunc(err))
                         .ToUnit()
                         .ToAff());

        var respFin = await respAff.Run();
        logger?.LogIfFail(respFin);

        return respFin;
    }

    public ValueTask<Fin<Unit>> RunAndResponseAsync<TSuccResp>(
        ILogger? logger = default)
        where TSuccResp : class, ICorrelatableMessage
    {
        var @this = this;
        var correlationId = @this.consumeContext.CorrelationId;

        return RunAndResponseAsync<TSuccResp, IFaultMessage>(
            err => new
            {
                Reason = err.Message,
                CorrelationId = correlationId
            },
            logger);
    }
}
