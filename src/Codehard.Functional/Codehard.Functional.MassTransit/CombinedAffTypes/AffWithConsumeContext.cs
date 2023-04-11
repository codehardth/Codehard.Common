using Codehard.Functional.Logger;
using Codehard.Functional.MassTransit.Interfaces;
using LanguageExt.Common;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Codehard.Functional.MassTransit.CombinedAffTypes
{
    /// <summary>
    /// Aff pair with Masstransit ConsumeContext
    /// </summary>
    public readonly struct AffWithConsumeContext<TRes>
    {
        private readonly Aff<TRes> aff;

        private readonly ConsumeContext consumeContext;

        /// <summary>
        /// Construct Aff with ConsumeContext
        /// </summary>
        public AffWithConsumeContext(Aff<TRes> aff, ConsumeContext consumeContext)
        {
            this.aff = aff;
            this.consumeContext = consumeContext;
        }

        /// <summary>
        /// Invoke the effect and response result to caller
        /// </summary>
        public async ValueTask<Fin<Unit>> RunAndResponseAsync<TSuccResp, TFailResp>(
            Func<Error, object> errorRespMsgFunc,
            ILogger? logger = default)
            where TSuccResp : class, ICorrelatableMessage
            where TFailResp : class, IFaultMessage
        {
            var consumeContext = this.consumeContext;

            var respAff =
                aff.Match(
                        Succ: res =>
                            consumeContext.RespondAsync<TSuccResp>(res),
                        Fail: err =>
                            consumeContext.RespondAsync<TFailResp>(errorRespMsgFunc(err)))
                    .Map(static _ => unit);

            var respFin = await respAff.Run();
            logger?.LogIfFail(respFin);

            return respFin;
        }

        /// <summary>
        /// Invoke the effect and response result to caller
        /// </summary>
        public ValueTask<Fin<Unit>> RunAndResponseAsync<TSuccResp>(
            ILogger? logger = default)
            where TSuccResp : class, ICorrelatableMessage
        {
            var correlationId = this.consumeContext.CorrelationId;

            return RunAndResponseAsync<TSuccResp, IFaultMessage>(
                err => new
                {
                    Reason = err.Message,
                    CorrelationId = correlationId
                },
                logger);
        }
    }
}