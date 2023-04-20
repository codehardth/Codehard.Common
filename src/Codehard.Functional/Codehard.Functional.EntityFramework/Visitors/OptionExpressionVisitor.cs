using System.Collections;
using System.Linq.Expressions;
using System.Reflection;
using LanguageExt;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query;

namespace Codehard.Functional.EntityFramework.Visitors;

public class OptionExpressionVisitor : ExpressionVisitor
{
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
        var methodName = node.Method.Name;

        if (node.Method.ReflectedType == typeof(StringOptionDbFunctionsExtensions))
        {
            var expressions =
                node.Arguments
                    .Where(a => a.Type != typeof(DbFunctions))
                    .Select(this.Visit);

            var first = expressions.First();
            var remaining = expressions.Skip(1);


            return Expression.Call(first, methodName, null, remaining.ToArray());
        }

        return methodName switch
        {
            "Select" => TranslateLinqSelect(),
            "Include" => TranslateLinqInclude(),
            "OrderBy" or "OrderByDescending" => TranslateLinqOrderBy(),
            _ => base.VisitMethodCall(node),
        };

        Expression TranslateLinqSelect()
        {
            var isOption = node.Method.GetGenericArguments().Any(IsOptionType);

            if (!isOption)
            {
                return base.VisitMethodCall(node);
            }

            var expr = Translate(baseMethod =>
            {
                var parameters = baseMethod.GetParameters();

                // param 0: IQueryable
                // param 1: a selector
                // -- Expression<Func<TSource, TResult>>
                // -- Expression<Func<TSource, int, TResult>>
                var selector = parameters[1];

                var methods =
                    node.Method.DeclaringType.GetMethods()
                        .Where(m => m.Name == methodName);

                var method = methods.SingleOrDefault(m =>
                    ReadSelectorGenericTypeArguments(m.GetParameters()[1]).Length ==
                    ReadSelectorGenericTypeArguments(selector).Length);

                return method;

                static Type[] ReadSelectorGenericTypeArguments(ParameterInfo pi)
                {
                    return pi.ParameterType.GenericTypeArguments[0].GenericTypeArguments;
                }
            });

            return expr;
        }

        Expression TranslateLinqOrderBy()
        {
            var isOption = node.Method.GetGenericArguments().Any(IsOptionType);

            if (!isOption)
            {
                return base.VisitMethodCall(node);
            }

            var expr = Translate(baseMethod =>
            {
                var parameters = baseMethod.GetParameters();

                // param 0: IQueryable
                // param 1: a selector
                // -- Expression<Func<TSource, TResult>>
                // -- Expression<Func<TSource, Option<TResult>>>
                var selector = parameters[1];

                var methods =
                    node.Method.DeclaringType.GetMethods()
                        .Where(m => m.Name == methodName);

                var method = methods.SingleOrDefault(m =>
                    ReadSelectorGenericTypeArguments(m.GetParameters()[1]).Length ==
                    ReadSelectorGenericTypeArguments(selector).Length &&
                    m.GetParameters().Length == parameters.Length);

                return method;

                static Type[] ReadSelectorGenericTypeArguments(ParameterInfo pi)
                {
                    return pi.ParameterType.GenericTypeArguments[0].GenericTypeArguments;
                }
            });

            return expr;
        }

        Expression TranslateLinqInclude()
        {
            var isOption = node.Method.GetGenericArguments().Any(IsOptionType);

            if (!isOption)
            {
                return base.VisitMethodCall(node);
            }

            var expr = Translate(baseMethod =>
            {
                return node.Method.DeclaringType.GetMethods()
                    .Where(m => m.Name == methodName)
                    .SingleOrDefault(m =>
                        m.ReturnType.GenericTypeArguments.Length ==
                        baseMethod.ReturnType.GenericTypeArguments.Length);
            });

            return expr;
        }

        // Keeping this for now, as a reference to implement a `ThenInclude`.
        Expression TranslateLinqThenInclude()
        {
            var isOption = node.Method.GetGenericArguments().Any(IsOptionType);

            if (!isOption)
            {
                return base.VisitMethodCall(node);
            }

            var expr = Translate(static oldMethod =>
            {
                var newMethod = typeof(EntityFrameworkQueryableExtensions)
                    .GetMethods()
                    .Where(m => m.Name == oldMethod.Name)
                    .Single(m =>
                    {
                        var methodParams = m.GetParameters();
                        var includeQueryableType = methodParams[0].ParameterType;
                        var args = includeQueryableType.GetGenericArguments();

                        var isEnumerableOverload = args[1].IsGenericType &&
                                                   args[1].GetGenericTypeDefinition() == typeof(IEnumerable<>);
                        var condition =
                            methodParams.Length == 2 &&
                            includeQueryableType.IsGenericType &&
                            includeQueryableType.GetGenericTypeDefinition() == typeof(IIncludableQueryable<,>) &&
                            args.Length == 2 &&
                            !isEnumerableOverload;

                        return condition;
                    });

                return newMethod;
            });

            return expr;
        }

        Expression Translate(Func<MethodInfo, MethodInfo> getNewMethod)
        {
            var baseMethod = node.Method;
            var genericMethod = getNewMethod(baseMethod) ??
                                throw new Exception(
                                    $"Unable to translate {node} during 'VisitMethodCall' translation.");

            var genericArgs =
                node.Method.GetGenericArguments()
                    .Select(t => IsOptionType(t) ? t.GenericTypeArguments[0] : t)
                    .Select(MakeNullableType)
                    .ToArray();
            var method = genericMethod.MakeGenericMethod(genericArgs);

            var transformedArguments = this.Visit(node.Arguments);
            var anotherExpressionCall = transformedArguments[0];
            var translatedOptionUnary = transformedArguments[1];
            var callExpression = Expression.Call(null, method, anotherExpressionCall, translatedOptionUnary);

            return callExpression;
        }
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

        if (exprNode is not MemberExpression memberExpression)
        {
            return base.VisitMember(node);
        }

        var genericType = memberExpression.Type.GenericTypeArguments[0];

        var memberName = member.Name;
        var left = memberExpression.Member;
        var param = memberExpression.Expression;

        var backingField =
            Expression.Field(param, ConfigurationCache.BackingField[(left.DeclaringType?.FullName, left.Name)]);

        return memberName switch
        {
            "IsSome" => Expression.NotEqual(backingField, GetNullConstant(genericType)),
            "IsNone" => Expression.Equal(backingField, GetNullConstant(genericType)),
            _ => backingField,
        };

        static ConstantExpression GetNullConstant(Type type)
        {
            return Expression.Constant(null, MakeNullableType(type));
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

        var constant = Expression.Constant(value, MakeNullableType(genericType));

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

    /// <summary>Visits the children of the <see cref="T:System.Linq.Expressions.Expression`1" />.</summary>
    /// <param name="node">The expression to visit.</param>
    /// <typeparam name="T">The type of the delegate.</typeparam>
    /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
    protected override Expression VisitLambda<T>(Expression<T> node)
    {
        if (TryGetReturnTypeFromFunc(node, out var retType) &&
            TryGetGenericOfOptionType(retType, out _))
        {
            if (node.Body is not MemberExpression memberExpression)
            {
                return base.VisitLambda(node);
            }

            var paramExpression = node.Parameters[0];
            var key = (paramExpression.Type.FullName, memberExpression.Member.Name);
            var fieldAccessExpression = Expression.Field(paramExpression, ConfigurationCache.BackingField[key]);
            var lambda = Expression.Lambda(fieldAccessExpression, paramExpression);

            return lambda;
        }

        return base.VisitLambda(node);
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

        return Expression.Parameter(MakeNullableType(genericType), node.Name);
    }

    private static Type MakeNullableType(Type type)
        => type.IsPrimitive ? typeof(Nullable<>).MakeGenericType(type) : type;

    private static bool IsOptionType(Type type)
        => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Option<>);

    private static bool IsOptionType(Expression expression)
        => IsOptionType(expression.Type);

    private static bool TryGetGenericOfOptionType(Type type, out Type genericType)
    {
        var isOption = IsOptionType(type);

        genericType = isOption ? type.GenericTypeArguments[0] : null;

        return isOption;
    }

    private static bool TryGetReturnTypeFromFunc(Expression expression, out Type returnType)
    {
        var expressionType = expression.Type;

        var isFunc = expression.Type.IsGenericType &&
                     expression.Type.GetGenericTypeDefinition() == typeof(Func<,>);

        returnType = isFunc ? expressionType.GenericTypeArguments[1] : null;

        return isFunc;
    }
}