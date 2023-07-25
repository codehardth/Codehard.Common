#pragma warning disable CS1591
namespace Codehard.Functional;

public static class AffEitherExtensions
{
    public static Eff<Either<TLeft, T>> MapNoneToLeft<T, TLeft>(
        this Eff<Option<T>> optEff,
        Func<TLeft> leftF)
    {
        return 
            optEff.Map(
                valOpt =>
                    valOpt.ToEither(leftF));
    }
    
    public static Aff<Either<TLeft, T>> MapNoneToLeft<T, TLeft>(
        this Aff<Option<T>> optAff,
        Func<TLeft> leftF)
    {
        return 
            optAff.Map(
                optVal =>
                    optVal.ToEither(leftF));
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
    
    public static Aff<Either<TLeft, T>> GuardToLeft<T, TLeft>(
        this Aff<T> aff,
        Func<T, bool> predicate,
        Func<TLeft> leftF)
    {
        return
            aff.Map(
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
    
    public static Aff<Either<TLeft, T>> GuardEither<T, TLeft>(
        this Aff<Either<TLeft, T>> eitherValueAff,
        Func<T, bool> predicate,
        Func<TLeft> leftF)
    {
        return
            eitherValueAff.Map(
                eitherValue =>
                    eitherValue.Bind(
                        val =>
                            predicate(val)
                                ? Right<TLeft, T>(val)
                                : Left<TLeft, T>(leftF())));
    }
    
    public static Aff<Either<TLeft, T>> GuardEitherAsync<T, TLeft>(
        this Aff<Either<TLeft, T>> eitherValueAff,
        Func<T, ValueTask<bool>> predicateAsync,
        Func<T, TLeft> leftF)
    {
        return
            eitherValueAff.MapAsync(
                async eitherValue =>
                    await eitherValue.BindAsync(
                        async val =>
                            await predicateAsync(val)
                                ? Right<TLeft, T>(val)
                                : Left<TLeft, T>(leftF(val))));
    }
    
    public static Aff<Either<TLeft, T>> GuardEitherAsync<T, TLeft>(
        this Aff<Either<TLeft, T>> eitherValueAff,
        Func<T, bool> predicate,
        Func<T, ValueTask<TLeft>> leftAsync)
    {
        return
            eitherValueAff.MapAsync(
                async eitherValue =>
                    await eitherValue.BindAsync(
                        async val =>
                            predicate(val)
                                ? Right<TLeft, T>(val)
                                : Left<TLeft, T>(await leftAsync(val))));
    }
    
    public static Aff<Either<TLeft, T>> GuardEitherAsync<T, TLeft>(
        this Aff<Either<TLeft, T>> eitherValueAff,
        Func<T, ValueTask<bool>> predicateAsync,
        Func<T, ValueTask<TLeft>> leftAsync)
    {
        return
            eitherValueAff.MapAsync(
                async eitherValue =>
                    await eitherValue.BindAsync(
                        async val =>
                            await predicateAsync(val)
                                ? Right<TLeft, T>(val)
                                : Left<TLeft, T>(await leftAsync(val))));
    }
    
    public static Aff<Either<TLeft, TRight>> MapRight<T, TRight, TLeft>(
        this Aff<Either<TLeft, T>> eitherValueAff,
        Func<T, TRight> mapF)
    {
        return
            eitherValueAff
                .Map(eitherValue =>
                    eitherValue.Map(mapF));
    }

    public static Aff<Either<TLeft, TRight>> MapRightAsync<T, TRight, TLeft>(
        this Eff<Either<TLeft, T>> eitherValueEff,
        Func<T, ValueTask<TRight>> mapAsync)
    {
        return
            eitherValueEff
                .MapAsync(
                    async eitherValue =>
                        await eitherValue
                            .MapAsync(
                                async val =>
                                    await mapAsync(val)));
    }
    
    public static Aff<Either<TLeft, TRight>> MapRightAsync<T, TRight, TLeft>(
        this Aff<Either<TLeft, T>> eitherValueAff,
        Func<T, ValueTask<TRight>> mapAsync)
    {
        return
            eitherValueAff
                .MapAsync(
                    async eitherValue =>
                        await eitherValue
                            .MapAsync(
                                async val =>
                                    await mapAsync(val)));
    }
    
    public static Aff<Either<TLeft, TRight>> DoIfRight<TRight, TLeft>(
        this Aff<Either<TLeft, TRight>> eitherValueAff,
        Action<TRight> f)
    {
        return
            eitherValueAff
                .Map(
                    eitherValue =>
                        eitherValue.Do(f));
    }
    
    public static Aff<Either<TLeft, TRight>> DoIfRightAsync<TRight, TLeft>(
        this Aff<Either<TLeft, TRight>> eitherValueAff,
        Func<TRight, ValueTask<Unit>> f)
    {
        return
            eitherValueAff
                .MapAsync(
                    async eitherValue =>
                        await eitherValue
                            .MapAsync(
                                async val =>
                                {
                                    await f(val);
                                    return val;
                                }));
    }
}