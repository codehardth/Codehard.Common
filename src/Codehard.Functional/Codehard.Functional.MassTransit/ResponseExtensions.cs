using Codehard.Functional.MassTransit.Exceptions;
using Codehard.Functional.MassTransit.Interfaces;
using LanguageExt.Common;

// ReSharper disable once CheckNamespace
namespace MassTransit;

/// <summary>
/// Masstransit <see cref="Response{T1, T2}"/> extensions
/// </summary>
public static class ResponseExtensions
{
    /// <summary>
    /// Run request client as an async effect.
    /// </summary>
    /// <typeparam name="TSucc">Success case</typeparam>
    /// <typeparam name="TFail">Failure case</typeparam>
    /// <returns>
    /// An instance of <typeparamref name="TSucc"/> if the call is success,
    /// otherwise an error with <see cref="MassTransitFaultMessageException{T}"/> of <typeparamref name="TFail"/>.
    /// </returns>
    public static Eff<TSucc> ToEff<TSucc, TFail>(this Task<Response<TSucc, TFail>> response)
        where TSucc : class, ICorrelatableMessage
        where TFail : class, IFaultMessage
    {
        return
            liftEff(async () => await response)
                .Bind(r =>
                {
                    r.Deconstruct(out var success, out var failure);

                    return
                        !success.IsCompletedSuccessfully
                            ? liftEff(() => failure)
                                .Map(f =>
                                    Error.New(
                                        string.Empty,
                                        (Exception) new MassTransitFaultMessageException<TFail>(f.Message)))
                                .Bind(FailEff<TSucc>)
                            : liftEff(() => success).Map(s => s.Message);
                });
    }
}