namespace Codehard.Functional.AspNetCore;

public static class Commons
{
    public static Error Flatten(this Seq<Error> errors, int errorCode, char sep = '\n')
        => Error.New(errorCode, string.Concat(sep, errors.Map(e => e.Message)));

    internal static Aff<A> MapFailToHttpResultError<A>(
        this Aff<A> ma,
        HttpStatusCode code,
        Option<string> errorCode = default,
        Option<string> messageOpt = default,
        Option<object> data = default,
        bool @override = true)
        => ma.MapFail(err =>
        {
            var message = messageOpt.IfNone(string.Empty);
            
            return
                err switch
                {
                    HttpResultError when @override => 
                        HttpResultError.New(
                            code, message, errorCode, data, err),
                    HttpResultError hre when !@override => hre,
                    _ => 
                        HttpResultError.New(
                            code, message, errorCode, data, err),
                };
        });
            
    
    internal static Aff<A> MapFailToHttpResultError<A>(
        this Aff<A> ma,
        HttpStatusCode code,
        Func<Error, string> messageFunc,
        Option<string> errorCode = default,
        Option<object> data = default,
        bool @override = true)
        => ma.MapFail(err =>
        {
            var message = 
                Some(messageFunc)
                    .Map(f => f(err))
                    .IfNone(string.Empty);
            
            return
                err switch
                {
                    HttpResultError when @override =>
                        HttpResultError.New(
                            code, message, errorCode, data, err),
                    HttpResultError hre when !@override => hre,
                    _ =>
                        HttpResultError.New(
                            code, message, errorCode, data, err),
                };
        });
    
    internal static Aff<A> MapToHttpResultError<A>(this Aff<A> ma, int code, Func<Error, string> messageFunc)
        => ma.MapFail(err =>
            err.Exception.Match(
                Some: ex => Error.New(code, messageFunc(err), ex),
                None: () => Error.New(code, messageFunc(err), err)));

    internal static Eff<A> MapToHttpResultError<A>(this Eff<A> ma, int code, string message)
        => ma.MapFail(err =>
            err.Exception.Match(
                Some: ex => Error.New(code, message, ex),
                None: () => Error.New(code, message, err)));
    
    internal static Eff<A> MapToHttpResultError<A>(this Eff<A> ma, int code, Func<Error, string> messageFunc)
        => ma.MapFail(err =>
            err.Exception.Match(
                Some: ex => Error.New(code, messageFunc(err), ex),
                None: () => Error.New(code, messageFunc(err), err)));
}