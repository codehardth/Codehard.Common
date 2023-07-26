using LanguageExt.Common;

namespace Codehard.Functional;

/// <summary>
/// Provides a collection of utility methods to work with Aff&lt;Option&gt; monad and parallel execution of effects.
/// </summary>
public static class Prelude
{
    /// <summary>
    /// Lift an asynchronous effect into the Aff&lt;Option&gt; monad
    /// </summary>
    public static Aff<Option<A>> AffOption<A>(Func<ValueTask<A?>> f)
        where A : class
    {
        return LanguageExt.Aff<Option<A>>
            .Effect(async () => Optional(await f()));
    }

    /// <summary>
    /// Lift an asynchronous effect into the Aff&lt;Option&gt; monad
    /// </summary>
    public static Aff<Option<A>> AffOption<A>(Func<ValueTask<A?>> f)
        where A : struct
    {
        return LanguageExt.Aff<Option<A>>
            .Effect(async () => Optional(await f()));
    }
    
    /// <summary>
    /// Lift an synchronous effect into the Eff&lt;Option&gt; monad
    /// </summary>
    public static Eff<Option<A>> EffOption<A>(Func<A?> f)
        where A : class
    {
        return LanguageExt.Eff<Option<A>>
            .Effect(() => Optional(f()));
    }

    /// <summary>
    /// Lift an synchronous effect into the Eff&lt;Option&gt; monad
    /// </summary>
    public static Eff<Option<A>> EffOption<A>(Func<A?> f)
        where A : struct
    {
        return LanguageExt.Eff<Option<A>>
            .Effect(() => Optional(f()));
    }

    /// <summary>
    /// Run the effects in parallel, wait for them all to finish, then discard all the results into a single unit
    /// </summary>
    /// <param name="affs"></param>
    /// <returns></returns>
    public static Aff<Unit> IterParallel<A>(params Aff<A>[] affs)
    {
        return
            Aff(async () =>
                await Task.WhenAll(affs.Map(aff => aff.Run().AsTask())))
            .Bind(fins =>
                fins.Any(f => f.IsFail)
                    ? FailAff<Unit>(
                        Error.Many(
                            fins.Filter(f => f.IsFail)
                                .Match(Succ: _ => Error.New(string.Empty), Fail: err => err)
                                .ToArray()))
                    : unitAff);
    }
    
    /// <summary>
    /// Wrap Task of no returned result in Aff&lt;Unit&gt;
    /// </summary>
    public static Aff<Unit> AffUnit(Func<ValueTask> f)
    {
        return LanguageExt.Aff<Unit>
            .Effect(async () =>
            {
                await f();

                return unit;
            });
    }
}