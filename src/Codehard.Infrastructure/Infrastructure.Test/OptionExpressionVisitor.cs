using System.Linq.Expressions;
using System.Reflection;
using LanguageExt;
using LanguageExt.UnsafeValueAccess;

namespace Infrastructure.Test;

public class OptionExpressionVisitor : ExpressionVisitor
{
    private Func<Expression, string> getExpressionName;

    public OptionExpressionVisitor()
    {
        var propertyInfo = typeof(Expression).GetProperty("DebugView", BindingFlags.Instance | BindingFlags.NonPublic);

        this.getExpressionName = expr => ((string)propertyInfo!.GetValue(expr))[1..];
    }

    protected override Expression VisitMember(MemberExpression node)
    {
        var isOptionMember = node.Expression!.Type.IsGenericType && node.Expression!.Type.GetGenericTypeDefinition() == typeof(Option<>);

        if (!isOptionMember)
        {
            return base.VisitMember(node);
        }

        var genericType = node.Expression.Type.GenericTypeArguments[0];
        var isPrimitiveType = genericType.IsPrimitive;

        var memberName = node.Member.Name;

        var leftName = this.getExpressionName(node.Expression);
        var param = (ParameterExpression)GetParameterExpression(node.Expression);
        var left =
            Expression.Property(param, GetBackingField(param, leftName));

        var right =
            isPrimitiveType ? Expression.Constant(null, typeof(Nullable<>).MakeGenericType(genericType)) : Expression.Constant(null, genericType);

        // Expression.Default(node.Expression.Type) is not a correct solution
        // as it is a default(T), in case of primitive type it will yield an incorrect behavior
        Expression expr = memberName switch
        {
            "IsSome" => Expression.NotEqual(left, right),
            "IsNone" => Expression.Equal(left, right),
            _ => throw new NotSupportedException(),
        };

        return expr;
    }

    private PropertyInfo GetBackingField(ParameterExpression param, string expressionName)
    {
        // Oops
        var props = param.Type.GetRuntimeProperties();

        var name = expressionName.Replace($"{param.Name}.", "").ToLowerInvariant();

        var propertyInfos = props.Where(p => p.Name.ToLowerInvariant().Contains(name) && p.CanWrite);

        return propertyInfos.FirstOrDefault() ?? throw new Exception();
    }

    private Expression GetParameterExpression(Expression expression)
    {
        // Yet another oops.
        if (expression is ParameterExpression paramExpr)
        {
            return paramExpr;
        }

        return expression switch
        {
            MemberExpression memberExpression => GetParameterExpression(memberExpression.Expression!),
            _ => throw new NotSupportedException(),
        };
    }
}