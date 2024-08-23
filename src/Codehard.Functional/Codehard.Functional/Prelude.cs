// ReSharper disable InconsistentNaming
#pragma warning disable CS1591

namespace Codehard.Functional;

/// <summary>
/// Provides a collection of utility methods to work with Aff&lt;Option&gt; monad and parallel execution of effects.
/// </summary>
public static class Prelude
{
    /// <summary>
    /// Lift an asynchronous effect into the Aff&lt;Option&gt; monad
    /// </summary>
    public static Eff<Option<A>> EffOption<A>(Func<ValueTask<A?>> f)
        where A : class
    {
        return liftEff(async () => Optional(await f()));
    }

    /// <summary>
    /// Lift an asynchronous effect into the Aff&lt;Option&gt; monad
    /// </summary>
    public static Eff<Option<A>> EffOption<A>(Func<ValueTask<A?>> f)
        where A : struct
    {
        return liftEff(async () => Optional(await f()));
    }
    
    /// <summary>
    /// Lift an synchronous effect into the Eff&lt;Option&gt; monad
    /// </summary>
    public static Eff<Option<A>> EffOption<A>(Func<A?> f)
        where A : class
    {
        return liftEff(() => Optional(f()));
    }

    /// <summary>
    /// Lift an synchronous effect into the Eff&lt;Option&gt; monad
    /// </summary>
    public static Eff<Option<A>> EffOption<A>(Func<A?> f)
        where A : struct
    {
        return liftEff(() => Optional(f()));
    }

    /// <summary>
    /// Run the effects in parallel, wait for them all to finish, then discard all the results into a single unit
    /// </summary>
    /// <param name="affs"></param>
    /// <returns></returns>
    public static Eff<Unit> IterParallel<A>(params Eff<A>[] affs)
    {
        return
            liftEff(async () => await Task.WhenAll(affs.Select(aff => aff.Run().AsTask())))
                .Bind(fins =>
                    fins.Any(f => f.IsFail)
                        ? FailEff<Unit>(
                            Error.Many(
                                fins.Where(f => f.IsFail)
                                    .Match(Succ: _ => Error.New(string.Empty), Fail: err => err)
                                    .ToArray()))
                        : unitEff);
    }
    
    /// <summary>
    /// Wrap Task of no returned result in Aff&lt;Unit&gt;
    /// </summary>
    public static Eff<Unit> EffUnit(Func<ValueTask> f)
    {
        return liftEff(async () =>
            {
                await f();
    
                return unit;
            });
    }
    
    /// <summary>
    /// Wraps a synchronous action into an Eff&lt;Unit&gt; monad.
    /// </summary>
    /// <param name="action">The action to be executed.</param>
    /// <returns>
    /// An Eff&lt;Unit&gt; monad that represents the synchronous operation. The Eff monad wraps a unit that is returned after the action is executed.
    /// </returns>
    public static Eff<Unit> EffUnit(Action action)
    {
        return liftEff(() =>
            {
                action();

                return unit;
            });
    }
}