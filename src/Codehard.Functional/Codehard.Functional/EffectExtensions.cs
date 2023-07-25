using LanguageExt.Common;
#pragma warning disable CS1591

namespace Codehard.Functional;

public static class EffectExtensions
{
    public static Eff<A> Guard<A>(
        this Eff<A> ma, Func<A, bool> predicate, 
        Error error)
        => ma.Bind(
            a =>
                predicate(a)
                    ? SuccessEff(a)
                    : FailEff<A>(error));
    
    public static Eff<A> Guard<A>(
        this Eff<A> ma, Func<A, bool> predicate,
        Func<A, Error> errorMsgFunc)
        => ma.Bind(
            a =>
                predicate(a)
                    ? SuccessEff(a)
                    : FailEff<A>(errorMsgFunc(a)));
}