namespace Codehard.Functional.MassTransit.Interfaces;

/// <summary>
/// Interface for message that must be correlate-able.
/// </summary>
public interface ICorrelatableMessage
{
    /// <summary>
    /// Correlation Id for a message.
    /// </summary>
    public Guid CorrelationId { get; set; }
}