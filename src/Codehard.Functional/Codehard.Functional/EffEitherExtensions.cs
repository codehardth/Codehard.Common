#pragma warning disable CS1591

// ReSharper disable once CheckNamespace
namespace LanguageExt;

public static class EffEitherExtensions
{
    public static Eff<Either<TLeft, T>> MapNoneToLeft<T, TLeft>(
        this Eff<Option<T>> optEff, Func<TLeft> leftF)
    {
        return
            optEff.Map(
                valOpt =>
                    valOpt.ToEither(leftF));
    }

    public static Eff<Either<TLeft, T>> GuardToLeft<T, TLeft>(
        this Eff<T> eff,
        Func<T, bool> predicate,
        Func<TLeft> leftF)
    {
        return
            eff.Map(
                val =>
                    predicate(val)
                        ? Right<TLeft, T>(val)
                        : Left<TLeft, T>(leftF()));
    }

    public static Eff<Either<TLeft, T>> GuardEither<T, TLeft>(
        this Eff<Either<TLeft, T>> eitherValueEff,
        Func<T, bool> predicate,
        Func<TLeft> leftF)
    {
        return
            eitherValueEff.Map(
                eitherValue =>
                    eitherValue.Bind(
                        val =>
                            predicate(val)
                                ? Right<TLeft, T>(val)
                                : Left<TLeft, T>(leftF())));
    }

    public static Eff<Either<TLeft, T>> GuardEitherAsync<T, TLeft>(
        this Eff<Either<TLeft, T>> eitherValueEff,
        Func<T, ValueTask<bool>> predicateAsync,
        Func<T, TLeft> leftF)
    {
        return
            eitherValueEff
                .Bind(eitherValue =>
                    (from eitherValue2 in EitherT.lift<TLeft, Eff, T>(eitherValue)
                        from flag in liftEff(async () => await predicateAsync(eitherValue2))
                        from b in
                            flag
                                ? Right<TLeft, T>(eitherValue2)
                                : Left<TLeft, T>(leftF(eitherValue2))
                        select b)
                    .Run()
                );
    }

    public static Eff<Either<TLeft, T>> GuardEitherAsync<T, TLeft>(
        this Eff<Either<TLeft, T>> eitherValueEff,
        Func<T, bool> predicate,
        Func<T, ValueTask<TLeft>> leftAsync)
    {
        return
            eitherValueEff
                .Bind(eitherValue =>
                    (from eitherValue2 in EitherT.lift<TLeft, Eff, T>(eitherValue)
                        from flag in liftEff(() => predicate(eitherValue2))
                        from b in
                            flag
                                ? SuccessEff(Right<TLeft, T>(eitherValue2))
                                : liftEff(async () => Left<TLeft, T>(await leftAsync(eitherValue2)))
                        select b)
                    .Run().Map(x => x.Flatten<TLeft, T>())
                );
    }

    public static Eff<Either<TLeft, T>> GuardEitherAsync<T, TLeft>(
        this Eff<Either<TLeft, T>> eitherValueEff,
        Func<T, ValueTask<bool>> predicateAsync,
        Func<T, ValueTask<TLeft>> leftAsync)
    {
        return
            eitherValueEff
                .Bind(eitherValue =>
                    (from eitherValue2 in EitherT.lift<TLeft, Eff, T>(eitherValue)
                        from flag in liftEff(async () => await predicateAsync(eitherValue2))
                        from b in
                            flag
                                ? SuccessEff(Right<TLeft, T>(eitherValue2))
                                : liftEff(async () => Left<TLeft, T>(await leftAsync(eitherValue2)))
                        select b)
                    .Run().Map(x => x.Flatten<TLeft, T>())
                );
    }

    public static Eff<Either<TLeft, TRight>> MapRight<T, TRight, TLeft>(
        this Eff<Either<TLeft, T>> eitherValueEff,
        Func<T, TRight> mapF)
    {
        return
            eitherValueEff
                .Map(eitherValue =>
                    eitherValue.Map(mapF));
    }

    public static Eff<Either<TLeft, TRight>> MapRightAsync<T, TRight, TLeft>(
        this Eff<Either<TLeft, T>> eitherValueEff,
        Func<T, ValueTask<TRight>> mapAsync)
    {
        return
            eitherValueEff
                .Bind(eitherValue =>
                    (from eitherValue2 in EitherT.lift<TLeft, Eff, T>(eitherValue)
                        from b in liftEff(async () => await mapAsync(eitherValue2))
                        select b)
                    .Run()
                );
    }

    public static Eff<Either<TLeft, TRight>> DoIfRight<TRight, TLeft>(
        this Eff<Either<TLeft, TRight>> eitherValueEff,
        Action<TRight> f)
    {
        return
            eitherValueEff
                .Map(eitherValue =>
                    eitherValue.Do(f));
    }

    public static Eff<Either<TLeft, TRight>> DoIfRightAsync<TRight, TLeft>(
        this Eff<Either<TLeft, TRight>> eitherValueEff,
        Func<TRight, ValueTask<Unit>> f)
    {
        return
            eitherValueEff
                .Bind(eitherValue =>
                    (from eitherValue2 in EitherT.lift<TLeft, Eff, TRight>(eitherValue)
                        from b in liftEff(async () => await f(eitherValue2))
                        select eitherValue2)
                    .Run()
                );
    }
}