namespace CodeHard.Function.FSharp.Tests.Types

module ResultType =
    type Error =
        | Error1 of string
        | Error2 of int

    let getOkResult () : Result<int, Error> =
        Ok 0

    let getErrorResult () : Result<int, Error> =
        Result.Error (Error1 "Something went wrong")

    let mapError err =
        match err with
        | Error1 msg ->
            LanguageExt.Common.Error.New msg
        | Error2 code ->
            LanguageExt.Common.Error.New(code, "Something went wrong")
