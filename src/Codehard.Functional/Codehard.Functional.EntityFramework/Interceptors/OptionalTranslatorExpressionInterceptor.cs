using System.Collections;
using System.Data.Common;
using System.Linq.Expressions;
using Codehard.Functional.EntityFramework.Visitors;
using LanguageExt;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Codehard.Functional.EntityFramework.Interceptors;

public sealed class OptionalTranslatorExpressionInterceptor : IQueryExpressionInterceptor
{
    Expression IQueryExpressionInterceptor.QueryCompilationStarting(
        Expression queryExpression,
        QueryExpressionEventData eventData)
    {
        var exprVisitor = new OptionExpressionVisitor();

        return exprVisitor.Visit(queryExpression)!;
    }
}

public sealed class OptionalQueryInterceptor : DbCommandInterceptor
{
    /// <summary>
    ///     Called just before EF intends to call <see cref="M:System.Data.Common.DbCommand.ExecuteReader" />.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <param name="eventData">Contextual information about the command and execution.</param>
    /// <param name="result">
    ///     Represents the current result if one exists.
    ///     This value will have <see cref="P:Microsoft.EntityFrameworkCore.Diagnostics.InterceptionResult`1.HasResult" /> set to <see langword="true" /> if some previous
    ///     interceptor suppressed execution by calling <see cref="M:Microsoft.EntityFrameworkCore.Diagnostics.InterceptionResult`1.SuppressWithResult(`0)" />.
    ///     This value is typically used as the return value for the implementation of this method.
    /// </param>
    /// <returns>
    ///     If <see cref="P:Microsoft.EntityFrameworkCore.Diagnostics.InterceptionResult`1.HasResult" /> is false, the EF will continue as normal.
    ///     If <see cref="P:Microsoft.EntityFrameworkCore.Diagnostics.InterceptionResult`1.HasResult" /> is true, then EF will suppress the operation it
    ///     was about to perform and use <see cref="P:Microsoft.EntityFrameworkCore.Diagnostics.InterceptionResult`1.Result" /> instead.
    ///     An implementation of this method for any interceptor that is not attempting to change the result
    ///     is to return the <paramref name="result" /> value passed in.
    /// </returns>
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