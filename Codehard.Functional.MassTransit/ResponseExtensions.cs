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
    /// <typeparam name="T1">Success case</typeparam>
    /// <typeparam name="T2">Failure case</typeparam>
    /// <returns>Returns an instance of <see cref="T1"/> if the call is success, otherwise an error with <see cref="MassTransitFaultMessageException{T}"/> of <see cref="T2"/>.</returns>
    public static Aff<T1> ToAff<T1, T2>(this Task<Response<T1, T2>> response)
        where T1 : class
        where T2 : class, IFaultMessage
    {
        return
            Aff(async () => await response)
                .Bind(r =>
                {
                    r.Deconstruct(out var success, out var failure);

                    return
                        !success.IsCompletedSuccessfully
                            ? failure.ToAff()
                                .Map(f => Error.New(string.Empty,
                                    (Exception) new MassTransitFaultMessageException<T2>(f.Message)))
                                .Bind(FailAff<T1>)
                            : success.ToAff().Map(s => s.Message);
                });
    }
}