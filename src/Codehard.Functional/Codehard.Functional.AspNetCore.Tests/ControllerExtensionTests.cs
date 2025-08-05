using System.Net;
using LanguageExt;
using LanguageExt.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using static LanguageExt.Prelude;

namespace Codehard.Functional.AspNetCore.Tests;

public class ControllerExtensionTests
{
    [Theory]
    [InlineData(HttpStatusCode.OK, "FailSuccessfully")]
    [InlineData(HttpStatusCode.NotFound, "Err404")]
    [InlineData(HttpStatusCode.InternalServerError, "Err500")]
    public async Task WhenRunToResultWithFail_ShouldLogAndReturnCorrespondingHttpHeader(
        HttpStatusCode expectedStatusCode,
        string expectedData)
    {
        // Arrange
        var aff = Aff(() => ValueTask.FromException<int>(
            new Exception("Something went wrong")));

        var actionContextMock = new Mock<ActionContext>();
        var httpContextMock = new Mock<HttpContext>();
        var httpResponseMock = new Mock<HttpResponse>();
        var headerDictionary = new HeaderDictionary();

        httpResponseMock
            .SetupGet(hr => hr.Headers)
            .Returns(() => headerDictionary);

        httpContextMock
            .SetupGet(hc => hc.Response)
            .Returns(() => httpResponseMock.Object);

        actionContextMock.Object.HttpContext = httpContextMock.Object;

        // Act
        var actionResult =
            await aff.MapFailToHttpResultError(expectedStatusCode, expectedData)
                .RunToResultAsync();
        await actionResult.ExecuteResultAsync(actionContextMock.Object);

        // Assert
        httpResponseMock.VerifySet(
            hr => hr.StatusCode = (int)expectedStatusCode, Times.Once());
        Assert.IsType<ErrorWrapperActionResult>(actionResult);
        Assert.Equal(expectedData, headerDictionary["x-error-code"]);
    }

    [Fact]
    public void WhenMatchToResultWithRegularError_ShouldLogWithWebApiExceptionHandler()
    {
        // Arrange
        var error = Error.New(500, "Test error message");
        var fin = FinFail<string>(error);
        var mockLogger = new Mock<ILogger>();

        // Act
        var result = fin.MatchToResult(logger: mockLogger.Object);

        // Assert
        Assert.IsType<ErrorWrapperActionResult>(result);

        // Verify that Log was called with the error using WebApiExceptionHandler
        // The WebApiExceptionHandler will log with Information level since there's no exception
        mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) =>
                    o.ToString()!.Contains("500") && o.ToString()!.Contains("Test error message")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public void WhenMatchToResultWithHttpResultError_ShouldLogWithoutWebApiExceptionHandler()
    {
        // Arrange
        var httpError = HttpResultError.New(HttpStatusCode.BadRequest, "Bad request error");
        var fin = FinFail<string>(httpError);
        var mockLogger = new Mock<ILogger>();

        // Act
        var result = fin.MatchToResult(logger: mockLogger.Object);

        // Assert
        Assert.IsType<ErrorWrapperActionResult>(result);

        // Verify that Log was called for HttpResultError (uses different logging path - LoggingExtensions.Log)
        mockLogger.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) =>
                    o.ToString()!.Contains("BadRequest") && o.ToString()!.Contains("Bad request error")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public void WhenMatchToResultWithRegularErrorAndNullLogger_ShouldNotThrow()
    {
        // Arrange
        var error = Error.New(500, "Test error message");
        var fin = FinFail<string>(error);

        // Act & Assert
        var exception = Record.Exception(() => fin.MatchToResult(logger: null));
        Assert.Null(exception);

        var result = fin.MatchToResult(logger: null);
        Assert.IsType<ErrorWrapperActionResult>(result);
    }

    [Fact]
    public void WhenMatchToResultWithErrorContainingException_ShouldLogAsError()
    {
        // Arrange
        var exception = new InvalidOperationException("Test exception");
        var error = Error.New(exception);
        var fin = FinFail<string>(error);
        var mockLogger = new Mock<ILogger>();

        // Act
        var result = fin.MatchToResult(logger: mockLogger.Object);

        // Assert
        Assert.IsType<ErrorWrapperActionResult>(result);

        // Verify that LogError was called due to the exception (WebApiExceptionHandler behavior)
        mockLogger.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.Is<Exception>(ex => ex == exception),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public void WhenMatchToResultWithTaskCanceledException_ShouldLogAsInformation()
    {
        // Arrange
        var exception = new TaskCanceledException("Operation was canceled");
        var error = Error.New(exception);
        var fin = FinFail<string>(error);
        var mockLogger = new Mock<ILogger>();

        // Act
        var result = fin.MatchToResult(logger: mockLogger.Object);

        // Assert
        Assert.IsType<ErrorWrapperActionResult>(result);

        // Verify that Log was called with Information level for TaskCanceledException
        // WebApiExceptionHandler has special handling for TaskCanceledException
        mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) =>
                    o.ToString()!.Contains("Operation was canceled")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public void WhenMatchToResultWithOperationCanceledException_ShouldLogAsInformation()
    {
        // Arrange
        var exception = new OperationCanceledException("Operation was canceled");
        var error = Error.New(exception);
        var fin = FinFail<string>(error);
        var mockLogger = new Mock<ILogger>();

        // Act
        var result = fin.MatchToResult(logger: mockLogger.Object);

        // Assert
        Assert.IsType<ErrorWrapperActionResult>(result);

        // Verify that Log was called with Information level for OperationCanceledException
        // WebApiExceptionHandler has special handling for OperationCanceledException
        mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) =>
                    o.ToString()!.Contains("Operation was canceled")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public void WhenMatchToResultWithRegularErrorWithoutMessage_ShouldLogCodeOnly()
    {
        // Arrange
        var error = Error.New(404, ""); // Empty message
        var fin = FinFail<string>(error);
        var mockLogger = new Mock<ILogger>();

        // Act
        var result = fin.MatchToResult(logger: mockLogger.Object);

        // Assert
        Assert.IsType<ErrorWrapperActionResult>(result);

        // Verify that Log was called with just the code when message is empty
        // This tests the WebApiExceptionHandler behavior for empty messages
        mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) =>
                    o.ToString()!.Contains("404") && !o.ToString()!.Contains(":")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public void WhenMatchToResultWithSuccessValue_ShouldNotLog()
    {
        // Arrange
        var fin = FinSucc("success value");
        var mockLogger = new Mock<ILogger>();

        // Act
        var result = fin.MatchToResult(logger: mockLogger.Object);

        // Assert
        Assert.IsType<ObjectResult>(result);
        var objectResult = (ObjectResult)result;
        Assert.Equal("success value", objectResult.Value);
        Assert.Equal(200, objectResult.StatusCode);

        // Verify that no logging occurred for successful results
        mockLogger.Verify(
            x => x.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Never);
    }

    [Fact]
    public void WhenMatchToResultOptionWithRegularError_ShouldLogWithoutWebApiExceptionHandler()
    {
        // Arrange
        var error = Error.New(500, "Test error message");
        var fin = FinFail<Option<string>>(error);
        var mockLogger = new Mock<ILogger>();

        // Act
        var result = fin.MatchToResult(logger: mockLogger.Object);

        // Assert
        Assert.IsType<ErrorWrapperActionResult>(result);

        // Verify that Log was called WITHOUT WebApiExceptionHandler (different from regular MatchToResult)
        // This method uses the default exception handler, not WebApiExceptionHandler.Instance
        mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) =>
                    o.ToString()!.Contains("500") && o.ToString()!.Contains("Test error message")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public void WhenMatchToResultOptionWithSomeValue_ShouldReturnObjectResult()
    {
        // Arrange
        var fin = FinSucc(Some("test value"));
        var mockLogger = new Mock<ILogger>();

        // Act
        var result = fin.MatchToResult(logger: mockLogger.Object);

        // Assert
        Assert.IsType<ObjectResult>(result);
        var objectResult = (ObjectResult)result;
        Assert.Equal("test value", objectResult.Value);
        Assert.Equal(200, objectResult.StatusCode);

        // Verify that no logging occurred for successful results
        mockLogger.Verify(
            x => x.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Never);
    }

    [Fact]
    public void WhenMatchToResultOptionWithNoneValue_ShouldReturnNotFound()
    {
        // Arrange
        var fin = FinSucc(Option<string>.None);
        var mockLogger = new Mock<ILogger>();

        // Act
        var result = fin.MatchToResult(logger: mockLogger.Object);

        // Assert
        Assert.IsType<NotFoundResult>(result);

        // Verify that no logging occurred for None results
        mockLogger.Verify(
            x => x.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Never);
    }
}