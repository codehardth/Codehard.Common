using System.Linq.Expressions;

namespace Codehard.Functional;

public static class OptionExtensions
{
    public static Option<Func<T, bool>> ToPredicate<T>(
        this Option<string> optional,
        Func<string, Func<T, bool>> predicate)
        => optional.ToPredicate<string, T>(predicate);
    
    public static Option<Func<T, bool>> ToPredicate<T>(
        this Option<int> optional,
        Func<int, Func<T, bool>> predicate)
        => optional.ToPredicate<int, T>(predicate);
    
    public static Option<Func<int, bool>> ToPredicate(
        this Option<int> optional,
        Func<int, Func<int, bool>> predicate)
        => optional.ToPredicate<int>(predicate);
    
    public static Option<Func<int, bool>> ToEqualPredicate(
        this Option<int> optional)
        => optional.Map<Func<int, bool>>(val => x => x == val);
    
    public static Option<Func<string, bool>> ToPredicate(
        this Option<string> optional,
        Func<string, Func<string, bool>> predicate)
        => optional.ToPredicate<string>(predicate);
    
    public static Option<Func<DateTimeOffset, bool>> ToPredicate(
        this Option<DateTimeOffset> optional,
        Func<DateTimeOffset, Func<DateTimeOffset, bool>> predicate)
        => optional.ToPredicate<DateTimeOffset>(predicate);
    
    public static Option<Func<T1, bool>> ToPredicate<T1>(
        this Option<T1> optional,
        Func<T1, Func<T1, bool>> predicate)
        => optional.Map(predicate);
    
    public static Option<Func<T2, bool>> ToPredicate<T1, T2>(
        this Option<T1> optional,
        Func<T1, Func<T2, bool>> predicate)
        => optional.Map(predicate);
    
    public static Option<Expression<Func<T, bool>>> ToPredExpr<T>(
        this Option<string> optional,
        Func<string, Expression<Func<T, bool>>> predicate)
        => optional.ToPredExpr<string, T>(predicate);
    
    public static Option<Expression<Func<T, bool>>> ToPredExpr<T>(
        this Option<int> optional,
        Func<int, Expression<Func<T, bool>>> predicate)
        => optional.ToPredExpr<int, T>(predicate);
    
    public static Option<Expression<Func<T, bool>>> ToPredExpr<T>(
        this Option<DateTimeOffset> optional,
        Func<DateTimeOffset, Expression<Func<T, bool>>> predicate)
        => optional.ToPredExpr<DateTimeOffset, T>(predicate);
    
    public static Option<Expression<Func<int, bool>>> ToPredExpr(
        this Option<int> optional,
        Func<int, Expression<Func<int, bool>>> predicate)
        => optional.ToPredExpr<int>(predicate);

    public static Option<Expression<Func<int, bool>>> ToEqPredExpr(
        this Option<int> optional)
        => optional.ToPredExpr(val => x => x == val);
    
    public static Option<Expression<Func<T2, bool>>> ToPredExpr<T1, T2>(
        this Option<T1> optional,
        Func<T1, Expression<Func<T2, bool>>> predicate)
        => optional.Map(predicate);
}