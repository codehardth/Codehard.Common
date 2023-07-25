using LanguageExt.Common;
#pragma warning disable CS1591

namespace Codehard.Functional;

public static class AsyncEffectExtensions
{
    #region With

    public static Aff<(A, B)> With<A, B>(this Aff<A> ma, Aff<B> mb)
    {
        return ma.Bind(a => mb.Map(b => (a, b)));
    }

    public static Aff<(A, B, C)> With<A, B, C>(this Aff<(A, B)> mab, Aff<C> mc)
    {
        return
            mab.Bind(
                ab => mc.Map(c => (ab.Item1, ab.Item2, c)));
    }

    #endregion

    #region Bind With

    public static Aff<(A, B)> BindWith<A, B>(this Aff<A> ma, Func<A, Aff<B>> f)
    {
        return ma.Bind(a => f(a).Map(b => (a, b)));
    }

    public static Aff<(A, B, C)> BindWith<A, B, C>(this Aff<(A, B)> mab, Func<A, B, Aff<C>> f)
    {
        return
            mab.Bind(
                ab => f(ab.Item1, ab.Item2).Map(c => (ab.Item1, ab.Item2, c)));
    }

    #endregion

    #region Guard

    public static Aff<A> Guard<A>(
        this Aff<A> ma, Func<A, bool> predicate, 
        Error error)
        => ma.Bind(
            a =>
                predicate(a)
                    ? SuccessAff(a)
                    : FailAff<A>(error));

    public static Aff<A> Guard<A>(
        this Aff<A> ma, Func<A, bool> predicate,
        Func<A, Error> errorMsgFunc)
        => ma.Bind(
            a =>
                predicate(a)
                    ? SuccessAff(a)
                    : FailAff<A>(errorMsgFunc(a)));

    public static Aff<(A, B)> Guard<A, B>(
        this Aff<(A, B)> mab, Func<A, B, bool> predicate,
        Error error)
        => mab.Bind(
            ab =>
                predicate(ab.Item1, ab.Item2)
                    ? SuccessAff(ab)
                    : FailAff<(A, B)>(error));

    public static Aff<(A, B)> Guard<A, B>(
        this Aff<(A, B)> mab, Func<A, B, bool> predicate,
        Func<A, B, Error> errorMsgFunc)
        => mab.Bind(
            ab =>
                predicate(ab.Item1, ab.Item2)
                    ? SuccessAff(ab)
                    : FailAff<(A, B)>(
                        errorMsgFunc(ab.Item1, ab.Item2)));

    public static Aff<(A, B, C)> Guard<A, B, C>(
        this Aff<(A, B, C)> mab, Func<A, B, C, bool> predicate,
        Error error)
        => mab.Bind(
            abc =>
                predicate(abc.Item1, abc.Item2, abc.Item3)
                    ? SuccessAff(abc)
                    : FailAff<(A, B, C)>(error));

    public static Aff<(A, B, C)> Guard<A, B, C>(
        this Aff<(A, B, C)> mab, Func<A, B, C, bool> predicate,
        Func<A, B, C, Error> errorMsgFunc)
        => mab.Bind(
            abc =>
                predicate(abc.Item1, abc.Item2, abc.Item3)
                    ? SuccessAff(abc)
                    : FailAff<(A, B, C)>(
                        errorMsgFunc(abc.Item1, abc.Item2, abc.Item3)));

    public static Aff<A> GuardNotNone<A>(this Aff<Option<A>> ma, Error? error = default)
        => ma.Bind(a => a.ToAff().MapFail(err => error == default ? err : error));

    #endregion

    #region IgnoreFail

    /// <summary>
    /// Ignore fail state and go back to success state with unit result
    /// </summary>
    public static Aff<Unit> IgnoreFail<A>(this Aff<A> ma, Action<Error> errorAction)
    {
        return ma.Match(
            Succ: _ => unit,
            Fail: err =>
            {
                errorAction(err);
                return unit;
            });
    }

    /// <summary>
    /// Ignore fail state and go back to success state with unit result
    /// </summary>
    public static Aff<Unit> IgnoreFail<A>(this Aff<A> ma, Func<Error, Unit> errorF)
    {
        return ma.Match(
            Succ: _ => unit,
            Fail: err => errorF(err));
    }

    #endregion
}