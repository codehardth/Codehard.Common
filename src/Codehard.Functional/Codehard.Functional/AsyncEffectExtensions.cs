// ReSharper disable InconsistentNaming
#pragma warning disable CS1591

using Codehard.Functional;

// ReSharper disable once CheckNamespace
namespace LanguageExt;

public static class AsyncEffectExtensions
{
    // #region With
    //
    // public static Eff<(A, B)> With<A, B>(this Eff<A> ma, Eff<B> mb)
    // {
    //     return ma.Bind(a => mb.Map(b => (a, b)));
    // }
    //
    // public static Eff<(A, B, C)> With<A, B, C>(this Eff<(A, B)> mab, Eff<C> mc)
    // {
    //     return
    //         mab.Bind(
    //             ab => mc.Map(c => (ab.Item1, ab.Item2, c)));
    // }
    //
    // #endregion
    //
    // #region Bind With
    //
    // public static Eff<(A, B)> BindWith<A, B>(this Eff<A> ma, Func<A, Eff<B>> f)
    // {
    //     return ma.Bind(a => f(a).Map(b => (a, b)));
    // }
    //
    // public static Eff<(A, B, C)> BindWith<A, B, C>(this Eff<(A, B)> mab, Func<A, B, Eff<C>> f)
    // {
    //     return
    //         mab.Bind(
    //             ab => f(ab.Item1, ab.Item2).Map(c => (ab.Item1, ab.Item2, c)));
    // }
    //
    // #endregion
    //
    
    //
    // #region IgnoreFail
    //
    // /// <summary>
    // /// Ignore fail state and go back to success state with unit result
    // /// </summary>
    // public static Eff<Unit> IgnoreFail<A>(this Eff<A> ma, Action<Error> errorAction)
    // {
    //     return ma.Match(
    //         Succ: _ => unit,
    //         Fail: err =>
    //         {
    //             errorAction(err);
    //             return unit;
    //         });
    // }
    //
    // /// <summary>
    // /// Ignore fail state and go back to success state with unit result
    // /// </summary>
    // public static Eff<Unit> IgnoreFail<A>(this Eff<A> ma, Func<Error, Unit> errorF)
    // {
    //     return ma.Match(
    //         Succ: _ => unit,
    //         Fail: errorF);
    // }
    //
    // #endregion
}