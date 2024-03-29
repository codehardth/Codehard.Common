﻿namespace Codehard.Functional.AspNetCore;

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
            return
                err switch
                {
                    HttpResultError hre when !@override => hre,
                    _ => 
                        HttpResultError.New(
                            code,
                            messageFunc(err),
                            errorCode,
                            data,
                            err),
                };
        });
    
    internal static Eff<A> MapFailToHttpResultError<A>(
        this Eff<A> ma,
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
                    HttpResultError hre when !@override => hre,
                    _ => 
                        HttpResultError.New(
                            code, message, errorCode, data, err),
                };
        });
    
    internal static Eff<A> MapFailToHttpResultError<A>(
        this Eff<A> ma,
        HttpStatusCode code,
        Func<Error, string> messageFunc,
        Option<string> errorCode = default,
        Option<object> data = default,
        bool @override = true)
        => ma.MapFail(err =>
        {
            return
                err switch
                {
                    HttpResultError hre when !@override => hre,
                    _ => 
                        HttpResultError.New(
                            code,
                            messageFunc(err),
                            errorCode,
                            data,
                            err),
                };
        });
}