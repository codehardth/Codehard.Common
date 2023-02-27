using System.Linq.Expressions;
using Codehard.Functional.EntityFramework.Visitors;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Codehard.Functional.EntityFramework.Interceptors;

public sealed class OptionalTranslatorExpressionInterceptor : IQueryExpressionInterceptor
{
    Expression IQueryExpressionInterceptor.QueryCompilationStarting(
        Expression queryExpression,
        QueryExpressionEventData eventData) =>
        new OptionExpressionVisitor().Visit(queryExpression)!;
}