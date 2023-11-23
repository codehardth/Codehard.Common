// ReSharper disable InconsistentNaming
#pragma warning disable CS1591

using System;
using Codehard.Functional;
using LanguageExt.Common;
using static LanguageExt.Prelude;

// ReSharper disable once CheckNamespace
namespace LanguageExt;

public static class EffectExtensions
{
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
    
    public static Eff<A> GuardNotNone<A>(this Eff<Option<A>> ma, Error? error = default)
        => ma.Bind(a => a.ToEff().MapFail(err => error == default ? err : error));
    
    public static Eff<A> GuardNotNoneWithExpectedResultObject<A>(this Eff<Option<A>> ma, object resultObject)
        => ma.Bind(a => a.ToEff().MapFail(err => new ExpectedResultError(resultObject, err)));
}