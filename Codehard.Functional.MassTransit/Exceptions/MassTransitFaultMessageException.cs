using Codehard.Functional.MassTransit.Interfaces;

namespace Codehard.Functional.MassTransit.Exceptions;

/// <summary>
/// Custom exception that contains failure message from a request via MassTransit's Request Client
/// </summary>
/// <typeparam name="T"></typeparam>
public class MassTransitFaultMessageException<T> : Exception
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
    public MassTransitFaultMessageException(T @object)
    {
        this.Object = @object;
    }

    /// <summary>
    /// Correlation Id for fault message.
    /// </summary>
    public Guid CorrelationId => this.Object.CorrelationId;

    /// <summary>
    /// A reason for fault message, may be null.
    /// </summary>
    public string? Reason => this.Object.Reason;
}