using MassTransit;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using Codehard.Functional.MassTransit.Exceptions;
using static LanguageExt.Prelude;

namespace Codehard.Functional.Masstransit.Tests;

public class AsyncEffectExtensionTests
{
    [Fact]
    public async Task WhenUseToAffOnGetResponseToSendMessageWithRequestClient_ShouldGetConsumedAndResponded()
    {
        await using var provider = new ServiceCollection()
            .AddMassTransitTestHarness(cfg => { cfg.AddConsumer<TestConsumer>(); })
            .BuildServiceProvider(true);

        var harness = provider.GetRequiredService<ITestHarness>();

        await harness.Start();

        var client = harness.GetRequestClient<PingMessage>();

        var fin = await client
            .GetResponse<PongMessage, PingFaultMessage>(
                new PingMessage
                {
                    ShouldSuccess = true,
                })
            .ToAff()
            .Run();

        Assert.True(fin.IsSucc);

        Assert.True(await harness.Consumed.Any<PingMessage>());
        Assert.True(await harness.Sent.Any<PongMessage>());

        var consumerHarness = harness.GetConsumerHarness<TestConsumer>();
        Assert.True(await consumerHarness.Consumed.Any<PingMessage>());
    }

    [Fact]
    public async Task WhenUseToAffOnGetResponseToSendMessage_AndExpectFailMessage_ShouldReturnFaultMessageException()
    {
        await using var provider = new ServiceCollection()
            .AddMassTransitTestHarness(cfg => { cfg.AddConsumer<TestConsumer>(); })
            .BuildServiceProvider(true);

        var harness = provider.GetRequiredService<ITestHarness>();

        await harness.Start();

        var client = harness.GetRequestClient<PingMessage>();

        var fin = await client
            .GetResponse<PongMessage, PingFaultMessage>(
                new PingMessage
                {
                    ShouldSuccess = false,
                })
            .ToAff()
            .Run();

        Assert.True(fin.IsFail);

        _ = fin.Match(Succ: _ => unit, Fail: err =>
        {
            var exception = err.ToException();

            Assert.IsAssignableFrom<MassTransitFaultMessageException>(exception);
            Assert.IsAssignableFrom<MassTransitFaultMessageException<PingFaultMessage>>(exception);

            return unit;
        });

        Assert.True(await harness.Consumed.Any<PingMessage>());
        Assert.True(await harness.Sent.Any<PingFaultMessage>());

        var consumerHarness = harness.GetConsumerHarness<TestConsumer>();
        Assert.True(await consumerHarness.Consumed.Any<PingMessage>());
    }
}