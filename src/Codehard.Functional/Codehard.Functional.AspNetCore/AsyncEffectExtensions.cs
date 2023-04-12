namespace Codehard.Functional.AspNetCore;

public static partial class AsyncEffectExtensions
{
    public static Aff<A> GuardWithHttpStatus<A>(
        this Aff<A> ma, Func<A, bool> predicate, HttpStatusCode httpStatusCode, string message = "")
        => ma.Guard(predicate, Error.New((int)httpStatusCode, message));

    public static Aff<A> GuardWithHttpStatus<A>(
        this Aff<A> ma, Func<A, bool> predicate, HttpStatusCode httpStatusCode, Func<A, string> messageFunc)
        => ma.Guard(predicate, a => Error.New((int)httpStatusCode, messageFunc(a)));
}