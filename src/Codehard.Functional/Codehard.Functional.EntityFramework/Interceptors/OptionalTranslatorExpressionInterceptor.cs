using System.Linq.Expressions;
using Codehard.Functional.EntityFramework.Visitors;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Codehard.Functional.EntityFramework.Interceptors;

public sealed class OptionalTranslatorExpressionInterceptor : IQueryExpressionInterceptor
{
    Expression IQueryExpressionInterceptor.QueryCompilationStarting(
        Expression queryExpression,
        QueryExpressionEventData eventData)
    {
        var nodeType = queryExpression.Type;

        var entityType =
            nodeType.IsGenericType && nodeType.GetGenericTypeDefinition() == typeof(IQueryable<>)
                ? nodeType.GenericTypeArguments[0]
                : nodeType;

        var exprVisitor = new OptionExpressionVisitor(entityType);

        var translatedExpression = exprVisitor.Visit(queryExpression)!;

        return translatedExpression;
    }
}