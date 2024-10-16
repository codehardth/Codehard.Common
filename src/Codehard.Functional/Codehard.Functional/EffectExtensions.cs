// ReSharper disable InconsistentNaming
#pragma warning disable CS1591

using Codehard.Functional;
using LanguageExt.Common;

// ReSharper disable once CheckNamespace
namespace LanguageExt;

public static class EffectExtensions
{
    #region With
    
    public static Eff<(A, B)> With<A, B>(this Eff<A> ma, Eff<B> mb)
    {
        return ma.Bind(a => mb.Map(b => (a, b)));
    }
    
    public static Eff<(A, B, C)> With<A, B, C>(this Eff<(A, B)> mab, Eff<C> mc)
    {
        return
            mab.Bind(
                ab => mc.Map(c => (ab.Item1, ab.Item2, c)));
    }
    
    #endregion
    
    #region Guard
    
    public static Eff<A> Guard<A>(
        this Eff<A> ma, Func<A, bool> predicate,
        Error error)
        => ma.Guard(predicate, _ => error);
    
    public static Eff<A> Guard<A>(
        this Eff<A> ma, Func<A, bool> predicate,
        Func<A, Error> errorMsgFunc)
        => ma.Bind(
            a =>
                predicate(a)
                    ? SuccessEff(a)
                    : FailEff<A>(errorMsgFunc(a)));
    
    public static Eff<(A, B)> Guard<A, B>(
        this Eff<(A, B)> mab, Func<A, B, bool> predicate,
        Error error)
        => mab.Guard(predicate, (_, _) => error);

    public static Eff<(A, B)> Guard<A, B>(
        this Eff<(A, B)> mab, Func<A, B, bool> predicate,
        Func<A, B, Error> errorMsgFunc)
        => mab.Bind(
            ab =>
                predicate(ab.Item1, ab.Item2)
                    ? SuccessEff(ab)
                    : FailEff<(A, B)>(
                        errorMsgFunc(ab.Item1, ab.Item2)));

    public static Eff<(A, B, C)> Guard<A, B, C>(
        this Eff<(A, B, C)> mab, Func<A, B, C, bool> predicate,
        Error error)
        => mab.Guard(predicate, (_, _, _) => error);

    public static Eff<(A, B, C)> Guard<A, B, C>(
        this Eff<(A, B, C)> mab, Func<A, B, C, bool> predicate,
        Func<A, B, C, Error> errorMsgFunc)
        => mab.Bind(
            abc =>
                predicate(abc.Item1, abc.Item2, abc.Item3)
                    ? SuccessEff(abc)
                    : FailEff<(A, B, C)>(
                        errorMsgFunc(abc.Item1, abc.Item2, abc.Item3)));
    
    public static Eff<A> GuardNotNone<A>(this Eff<Option<A>> ma, string? errorMsg = default)
        => ma.GuardNotNone(errorMsg == default ? default : Error.New(errorMsg));
    
    public static Eff<A> GuardNotNone<A>(this Eff<Option<A>> ma, Error? error = default)
        => ma.Bind(a => a.ToEff().MapFail(err => error == default ? err : error));
    
    public static Eff<A> GuardNotNoneWithExpectedResultObject<A>(this Eff<Option<A>> ma, object resultObject)
        => ma.Bind(a => a.ToEff().MapFail(err => new ExpectedResultError(resultObject, err)));
    
    #endregion
    
    public static Eff<TResult> MapExpectedResultError<TResult>(
        this Eff<TResult> eff)
    {
        return
            eff.Match(
                    Succ: SuccessEff,
                    Fail: err => err.MapExpectedResultError<TResult>())
               .Flatten();
    }
}