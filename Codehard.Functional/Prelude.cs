using LanguageExt.Common;

namespace Codehard.Functional;

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
    public static Aff<Option<A>> AffOption<A>(Func<ValueTask<Nullable<A>>> f)
        where A : struct
    {
        return LanguageExt.Aff<Option<A>>
            .Effect(async () => Optional(await f()));
    }

    /// <summary>
    /// Run the effects in parallel, wait for them all to finish, then discard all the results into a single unit
    /// </summary>
    /// <param name="affs"></param>
    /// <returns></returns>
    public static Aff<Unit> IterParallel<A>(params Aff<A>[] affs)
    {
        return Aff(async () =>
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
}