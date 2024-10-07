using Codehard.Functional.MassTransit.CombinedEffTypes;
using System.Diagnostics.Contracts;

// ReSharper disable once CheckNamespace
namespace MassTransit;

/// <summary>
/// The Collection of Async Effect extension
/// </summary>
public static class EffectExtensions
{
    /// <summary>
    /// Combine Eff type with ConsumeContext type.
    /// Use when you want to response the Eff result via ConsumeContext
    /// </summary>
    [Pure]
    public static EffWithConsumeContext<T> WithConsumeContext<T>(
        this Eff<T> eff,
        ConsumeContext consumerContext)
    {
        return new EffWithConsumeContext<T>(
            eff,
            consumerContext);
    }
}