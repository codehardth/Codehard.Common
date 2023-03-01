using System.Linq.Expressions;
using Codehard.Functional.EntityFramework.Visitors;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Codehard.Functional.EntityFramework.Interceptors;

public sealed class OptionalTranslatorExpressionInterceptor : IQueryExpressionInterceptor
{
    private static OptionExpressionVisitor Visitor = new();

    Expression IQueryExpressionInterceptor.QueryCompilationStarting(
        Expression queryExpression,
        QueryExpressionEventData eventData)
    {
        var translatedExpression = Visitor.Visit(queryExpression)!;

        return translatedExpression;
    }
}