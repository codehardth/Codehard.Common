// ReSharper disable once CheckNamespace
namespace LanguageExt;

public static class EffOptionExtensions
{
    public static Eff<T> IfNone<T>(
        this Eff<Option<T>> optEff, Func<T> noneF)
    {
        return
            optEff.Map(
                valOpt =>
                    valOpt.IfNone(noneF));
    }
}