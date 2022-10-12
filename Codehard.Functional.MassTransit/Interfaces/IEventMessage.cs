namespace Codehard.Functional.MassTransit.Interfaces;

/// <summary>
/// Interface for event message with correlation id for distributed systems.
/// </summary>
public interface IEventMessage
{
    /// <summary>
    /// Correlation Id for current message.
    /// </summary>
    public Guid CorrelationId { get; set; }
}