#pragma warning disable CS1591

using Codehard.Functional;

// ReSharper disable once CheckNamespace
namespace LanguageExt.Common;

public static class ErrorExtensions
{
    public static Eff<TResult> MapExpectedResultError<TResult>(
        this Error error)
    {
        return
            error is ExpectedResultError { ErrorObject: TResult } expected
                ? SuccessEff((TResult)expected.ErrorObject)
                : FailEff<TResult>(error);
    }
}