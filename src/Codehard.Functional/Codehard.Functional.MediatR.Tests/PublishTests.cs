using System.Text;
using Lamar;
using LanguageExt;
using MediatR;
using Shouldly;

namespace Codehard.Functional.MediatR.Tests;

public class PublishTests
{
    public class Ping : INotification
    {
        public string Message { get; init; }
    }
    
    [Fact]
    public async Task WhenPublishMessage_ShouldNotifiedEachHandlersCorrectly()
    {
        // Arrange
        var builder = new StringBuilder();
        var writer = new StringWriter(builder);
        var container = BuildMediatr();

        // Act
        var mediator = container.GetInstance<IMediator>();

        _ = await mediator.PublishEff(new Ping { Message = "Ping" })
                          .RunAsync();

        // Assert
        var result = builder.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.None);
        result.ShouldContain("Ping Pong");
        result.ShouldContain("Ping Pung");

        Container BuildMediatr()
        {
            var container = new Container(cfg =>
            {
                cfg.Scan(scanner =>
                {
                    scanner.AssemblyContainingType(typeof(PublishTests));
                    scanner.IncludeNamespaceContainingType<Ping>();
                    scanner.WithDefaultConventions();
                    scanner.AddAllTypesOf(typeof (INotificationHandler<>));
                });
                cfg.For<TextWriter>().Use(writer);
                cfg.For<IMediator>().Use<Mediator>();
            });

            return container;
        }
    }
    
    public class PongHandler : INotificationHandler<Ping>
    {
        private readonly TextWriter _writer;

        public PongHandler(TextWriter writer)
        {
            _writer = writer;
        }

        public Task Handle(Ping notification, CancellationToken cancellationToken)
        {
            return _writer.WriteLineAsync(notification.Message + " Pong");
        }
    }
    
    public class PungHandler : INotificationHandler<Ping>
    {
        private readonly TextWriter _writer;

        public PungHandler(TextWriter writer)
        {
            _writer = writer;
        }

        public Task Handle(Ping notification, CancellationToken cancellationToken)
        {
            return _writer.WriteLineAsync(notification.Message + " Pung");
        }
    }
}