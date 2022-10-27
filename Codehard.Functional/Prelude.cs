namespace Codehard.Functional;

public static class Prelude
{
    public static Aff<Option<A>> AffOption<A>(Func<Task<A?>> f)
        where A : class
    {
        return LanguageExt.Aff<Option<A>>
            .Effect(async () => Optional(await f()));
    }

    public static Aff<Option<A>> AffOption<A>(Func<Task<Nullable<A>>> f)
        where A : struct
    {
        return LanguageExt.Aff<Option<A>>
            .Effect(async () => Optional(await f()));
    }

    public static Aff<Option<A>> AffOption<A>(Func<ValueTask<A?>> f)
        where A : class
    {
        return LanguageExt.Aff<Option<A>>
            .Effect(async () => Optional(await f()));
    }

    public static Aff<Option<A>> AffOption<A>(Func<ValueTask<Nullable<A>>> f)
        where A : struct
    {
        return LanguageExt.Aff<Option<A>>
            .Effect(async () => Optional(await f()));
    }
}