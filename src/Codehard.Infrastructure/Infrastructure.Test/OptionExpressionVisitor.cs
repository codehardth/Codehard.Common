using System.Linq.Expressions;
using LanguageExt;

namespace Infrastructure.Test;

public class OptionExpressionVisitor : ExpressionVisitor
{
    public static readonly OptionExpressionVisitor Instance = new();

    private OptionExpressionVisitor()
    {
    }

    protected override Expression VisitMember(MemberExpression node)
    {
        var memberName = node.Member.Name;

        var isOptionMember = node.Expression!.Type.GetGenericTypeDefinition() == typeof(Option<>);

        // Expression.Default(node.Expression.Type) is not a correct solution
        // as it is a default(T), in case of primitive type it will yield an incorrect behavior
        return memberName switch
        {
            "IsSome" when isOptionMember => Expression.NotEqual(node.Expression, Expression.Default(node.Expression.Type)),
            "IsNone" when isOptionMember => Expression.Equal(node.Expression, Expression.Default(node.Expression.Type)),
            _ => base.VisitMember(node),
        };
    }
}