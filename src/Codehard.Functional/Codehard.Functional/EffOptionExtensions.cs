// ReSharper disable once CheckNamespace
namespace LanguageExt;

/// <summary>
/// Provides extension methods for working with Eff&lt;Option&lt;T>> types.
/// </summary>
public static class EffOptionExtensions
{
    /// <summary>
    /// Transforms an Eff&lt;Option&lt;T>> into an Eff&lt;T> by providing a default value function when the Option is None.
    /// </summary>
    /// <typeparam name="T">The type of the value in the Option.</typeparam>
    /// <param name="optEff">The Eff&lt;Option&lt;T>> to transform.</param>
    /// <param name="noneF">The function to execute when the Option is None.</param>
    /// <returns>An Eff&lt;T> containing either the Some value or the result of noneF.</returns>
    public static Eff<T> IfNone<T>(
        this Eff<Option<T>> optEff, Func<T> noneF)
    {
        return
            optEff.Map(
                valOpt =>
                    valOpt.IfNone(noneF));
    }
}