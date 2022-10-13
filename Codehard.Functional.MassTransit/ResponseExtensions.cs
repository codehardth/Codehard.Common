using Codehard.Functional.MassTransit.Exceptions;
using Codehard.Functional.MassTransit.Interfaces;
using LanguageExt.Common;
using MassTransit;

namespace Codehard.Functional.MassTransit;

public static class ResponseExtensions
{
    /// <summary>
    /// Run request client as an async effect.
    /// </summary>
    /// <param name="response"></param>
    /// <typeparam name="TSucc">Success case</typeparam>
    /// <typeparam name="TFail">Failure case</typeparam>
    /// <returns>Returns an instance of <see cref="TSucc"/> if the call is success, otherwise an error with <see cref="MassTransitFaultMessageException{T}"/> of <see cref="TFail"/>.</returns>
    public static Aff<TSucc> ToAff<TSucc, TFail>(this Task<Response<TSucc, TFail>> response)
        where TSucc : class, ICorrelatableMessage
        where TFail : class, IFaultMessage
    {
        return
            Aff(async () => await response)
                .Bind(r =>
                {
                    r.Deconstruct(out var success, out var failure);

                    return
                        !success.IsCompletedSuccessfully
                            ? failure.ToAff()
                                .Map(f =>
                                    Error.New(
                                        string.Empty,
                                        (Exception) new MassTransitFaultMessageException<TFail>(f.Message)))
                                .Bind(FailAff<TSucc>)
                            : success.ToAff().Map(s => s.Message);
                });
    }
}