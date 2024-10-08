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
        this Eff<Either<TLeft, T>> eitherValueAff,
        Func<T, ValueTask<bool>> predicateAsync,
        Func<T, TLeft> leftF)
    {
        return
            from eitherValue in eitherValueAff
            from res in
                liftEff(() =>
                    eitherValue.BindAsync(async val =>
                        await predicateAsync(val)
                            ? Right<TLeft, T>(val)
                            : Left<TLeft, T>(leftF(val))))
            select res;
    }
    
    public static Eff<Either<TLeft, T>> GuardEitherAsync<T, TLeft>(
        this Eff<Either<TLeft, T>> eitherValueAff,
        Func<T, bool> predicate,
        Func<T, ValueTask<TLeft>> leftAsync)
    {
        return
            from eitherValue in eitherValueAff
            from res in 
                liftEff(() =>
                    eitherValue.BindAsync(async val =>
                        predicate(val)
                            ? Right<TLeft, T>(val)
                            : Left<TLeft, T>(await leftAsync(val))))
            select res;
    }
    
    public static Eff<Either<TLeft, T>> GuardEitherAsync<T, TLeft>(
        this Eff<Either<TLeft, T>> eitherValueAff,
        Func<T, ValueTask<bool>> predicateAsync,
        Func<T, ValueTask<TLeft>> leftAsync)
    {
        return
            from eitherValue in eitherValueAff
            from res in
                liftEff(() =>
                    eitherValue.BindAsync(async val =>
                        await predicateAsync(val)
                            ? Right<TLeft, T>(val)
                            : Left<TLeft, T>(await leftAsync(val))))
            select res;
    }
    
    public static Eff<Either<TLeft, TRight>> MapRight<T, TRight, TLeft>(
        this Eff<Either<TLeft, T>> eitherValueAff,
        Func<T, TRight> mapF)
    {
        return
            eitherValueAff
                .Map(eitherValue =>
                    eitherValue.Map(mapF));
    }

    public static Eff<Either<TLeft, TRight>> MapRightAsync<T, TRight, TLeft>(
        this Eff<Either<TLeft, T>> eitherValueEff,
        Func<T, ValueTask<TRight>> mapAsync)
    {
        return
            from eitherValue in eitherValueEff
            from res in
                liftEff(() =>
                    eitherValue
                        .MapAsync(async val =>
                            await mapAsync(val)))
            select res;
    }
    
    public static Eff<Either<TLeft, TRight>> DoIfRight<TRight, TLeft>(
        this Eff<Either<TLeft, TRight>> eitherValueAff,
        Action<TRight> f)
    {
        return
            eitherValueAff
                .Map(eitherValue =>
                    eitherValue.Do(f));
    }
    
    public static Eff<Either<TLeft, TRight>> DoIfRightAsync<TRight, TLeft>(
        this Eff<Either<TLeft, TRight>> eitherValueAff,
        Func<TRight, ValueTask<Unit>> f)
    {
        return
            from eitherValue in eitherValueAff
            from res in
                liftEff(() =>
                    eitherValue
                        .MapAsync(async val =>
                        {
                            await f(val);
                            return val;
                        }))
            select res;
    }
}