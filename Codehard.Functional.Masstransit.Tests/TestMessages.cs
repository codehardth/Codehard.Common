using Codehard.Functional.MassTransit.Interfaces;

namespace Codehard.Functional.Masstransit.Tests;

public class PingMessage : ICorrelatableMessage
{
    public Guid CorrelationId { get; }
}

public class PongMessage : ICorrelatableMessage
{
    public Guid CorrelationId { get; }
}

public class PingFaultMessage : IFaultMessage
{
    public Guid CorrelationId { get; }
    public string? Reason { get; set; }
}