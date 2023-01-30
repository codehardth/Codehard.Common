namespace Codehard.Functional.MassTransit.Interfaces;

/// <summary>
/// Interface for event message that indicate invalid state with correlation id for distributed systems.
/// </summary>
public interface IFaultMessage : ICorrelatableMessage
{
    /// <summary>
    /// A reason for fault message, may be null.
    /// </summary>
    public string? Reason { get; set; }
}