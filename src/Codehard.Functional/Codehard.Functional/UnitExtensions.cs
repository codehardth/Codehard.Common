// ReSharper disable once CheckNamespace
namespace LanguageExt;

/// <summary>
/// This static class provides extension methods for the Unit type.
/// </summary>
public static class UnitExtensions
{
    /// <summary>
    /// Converts a Unit value to an Eff&lt;Unit&gt;
    /// </summary>
    /// <param name="_">The Unit value to convert.</param>
    /// <returns>An Eff&lt;Unit&gt; representing the original Unit value.</returns>
    public static Eff<Unit> ToEff(this Unit _) => unitEff;
}