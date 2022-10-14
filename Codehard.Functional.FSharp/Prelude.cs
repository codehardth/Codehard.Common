using LanguageExt;
using LanguageExt.Common;
using Microsoft.FSharp.Core;
using static LanguageExt.Prelude;

namespace Codehard.Functional.FSharp
{
    public static class Prelude
    {
        /// <summary>
        /// Wrap Task of F# Result in Aff
        /// </summary>
        public static Aff<T> Aff<T, TError>(
            Func<Task<FSharpResult<T, TError>>> resultTaskF,
            Func<TError, Error> errorMapper)
        {
            return
                LanguageExt.Aff<FSharpResult<T, TError>>
                .Effect(async () => await resultTaskF())
                .Bind(result =>
                    result.IsError
                        ? FailAff<T>(errorMapper(result.ErrorValue))
                        : SuccessAff(result.ResultValue));
        }
    }
}
