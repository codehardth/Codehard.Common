using LanguageExt;
using LanguageExt.Common;
using Microsoft.FSharp.Core;
using static LanguageExt.Prelude;

namespace Codehard.Functional.FSharp
{
    /// <summary>
    /// F# result extensions
    /// </summary>
    public static class ResultExtensions
    {
        /// <summary>
        /// Convert F# Result to Fin
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TError"></typeparam>
        /// <param name="result">The F# result value</param>
        /// <param name="errorMapper">Map <typeparamref name="TError"/> -> <see cref="Error"/></param>
        /// <returns></returns>
        public static Fin<T> ToFin<T, TError>(
            this FSharpResult<T, TError> result,
            Func<TError, Error> errorMapper)
        {
            return
                result.IsError
                    ? FinFail<T>(errorMapper(result.ErrorValue))
                    : FinSucc(result.ResultValue);
        }

        /// <summary>
        /// Convert F# Result to Aff
        /// </summary>
        public static Aff<T> ToAff<T, TError>(
            this FSharpResult<T, TError> result,
            Func<TError, Error> errorMapper)
        {
            return
                result.IsError
                    ? FailAff<T>(errorMapper(result.ErrorValue))
                    : SuccessAff(result.ResultValue);
        }

        /// <summary>
        /// Convert F# Result to Eff
        /// </summary>
        public static Eff<T> ToEff<T, TError>(
            this FSharpResult<T, TError> result,
            Func<TError, Error> errorMapper)
        {
            return
                result.IsError
                    ? FailEff<T>(errorMapper(result.ErrorValue))
                    : SuccessEff(result.ResultValue);
        }

        /// <summary>
        /// Convert Task of F# Result to Aff
        /// </summary>
        public static Aff<T> ToAff<T, TError>(
            this Task<FSharpResult<T, TError>> resultTask,
            Func<TError, Error> errorMapper)
        {
            return
                Aff(async () => await resultTask)
                .Bind(result =>
                    result.IsError
                        ? FailAff<T>(errorMapper(result.ErrorValue))
                        : SuccessAff(result.ResultValue));
        }
    }
}