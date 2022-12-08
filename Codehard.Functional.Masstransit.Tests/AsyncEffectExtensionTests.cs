using MassTransit;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using Codehard.Functional.MassTransit;

namespace Codehard.Functional.Masstransit.Tests;

public class AsyncEffectExtensionTests
{
    [Fact]
    public async Task WhenUseToAffOnGetResponseToSendMessageWithRequestClient_ShouldGetConsumedAndResponded()
    {
        await using var provider = new ServiceCollection()
            .AddMassTransitTestHarness(cfg =>
            {
                cfg.AddConsumer<TestConsumer>();
            })
            .BuildServiceProvider(true);

        var harness = provider.GetRequiredService<ITestHarness>();

        await harness.Start();

        var client = harness.GetRequestClient<PingMessage>();

        var fin = await client
            .GetResponse<PongMessage, PingFaultMessage>(
                new { })
            .ToAff()
            .Run();
        
        Assert.True(fin.IsSucc);
        
        Assert.True(await harness.Consumed.Any<PingMessage>());
        Assert.True(await harness.Sent.Any<PongMessage>());

        var consumerHarness = harness.GetConsumerHarness<TestConsumer>();
        Assert.True(await consumerHarness.Consumed.Any<PingMessage>());
    }
}