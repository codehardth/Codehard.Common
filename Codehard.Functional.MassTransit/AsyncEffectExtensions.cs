using Codehard.Functional.MassTransit.CombinedAffTypes;
using MassTransit;
using System.Diagnostics.Contracts;

namespace Codehard.Functional.MassTransit;

public static class AsyncEffectExtensions
{
    [Pure]
    public static AffWithConsumeContext<T> WithConsumeContext<T>(
        this Aff<T> aff,
        ConsumeContext consumerContext)
    {
        return new AffWithConsumeContext<T>(
            aff,
            consumerContext);
    }
}