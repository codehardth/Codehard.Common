using System.Text;
using Lamar;
using LanguageExt;
using MediatR;
using Shouldly;

namespace Codehard.Functional.MediatR.Tests;

public class QueryTests
{
    public class Ping : IQuery<PongQueryResult>
    {
        public string? Message { get; set; }
    }
    
    public class PingNotFound : IQuery<PongQueryResult>
    {
        public string? Message { get; set; }
    }

    public class Pong
    {
        public string? Message { get; set; }
    }

    public class PingHandler 
        : IQueryHandler<Ping, PongQueryResult>,
          IQueryHandler<PingNotFound, PongQueryResult>
    {
        public Task<Fin<PongQueryResult>> Handle(Ping request, CancellationToken cancellationToken)
        {
            return Task.FromResult(
                Fin<PongQueryResult>.Succ(
                    new PongQueryResult.Success(new Pong { Message = request.Message + " Pong" })));
        }
        
        public Task<Fin<PongQueryResult>> Handle(PingNotFound request, CancellationToken cancellationToken)
        {
            return Task.FromResult(
                Fin<PongQueryResult>.Fail(
                    new ExpectedResultError(new PongQueryResult.NotFound())));
        }
    }
    
    public abstract record PongQueryResult
    {
        private PongQueryResult()
        {
        }

        public sealed record Success(Pong Pong) : PongQueryResult;

        public sealed record NotFound : PongQueryResult;
    }
    
    [Fact]
    public async Task WhenSendQuery_ShouldResponseCorrectly()
    {
        // Arrange
        var builder = new StringBuilder();
        var writer = new StringWriter(builder);
        var container = BuildMediatr();

        // Act
        var mediator = container.GetInstance<IMediator>();

        var response =
            await mediator
                .SendQueryEff<Ping, PongQueryResult>(new Ping { Message = "Ping" })
                .MapExpectedResultError()
                .RunAsync();

        // Assert
        Assert.True(response.IsSucc);

        var result = response.ThrowIfFail();
        
        var successValue = result.ShouldBeOfType<PongQueryResult.Success>();
        
        successValue.Pong.Message.ShouldBe("Ping Pong");
        
        return;
        
        Container BuildMediatr()
        {
            var container = new Container(cfg =>
            {
                cfg.Scan(scanner =>
                {
                    scanner.AssemblyContainingType(typeof(QueryTests));
                    scanner.IncludeNamespaceContainingType<Ping>();
                    scanner.WithDefaultConventions();
                    scanner.AddAllTypesOf(typeof(IRequestHandler<,>));
                });
                cfg.For<IMediator>().Use<Mediator>();
            });

            return container;
        }
    }
    
    [Fact]
    public async Task WhenSendNotFoundQuery_ShouldResponseNotFoundCorrectly()
    {
        // Arrange
        var builder = new StringBuilder();
        var writer = new StringWriter(builder);
        var container = BuildMediatr();

        // Act
        var mediator = container.GetInstance<IMediator>();

        var response =
            await mediator
                .SendQueryEff<PingNotFound, PongQueryResult>(new PingNotFound { Message = "Ping" })
                .MapExpectedResultError()
                .RunAsync();

        // Assert
        Assert.True(response.IsSucc);

        var result = response.ThrowIfFail();
        
        result.ShouldBeOfType<PongQueryResult.NotFound>();
        
        return;
        
        Container BuildMediatr()
        {
            var container = new Container(cfg =>
            {
                cfg.Scan(scanner =>
                {
                    scanner.AssemblyContainingType(typeof(QueryTests));
                    scanner.IncludeNamespaceContainingType<Ping>();
                    scanner.WithDefaultConventions();
                    scanner.AddAllTypesOf(typeof(IRequestHandler<,>));
                });
                cfg.For<IMediator>().Use<Mediator>();
            });

            return container;
        }
    }
}