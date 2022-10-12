namespace Codehard.Functional.MassTransit.Interfaces;

/// <summary>
/// Interface for command message for distributed systems.
/// </summary>
public interface ICommandMessage : ICorrelatableMessage
{
}