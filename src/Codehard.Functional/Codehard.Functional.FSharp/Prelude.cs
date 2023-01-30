using LanguageExt;
using LanguageExt.Common;
using Microsoft.FSharp.Core;
using static LanguageExt.Prelude;
using Unit = LanguageExt.Unit;

namespace Codehard.Functional.FSharp;

public static class Prelude
{
    /// <summary>
    /// Wrap execution that returns Task of F# Result in Aff
    /// </summary>
    public static Aff<T> Aff<T, TError>(
        Func<Task<FSharpResult<T, TError>>> resultTaskF,
        Func<TError, Error> errorMapper)
    {
        return
            LanguageExt.Aff<FSharpResult<T, TError>>
                .Effect(async () => await resultTaskF())
                .Bind(result =>
                    result.IsError
                        ? FailAff<T>(errorMapper(result.ErrorValue))
                        : SuccessAff(result.ResultValue));
    }
    
    /// <summary>
    /// Wrap execution that returns Task of F# Unit in Aff
    /// </summary>
    public static Aff<Unit> Aff<TError>(
        Func<Task<FSharpResult<Microsoft.FSharp.Core.Unit, TError>>> resultTaskF,
        Func<TError, Error> errorMapper)
    {
        return
            LanguageExt.Aff<FSharpResult<Microsoft.FSharp.Core.Unit, TError>>
                .Effect(async () => await resultTaskF())
                .Bind(result =>
                    result.IsError
                        ? FailAff<Unit>(errorMapper(result.ErrorValue))
                        : unitAff);
    }

    /// <summary>
    /// Wrap execution that returns F# Result in Eff
    /// </summary>
    public static Eff<T> Eff<T, TError>(
        Func<FSharpResult<T, TError>> resultTaskF,
        Func<TError, Error> errorMapper)
    {
        return
            LanguageExt.Eff<FSharpResult<T, TError>>
                .Effect(resultTaskF)
                .Bind(result =>
                    result.IsError
                        ? FailEff<T>(errorMapper(result.ErrorValue))
                        : SuccessEff(result.ResultValue));
    }
}