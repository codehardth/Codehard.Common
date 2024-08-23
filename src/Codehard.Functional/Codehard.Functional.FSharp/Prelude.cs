using LanguageExt;
using LanguageExt.Common;
using Microsoft.FSharp.Core;
using static LanguageExt.Prelude;
using Unit = LanguageExt.Unit;

namespace Codehard.Functional.FSharp;

/// <summary>
/// Provides a collection of utility methods for working with functional programming constructs in C#.
/// </summary>
public static class Prelude
{
    /// <summary>
    /// Wrap execution that returns Task of F# Result in Aff
    /// </summary>
    public static Eff<T> Eff<T, TError>(
        Func<Task<FSharpResult<T, TError>>> resultTaskF,
        Func<TError, Error> errorMapper)
    {
        return
            liftEff(async () => await resultTaskF())
                .Bind(result =>
                    result.IsError
                        ? FailEff<T>(errorMapper(result.ErrorValue))
                        : SuccessEff(result.ResultValue));
    }
    
    /// <summary>
    /// Wrap execution that returns Task of F# Unit in Aff
    /// </summary>
    public static Eff<Unit> Aff<TError>(
        Func<Task<FSharpResult<Microsoft.FSharp.Core.Unit, TError>>> resultTaskF,
        Func<TError, Error> errorMapper)
    {
        return
            liftEff(async () => await resultTaskF())
                .Bind(result =>
                    result.IsError
                        ? FailEff<Unit>(errorMapper(result.ErrorValue))
                        : unitEff);
    }

    /// <summary>
    /// Wrap execution that returns F# Result in Eff
    /// </summary>
    public static Eff<T> Eff<T, TError>(
        Func<FSharpResult<T, TError>> resultTaskF,
        Func<TError, Error> errorMapper)
    {
        return
            liftEff(resultTaskF)
                .Bind(result =>
                    result.IsError
                        ? FailEff<T>(errorMapper(result.ErrorValue))
                        : SuccessEff(result.ResultValue));
    }
}