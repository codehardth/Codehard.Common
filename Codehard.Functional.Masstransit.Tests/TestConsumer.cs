using MassTransit;

namespace Codehard.Functional.Masstransit.Tests;

public class TestConsumer : IConsumer<PingMessage>
{
    public async Task Consume(ConsumeContext<PingMessage> context)
    {
        await context.RespondAsync<PongMessage>(new
        {
            context.Message.CorrelationId
        });
    }
}