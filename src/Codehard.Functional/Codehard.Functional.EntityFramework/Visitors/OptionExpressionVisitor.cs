using System;
using System.Linq;
using System.Linq.Expressions;
using LanguageExt;
using LanguageExt.UnsafeValueAccess;
using Microsoft.EntityFrameworkCore;

namespace Codehard.Functional.EntityFramework.Visitors;

public class OptionExpressionVisitor : ExpressionVisitor
{
    private Type? entityType;

    /// <summary>Dispatches the expression to one of the more specialized visit methods in this class.</summary>
    /// <param name="node">The expression to visit.</param>
    /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
    public override Expression? Visit(Expression? node)
    {
        if (this.entityType != null)
        {
            return base.Visit(node);
        }

        var isGenericType = node?.Type.IsGenericType ?? false;
        var isQueryable = isGenericType && node?.Type.GetGenericTypeDefinition() == typeof(IQueryable<>);

        if (!isQueryable)
        {
            return base.Visit(node);
        }

        if (node != null)
        {
            var type = node.Type.GenericTypeArguments[0];

            this.entityType = type;
        }

        return base.Visit(node);
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

    /// <summary>Visits the children of the <see cref="T:System.Linq.Expressions.MethodCallExpression" />.</summary>
    /// <param name="node">The expression to visit.</param>
    /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
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

        object value = genericType switch
        {
            _ when genericType == typeof(bool) => ((Option<bool>)node.Value!).ValueUnsafe(),
            _ when genericType == typeof(int) => ((Option<int>)node.Value!).ValueUnsafe(),
            _ when genericType == typeof(float) => ((Option<float>)node.Value!).ValueUnsafe(),
            _ when genericType == typeof(double) => ((Option<double>)node.Value!).ValueUnsafe(),
            _ when genericType == typeof(decimal) => ((Option<decimal>)node.Value!).ValueUnsafe(),
            _ when genericType == typeof(Guid) => ((Option<Guid>)node.Value!).ValueUnsafe(),
            _ when genericType == typeof(string) => ((Option<string>)node.Value!).ValueUnsafe(),
            _ when genericType == typeof(DateTime) => ((Option<DateTime>)node.Value!).ValueUnsafe(),
            _ when genericType == typeof(DateTimeOffset) => ((Option<DateTimeOffset>)node.Value!).ValueUnsafe(),
            _ when genericType == typeof(DateOnly) => ((Option<DateOnly>)node.Value!).ValueUnsafe(),
            _ when genericType == typeof(TimeOnly) => ((Option<TimeOnly>)node.Value!).ValueUnsafe(),
            _ => throw new NotSupportedException($"Type '{genericType}' is not supported in this context."),
        };

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

    private static bool IsOptionType(Expression expression)
        => expression.Type.IsGenericType &&
           expression.Type.GetGenericTypeDefinition() == typeof(Option<>);
}