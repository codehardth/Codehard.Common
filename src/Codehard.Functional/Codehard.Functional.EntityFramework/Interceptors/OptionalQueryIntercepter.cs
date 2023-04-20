using System.Collections;
using System.Data.Common;
using LanguageExt;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Codehard.Functional.EntityFramework.Interceptors;

public sealed class OptionalQueryInterceptor : DbCommandInterceptor
{
    public override InterceptionResult<DbDataReader> ReaderExecuting(
        DbCommand command,
        CommandEventData eventData,
        InterceptionResult<DbDataReader> result)
    {
        foreach (DbParameter param in command.Parameters)
        {
            var type = param.Value?.GetType();

            if (type is not { IsGenericType: true })
            {
                continue;
            }

            if (type.GetGenericTypeDefinition() == typeof(Option<>))
            {
                var enumerable = (IEnumerable)param.Value!;
                var enumerator = enumerable.GetEnumerator();
                var hasValue = enumerator.MoveNext();

                param.Value = hasValue ? enumerator.Current : DBNull.Value;
            }
        }

        return base.ReaderExecuting(command, eventData, result);
    }
}