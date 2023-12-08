using System.Linq.Expressions;

// ReSharper disable once CheckNamespace
namespace LanguageExt;

/// <summary>
/// Extension methods for working with Option monad to create predicates and expression predicates.
/// </summary>
public static class OptionExtensions
{
    /// <summary>
    /// Converts an Option&lt;string&gt; to a predicate of type Func&lt;T, bool&gt; using the specified conversion function.
    /// </summary>
    /// <typeparam name="T">The type of the parameter for the predicate.</typeparam>
    /// <param name="optional">The Option&lt;string&gt; to convert.</param>
    /// <param name="predicate">The function to convert the string value to a predicate of type Func&lt;T, bool&gt;.</param>
    /// <returns>An Option&lt;Func&lt;T, bool&gt;&gt; representing the predicate.</returns>
    public static Option<Func<T, bool>> ToPredicate<T>(
        this Option<string> optional,
        Func<string, Func<T, bool>> predicate)
        => optional.ToPredicate<string, T>(predicate);
    
    /// <summary>
    /// Converts an Option&lt;int&gt; to a predicate of type Func&lt;T, bool&gt; using the specified conversion function.
    /// </summary>
    /// <typeparam name="T">The type of the parameter for the predicate.</typeparam>
    /// <param name="optional">The Option&lt;int&gt; to convert.</param>
    /// <param name="predicate">The function to convert the int value to a predicate of type Func&lt;T, bool&gt;.</param>
    /// <returns>An Option&lt;Func&lt;T, bool&gt;&gt; representing the predicate.</returns>
    public static Option<Func<T, bool>> ToPredicate<T>(
        this Option<int> optional,
        Func<int, Func<T, bool>> predicate)
        => optional.ToPredicate<int, T>(predicate);
    
    /// <summary>
    /// Converts an Option&lt;int&gt; to a predicate of type Func&lt;int, bool&gt; using the specified conversion function.
    /// </summary>
    /// <param name="optional">The Option&lt;int&gt; to convert.</param>
    /// <param name="predicate">The function to convert the int value to a predicate of type Func&lt;int, bool&gt;.</param>
    /// <returns>An Option&lt;Func&lt;int, bool&gt;&gt; representing the predicate.</returns>
    public static Option<Func<int, bool>> ToPredicate(
        this Option<int> optional,
        Func<int, Func<int, bool>> predicate)
        => optional.ToPredicate<int>(predicate);
    
    /// <summary>
    /// Converts an Option&lt;int&gt; to a predicate that checks for equality with the value inside the Option.
    /// </summary>
    /// <param name="optional">The Option&lt;int&gt; to convert.</param>
    /// <returns>An Option&lt;Func&lt;int, bool&gt;&gt; representing the equality predicate.</returns>
    public static Option<Func<int, bool>> ToEqualPredicate(
        this Option<int> optional)
        => optional.Map<Func<int, bool>>(val => x => x == val);
    
    /// <summary>
    /// Converts an Option&lt;string&gt; to a predicate of type Func&lt;string, bool&gt; using the specified conversion function.
    /// </summary>
    /// <param name="optional">The Option&lt;string&gt; to convert.</param>
    /// <param name="predicate">The function to convert the string value to a predicate of type Func&lt;string, bool&gt;.</param>
    /// <returns>An Option&lt;Func&lt;string, bool&gt;&gt; representing the predicate.</returns>
    public static Option<Func<string, bool>> ToPredicate(
        this Option<string> optional,
        Func<string, Func<string, bool>> predicate)
        => optional.ToPredicate<string>(predicate);
    
    /// <summary>
    /// Converts an Option&lt;DateTimeOffset&gt; to a predicate of type Func&lt;DateTimeOffset, bool&gt; using the specified conversion function.
    /// </summary>
    /// <param name="optional">The Option&lt;DateTimeOffset&gt; to convert.</param>
    /// <param name="predicate">The function to convert the DateTimeOffset value to a predicate of type Func&lt;DateTimeOffset, bool&gt;.</param>
    /// <returns>An Option&lt;Func&lt;DateTimeOffset, bool&gt;&gt; representing the predicate.</returns>
    public static Option<Func<DateTimeOffset, bool>> ToPredicate(
        this Option<DateTimeOffset> optional,
        Func<DateTimeOffset, Func<DateTimeOffset, bool>> predicate)
        => optional.ToPredicate<DateTimeOffset>(predicate);
    
    /// <summary>
    /// Converts an Option&lt;T1&gt; to a predicate of type Func&lt;T1, bool&gt; using the specified conversion function.
    /// </summary>
    /// <typeparam name="T1">The type of the parameter for the predicate.</typeparam>
    /// <param name="optional">The Option&lt;T1&gt; to convert.</param>
    /// <param name="predicate">The function to convert the T1 value to a predicate of type Func&lt;T1, bool&gt;.</param>
    /// <returns>An Option&lt;Func&lt;T1, bool&gt;&gt; representing the predicate.</returns>
    public static Option<Func<T1, bool>> ToPredicate<T1>(
        this Option<T1> optional,
        Func<T1, Func<T1, bool>> predicate)
        => optional.Map(predicate);
    
    /// <summary>
    /// Converts an Option&lt;T1&gt; to a predicate of type Func&lt;T2, bool&gt; using the specified conversion function.
    /// </summary>
    /// <typeparam name="T1">The type of the parameter for the predicate.</typeparam>
    /// <typeparam name="T2">The type of the parameter for the converted predicate.</typeparam>
    /// <param name="optional">The Option&lt;T1&gt; to convert.</param>
    /// <param name="predicate">The function to convert the T1 value to a predicate of type Func&lt;T2, bool&gt;.</param>
    /// <returns>An Option&lt;Func&lt;T2, bool&gt;&gt; representing the predicate.</returns>
    public static Option<Func<T2, bool>> ToPredicate<T1, T2>(
        this Option<T1> optional,
        Func<T1, Func<T2, bool>> predicate)
        => optional.Map(predicate);
    
    /// <summary>
    /// Converts an Option&lt;string&gt; to an expression predicate of type Expression&lt;Func&lt;T, bool&gt;&gt; using the specified conversion function.
    /// </summary>
    /// <typeparam name="T">The type of the parameter for the expression predicate.</typeparam>
    /// <param name="optional">The Option&lt;string&gt; to convert.</param>
    /// <param name="predicate">The function to convert the string value to an expression predicate of type Expression&lt;Func&lt;T, bool&gt;&gt;.</param>
    /// <returns>An Option&lt;Expression&lt;Func&lt;T, bool&gt;&gt;&gt; representing the expression predicate.</returns>
    public static Option<Expression<Func<T, bool>>> ToPredExpr<T>(
        this Option<string> optional,
        Func<string, Expression<Func<T, bool>>> predicate)
        => optional.ToPredExpr<string, T>(predicate);
    
    /// <summary>
    /// Converts an Option&lt;int&gt; to an expression predicate of type Expression&lt;Func&lt;T, bool&gt;&gt; using the specified conversion function.
    /// </summary>
    /// <typeparam name="T">The type of the parameter for the expression predicate.</typeparam>
    /// <param name="optional">The Option&lt;int&gt; to convert.</param>
    /// <param name="predicate">The function to convert the int value to an expression predicate of type Expression&lt;Func&lt;T, bool&gt;&gt;.</param>
    /// <returns>An Option&lt;Expression&lt;Func&lt;T, bool&gt;&gt;&gt; representing the expression predicate.</returns>
    public static Option<Expression<Func<T, bool>>> ToPredExpr<T>(
        this Option<int> optional,
        Func<int, Expression<Func<T, bool>>> predicate)
        => optional.ToPredExpr<int, T>(predicate);
    
    /// <summary>
    /// Converts an Option&lt;DateTimeOffset&gt; to an expression predicate of type Expression&lt;Func&lt;T, bool&gt;&gt; using the specified conversion function.
    /// </summary>
    /// <typeparam name="T">The type of the parameter for the expression predicate.</typeparam>
    /// <param name="optional">The Option&lt;DateTimeOffset&gt; to convert.</param>
    /// <param name="predicate">The function to convert the DateTimeOffset value to an expression predicate of type Expression&lt;Func&lt;T, bool&gt;&gt;.</param>
    /// <returns>An Option&lt;Expression&lt;Func&lt;T, bool&gt;&gt;&gt; representing the expression predicate.</returns>
    public static Option<Expression<Func<T, bool>>> ToPredExpr<T>(
        this Option<DateTimeOffset> optional,
        Func<DateTimeOffset, Expression<Func<T, bool>>> predicate)
        => optional.ToPredExpr<DateTimeOffset, T>(predicate);
    
    /// <summary>
    /// Converts an Option&lt;int&gt; to an expression predicate of type Expression&lt;Func&lt;int, bool&gt;&gt; using the specified conversion function.
    /// </summary>
    /// <param name="optional">The Option&lt;int&gt; to convert.</param>
    /// <param name="predicate">The function to convert the int value to an expression predicate of type Expression&lt;Func&lt;int, bool&gt;&gt;.</param>
    /// <returns>An Option&lt;Expression&lt;Func&lt;int, bool&gt;&gt;&gt; representing the expression predicate.</returns>
    public static Option<Expression<Func<int, bool>>> ToPredExpr(
        this Option<int> optional,
        Func<int, Expression<Func<int, bool>>> predicate)
        => optional.ToPredExpr<int>(predicate);

    /// <summary>
    /// Converts an Option&lt;int&gt; to an expression predicate that checks for equality with the value inside the Option.
    /// </summary>
    /// <param name="optional">The Option&lt;int&gt; to convert.</param>
    /// <returns>An Option&lt;Expression&lt;Func&lt;int, bool&gt;&gt;&gt; representing the equality expression predicate.</returns>
    public static Option<Expression<Func<int, bool>>> ToEqPredExpr(
        this Option<int> optional)
        => optional.ToPredExpr(val => x => x == val);
    
    /// <summary>
    /// Converts an Option&lt;T1&gt; to an expression predicate of type Expression&lt;Func&lt;T2, bool&gt;&gt; using the specified conversion function.
    /// </summary>
    /// <typeparam name="T1">The type of the parameter for the Option.</typeparam>
    /// <typeparam name="T2">The type of the parameter for the expression predicate.</typeparam>
    /// <param name="optional">The Option&lt;T1&gt; to convert.</param>
    /// <param name="predicate">The function to convert the T1 value to an expression predicate of type Expression&lt;Func&lt;T2, bool&gt;&gt;.</param>
    /// <returns>An Option&lt;Expression&lt;Func&lt;T2, bool&gt;&gt;&gt; representing the expression predicate.</returns>
    public static Option<Expression<Func<T2, bool>>> ToPredExpr<T1, T2>(
        this Option<T1> optional,
        Func<T1, Expression<Func<T2, bool>>> predicate)
        => optional.Map(predicate);

    /// <summary>
    /// Executes a function if the Option contains a value, otherwise returns an Aff monad representing a unit of work that does nothing.
    /// </summary>
    /// <typeparam name="T">The type of the value contained in the Option.</typeparam>
    /// <param name="optional">The Option to check for a value.</param>
    /// <param name="ifSome">The function to execute if the Option contains a value. The function takes the value as a parameter and returns an Aff monad representing a unit of work.</param>
    /// <returns>An Aff monad representing a unit of work. If the Option contains a value, the monad represents the work defined by the function. If the Option does not contain a value, the monad represents a unit of work that does nothing.</returns>
    public static Aff<Unit> IfSomeAff<T>(
        this Option<T> optional, Func<T, Aff<Unit>> ifSome)
        => optional.Match(
            Some: ifSome,
            None: unitAff);
    
    /// <summary>
    /// Executes a function if the Option contains a value, otherwise returns an Eff monad representing a unit of work that does nothing.
    /// </summary>
    /// <typeparam name="T">The type of the value contained in the Option.</typeparam>
    /// <param name="optional">The Option to check for a value.</param>
    /// <param name="ifSome">The function to execute if the Option contains a value. The function takes the value as a parameter and returns an Eff monad representing a unit of work.</param>
    /// <returns>An Eff monad representing a unit of work. If the Option contains a value, the monad represents the work defined by the function. If the Option does not contain a value, the monad represents a unit of work that does nothing.</returns>
    public static Eff<Unit> IfSomeEff<T>(
        this Option<T> optional, Func<T, Eff<Unit>> ifSome)
        => optional.Match(
            Some: ifSome,
            None: unitEff);
    
    /// <summary>
    /// Executes a function if the Option contains a value, otherwise returns an Eff monad representing a unit of work that does nothing.
    /// </summary>
    /// <typeparam name="T">The type of the value contained in the Option.</typeparam>
    /// <param name="optional">The Option to check for a value.</param>
    /// <param name="ifSome">The function to execute if the Option contains a value. The function takes the value as a parameter and returns an Eff monad representing a unit of work.</param>
    /// <returns>An Eff monad representing a unit of work. If the Option contains a value, the monad represents the work defined by the function. If the Option does not contain a value, the monad represents a unit of work that does nothing.</returns>
    public static Aff<Unit> IfSomeAsyncAsAff<T>(
        this Option<T> optional, Func<T, Task<Unit>> ifSome)
        => optional.Match(
            Some: val => Aff(async () => await ifSome(val)),
            None: unitAff);
    
    /// <summary>
    /// Executes a function if the Option contains a value, otherwise returns an Eff monad representing a unit of work that does nothing.
    /// </summary>
    /// <typeparam name="T">The type of the value contained in the Option.</typeparam>
    /// <param name="optional">The Option to check for a value.</param>
    /// <param name="ifSome">The function to execute if the Option contains a value. The function takes the value as a parameter and returns an Eff monad representing a unit of work.</param>
    /// <returns>An Eff monad representing a unit of work. If the Option contains a value, the monad represents the work defined by the function. If the Option does not contain a value, the monad represents a unit of work that does nothing.</returns>
    public static Eff<Unit> IfSomeAsEff<T>(
        this Option<T> optional, Func<T, Unit> ifSome)
        => optional.Match(
            Some: val => Eff(() => ifSome(val)),
            None: unitEff);
}