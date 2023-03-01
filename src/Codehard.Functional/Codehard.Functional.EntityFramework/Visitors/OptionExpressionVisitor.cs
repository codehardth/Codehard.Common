using System.Collections;
using System.Linq.Expressions;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace Codehard.Functional.EntityFramework.Visitors;

public class OptionExpressionVisitor : ExpressionVisitor
{
    private Type? entityType;

    public OptionExpressionVisitor(Type entityType)
    {
        this.entityType = entityType;
    }

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

        if (genericType.IsPrimitive || genericType.IsValueType)
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

    protected override Expression VisitMethodCall(MethodCallExpression node)
    {
        if (node.Method.ReflectedType == typeof(StringOptionDbFunctionsExtensions))
        {
            var expressions =
                node.Arguments
                    .Where(a => a.Type != typeof(DbFunctions))
                    .Select(this.Visit);

            var first = expressions.First();
            var remaining = expressions.Skip(1);

            var methodName = node.Method.Name;

            return Expression.Call(first, methodName, null, remaining.ToArray());
        }

        return base.VisitMethodCall(node);
    }

    protected override Expression VisitMember(MemberExpression node)
    {
        Expression? exprNode = node;
        var member = node.Member;

        if (!IsOptionType(exprNode))
        {
            exprNode = node.Expression;
        }

        if (exprNode == null || !IsOptionType(exprNode))
        {
            return base.VisitMember(node);
        }

        var genericType = exprNode.Type.GenericTypeArguments[0];

        var memberName = member.Name;
        var left = ((MemberExpression)exprNode).Member;
        var param = GetParameterExpression(exprNode);

        var backingField = Expression.Field(param, ConfigurationCache.BackingField[(this.entityType!, left.Name)]);

        return memberName switch
        {
            "IsSome" => Expression.NotEqual(backingField, GetNullConstant(genericType)),
            "IsNone" => Expression.Equal(backingField, GetNullConstant(genericType)),
            _ => backingField,
        };

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
        var enumerable = (IEnumerable)node.Value!;
        var enumerator = enumerable.GetEnumerator();
        var hasValue = enumerator.MoveNext();

        var value = hasValue ? enumerator.Current : null;

        var type =
            genericType.IsPrimitive ? typeof(Nullable<>).MakeGenericType(genericType) : genericType;

        var constant = Expression.Constant(value, type);

        return constant;
    }

    /// <summary>Visits the children of the <see cref="T:System.Linq.Expressions.UnaryExpression" />.</summary>
    /// <param name="node">The expression to visit.</param>
    /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
    protected override Expression VisitUnary(UnaryExpression node)
    {
        if (!IsOptionType(node))
        {
            return base.VisitUnary(node);
        }

        var operand = node.Operand;

        return node.NodeType switch
        {
            ExpressionType.Convert =>
                operand.Type.IsPrimitive || operand.Type.IsValueType
                    ? Expression.Convert(operand, typeof(Nullable<>).MakeGenericType(operand.Type))
                    : operand,
            _ => throw new Exception($"{node.NodeType} is not supported in this context."),
        };
    }

    /// <summary>Visits the <see cref="T:System.Linq.Expressions.ParameterExpression" />.</summary>
    /// <param name="node">The expression to visit.</param>
    /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
    protected override Expression VisitParameter(ParameterExpression node)
    {
        if (!IsOptionType(node))
        {
            return base.VisitParameter(node);
        }

        var genericType = node.Type.GenericTypeArguments[0];
        var type =
            genericType.IsPrimitive ? typeof(Nullable<>).MakeGenericType(genericType) : genericType;

        return Expression.Parameter(type, node.Name);
    }

    private static ParameterExpression GetParameterExpression(Expression expression)
    {
        return expression switch
        {
            ParameterExpression parameterExpression => parameterExpression,
            MemberExpression memberExpression => GetParameterExpression(memberExpression.Expression!),
            _ => throw new NotSupportedException(),
        };
    }

    private static bool IsOptionType(Expression expression)
        => expression.Type.IsGenericType &&
           expression.Type.GetGenericTypeDefinition() == typeof(Option<>);
}