﻿using Codehard.Functional.MassTransit.CombinedAffTypes;
using System.Diagnostics.Contracts;

// ReSharper disable once CheckNamespace
namespace MassTransit;

/// <summary>
/// The Collection of Async Effect extension
/// </summary>
public static class AsyncEffectExtensions
{
    /// <summary>
    /// Combine Aff type with ConsumeContext type.
    /// Use when you want to response the Aff result via ConsumeContext
    /// </summary>
    [Pure]
    public static AffWithConsumeContext<T> WithConsumeContext<T>(
        this Eff<T> aff,
        ConsumeContext consumerContext)
    {
        return new AffWithConsumeContext<T>(
            aff,
            consumerContext);
    }
}