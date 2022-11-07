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
}