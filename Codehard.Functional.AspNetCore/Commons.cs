namespace Codehard.Functional.AspNetCore;

public static class Commons
{
    public static Error Flatten(this Seq<Error> errors, int errorCode, char sep = '\n')
        => Error.New(errorCode, string.Concat(sep, errors.Map(e => e.Message)));

    internal static Aff<A> CustomError<A>(this Aff<A> ma, int code, Func<Error, string> messageFunc)
        => ma.MapFail(err =>
            err.Exception.Match(
                Some: ex => Error.New(code, messageFunc(err), ex),
                None: () => Error.New(code, messageFunc(err), err)));

    internal static Eff<A> CustomError<A>(this Eff<A> ma, int code, Func<Error, string> messageFunc)
        => ma.MapFail(err =>
            err.Exception.Match(
                Some: ex => Error.New(code, messageFunc(err), ex),
                None: () => Error.New(code, messageFunc(err), err)));
}