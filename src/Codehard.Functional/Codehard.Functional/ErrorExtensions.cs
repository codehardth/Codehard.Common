#pragma warning disable CS1591

using Codehard.Functional;
using static LanguageExt.Prelude;

// ReSharper disable once CheckNamespace
namespace LanguageExt.Common;

public static class ErrorExtensions
{
    public static Aff<TResult> MapExpectedResultError<TResult>(
        this Error error)
    {
        return
            error is ExpectedResultError { ErrorObject: TResult } expected
                ? SuccessAff((TResult)expected.ErrorObject)
                : FailAff<TResult>(error);
    }
}