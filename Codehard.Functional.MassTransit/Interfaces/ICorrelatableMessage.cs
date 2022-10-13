using MassTransit;

namespace Codehard.Functional.MassTransit.Interfaces;

/// <summary>
/// Interface for message that must be correlate-able.
/// </summary>
public interface ICorrelatableMessage : CorrelatedBy<Guid>
{
}