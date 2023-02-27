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

    /// <summary>Visits the children of the <see cref="T:System.Linq.Expressions.BinaryExpression" />.</summary>
    /// <param name="node">The expression to visit.</param>
    /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
    protected override Expression VisitBinary(BinaryExpression node)
    {
        var isLeftOpt = IsOptionType(node.Left);
        var isRightOpt = IsOptionType(node.Right);

        if (!(isLeftOpt && isRightOpt && node.Left.Type == node.Right.Type))
        {
            return base.VisitBinary(node);
        }

        var left = this.Visit(node.Left);
        var right = this.Visit(node.Right);

        var genericType = node.Left.Type.GenericTypeArguments[0];

        if (genericType.IsPrimitive)
        {
            var methodName = node.Method!.Name.Replace("op_", "");
            var method = genericType.GetMethod(methodName);

            return Expression.MakeBinary(node.NodeType, left, right, node.IsLiftedToNull, method);
        }

        return node.NodeType switch
        {
            ExpressionType.Equal => Expression.ReferenceEqual(left, right),
            ExpressionType.NotEqual => Expression.ReferenceNotEqual(left, right),
            _ => base.VisitBinary(node),
        };
    }

    /// <summary>Visits the children of the <see cref="T:System.Linq.Expressions.MethodCallExpression" />.</summary>
    /// <param name="node">The expression to visit.</param>
    /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
    protected override Expression VisitMethodCall(MethodCallExpression node)
    {
        if (node.Method.ReflectedType == typeof(StringOptionExtensions))
        {
            var expressions =
                node.Arguments.Select(this.Visit);

            var first = expressions.First();
            var remaining = expressions.Skip(1);

            var methodName = node.Method.Name;

            return Expression.Call(first, methodName, null, remaining.ToArray());
        }

        return base.VisitMethodCall(node);
    }

    protected override Expression VisitMember(MemberExpression node)
    {
        Expression exprNode = node;
        var member = node.Member;

        if (!IsOptionType(exprNode))
        {
            exprNode = node.Expression;
        }

        if (!IsOptionType(exprNode))
        {
            return base.VisitMember(node);
        }

        var genericType = exprNode.Type.GenericTypeArguments[0];

        var memberName = member.Name;
        var leftName = this.getExpressionName(exprNode);
        var param = (ParameterExpression)GetParameterExpression(exprNode);

        var backingField = Expression.Property(param, GetBackingField(param, leftName));

        // Expression.Default(node.Expression.Type) is not a correct solution
        // as it is a default(T), in case of primitive type it will yield an incorrect behavior
        Expression expr = memberName switch
        {
            "IsSome" => Expression.NotEqual(backingField, GetNullConstant(genericType)),
            "IsNone" => Expression.Equal(backingField, GetNullConstant(genericType)),
            _ => backingField,
        };

        return expr;

        static ConstantExpression GetNullConstant(Type type)
        {
            return type.IsPrimitive
                ? Expression.Constant(null, typeof(Nullable<>).MakeGenericType(type))
                : Expression.Constant(null, type);
        }
    }

    /// <summary>Visits the <see cref="T:System.Linq.Expressions.ConstantExpression" />.</summary>
    /// <param name="node">The expression to visit.</param>
    /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
    protected override Expression VisitConstant(ConstantExpression node)
    {
        if (!IsOptionType(node))
        {
            return base.VisitConstant(node);
        }

        var genericType = node.Type.GenericTypeArguments[0];
        object value = genericType switch
        {
            _ when genericType == typeof(bool) => ((Option<bool>)node.Value!).ValueUnsafe(),
            _ when genericType == typeof(int) => ((Option<int>)node.Value!).ValueUnsafe(),
            _ when genericType == typeof(float) => ((Option<float>)node.Value!).ValueUnsafe(),
            _ when genericType == typeof(double) => ((Option<double>)node.Value!).ValueUnsafe(),
            _ when genericType == typeof(decimal) => ((Option<decimal>)node.Value!).ValueUnsafe(),
            _ when genericType == typeof(Guid) => ((Option<Guid>)node.Value!).ValueUnsafe(),
            _ when genericType == typeof(string) => ((Option<string>)node.Value!).ValueUnsafe(),
            _ => throw new NotSupportedException($"Type '{genericType}' is not supported in this context."),
        };

        var type =
            genericType.IsPrimitive ? typeof(Nullable<>).MakeGenericType(genericType) : genericType;

        var constant = Expression.Constant(value, type);

        return constant;
    }

    private static ParameterExpression GetParameterExpression(Expression expression)
    {
        // This will need more intensive tests.
        return expression switch
        {
            ParameterExpression parameterExpression => parameterExpression,
            MemberExpression memberExpression => GetParameterExpression(memberExpression.Expression!),
            _ => throw new NotSupportedException(),
        };
    }

    private static PropertyInfo GetBackingField(ParameterExpression param, string expressionName)
    {
        // Oops
        var props = param.Type.GetRuntimeProperties();

        var name = expressionName.Replace($"{param.Name}.", "").ToLowerInvariant();

        var propertyInfos = props.Where(p => p.Name.ToLowerInvariant().Contains(name) && p.CanWrite);

        return propertyInfos.FirstOrDefault() ?? throw new Exception();
    }

    private static bool IsOptionType(Expression expression)
        => expression.Type.IsGenericType &&
           expression.Type.GetGenericTypeDefinition() == typeof(Option<>);
}