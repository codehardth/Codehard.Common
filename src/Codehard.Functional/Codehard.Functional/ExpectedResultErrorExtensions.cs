using LanguageExt.Common;

namespace Codehard.Functional;

public static class ExpectedResultErrorExtensions
{
    public static Aff<TResult> MapExpectedResultError<TResult>(
        this Error error)
    {
        return
            error is ExpectedResultError { ErrorObject: TResult } expected
                ? SuccessAff((TResult)expected.ErrorObject)
                : FailAff<TResult>(error);
    }
    
    public static Aff<TResult> MapExpectedResultError<TResult>(
        this Aff<TResult> aff)
    {
        return
            aff.MatchAff(
                Succ: SuccessAff,
                Fail: err => err.MapExpectedResultError<TResult>());
    }
}