using MassTransit;

namespace Codehard.Functional.Masstransit.Tests;

public class TestConsumer :
    IConsumer<PingMessage>
{
    public Task Consume(ConsumeContext<PingMessage> context)
    {
        if (context.Message.ShouldSuccess)
        {
            return context.RespondAsync<PongMessage>(new
            {
                context.Message.CorrelationId
            });
        }

        return context.RespondAsync<PingFaultMessage>(new
        {
            Reason = "Oops!",
            context.Message.CorrelationId
        });
    }
}