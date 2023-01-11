using Codehard.Functional.MassTransit.Interfaces;

namespace Codehard.Functional.MassTransit.Exceptions;

/// <summary>
/// Custom exception that contains failure message from a request via MassTransit's Request Client
/// </summary>
[Serializable]
public abstract class MassTransitFaultMessageException : Exception
{
    /// <summary>
    /// Correlation Id for fault message.
    /// </summary>
    public abstract Guid CorrelationId { get; }

    /// <summary>
    /// A reason for fault message, may be null.
    /// </summary>
    public abstract string? Reason { get; }
}

/// <summary>
/// Custom exception that contains failure message from a request via MassTransit's Request Client, with error object.
/// </summary>
/// <typeparam name="T"></typeparam>
[Serializable]
public sealed class MassTransitFaultMessageException<T> : MassTransitFaultMessageException
    where T : IFaultMessage
{
    /// <summary>
    /// Data returned alongside the fault message.
    /// </summary>
    public readonly T Object;

    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="object"></param>
    internal MassTransitFaultMessageException(T @object)
    {
        this.Object = @object;
    }

    /// <inheritdoc/>
    public override Guid CorrelationId => this.Object.CorrelationId;

    /// <inheritdoc/>
    public override string? Reason => this.Object.Reason;
}