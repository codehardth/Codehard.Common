using System.Net;
using Codehard.Functional.AspNetCore;
using LanguageExt;
using LanguageExt.Common;
using Microsoft.Extensions.Logging;
using Moq;
using static LanguageExt.Prelude;

namespace Codehard.Functional.Logger.Tests
{
    public class LoggerExtensionTests
    {
        [Fact]
        public void WhenLogWithErrorWithInnerError_ShouldLogAllError()
        {
            // Arrange
            var innerError = Error.New(1, "Inner error");
            var error = Error.New(2, "Outer error", innerError);

            var mockedLogger = new Mock<ILogger>();

            // Act
            LoggerExtensions.Log(mockedLogger.Object, error);

            // Assert
            VerifyLogMessage("1 : Inner error");
            VerifyLogMessage("2 : Outer error");

            void VerifyLogMessage(string message)
            {
                mockedLogger.Verify(
                    x => x.Log(
                        LogLevel.Information,
                        It.IsAny<EventId>(),
                        It.Is<It.IsAnyType>((o, t) =>
                            string.Equals(message, o.ToString(), StringComparison.InvariantCultureIgnoreCase)),
                        It.IsAny<Exception>(),
                        It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                    Times.Once);
            }
        }

        [Fact]
        public async Task WhenRunFailAff_WithLog_ShouldLogInformation_ThenLogError()
        {
            // Arrange
            const string failMessage = "fail successfully.";

            Mock<ILogger> mockedLogger = new Mock<ILogger>();

            var aff = FailAff<Unit>(Error.New(failMessage));

            // Act
            _ = await aff.WithLog(mockedLogger.Object).Run();

            // Assert
            VerifyLogLevel(LogLevel.Information, 1);
            VerifyLogLevel(LogLevel.Error, 1);

            void VerifyLogLevel(LogLevel logLevel, int times)
            {
                mockedLogger.Verify(
                    x => x.Log(
                        logLevel,
                        It.IsAny<EventId>(),
                        It.IsAny<It.IsAnyType>(),
                        It.IsAny<Exception>(),
                        It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                    Times.Exactly(times));
            }
        }

        [Fact]
        public async Task WhenRunAff_WithLog_ShouldLogInformation_Twice()
        {
            // Arrange
            Mock<ILogger> mockedLogger = new Mock<ILogger>();

            var aff = SuccessAff(unit);

            // Act
            _ = await aff.WithLog(mockedLogger.Object).Run();

            // Assert
            VerifyLogLevel(LogLevel.Information, 2);

            void VerifyLogLevel(LogLevel logLevel, int times)
            {
                mockedLogger.Verify(
                    x => x.Log(
                        logLevel,
                        It.IsAny<EventId>(),
                        It.IsAny<It.IsAnyType>(),
                        It.IsAny<Exception>(),
                        It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                    Times.Exactly(times));
            }
        }

        [Fact]
        public void WhenLogWithHttpResultErrorWithInnerError_ShouldLogAllError()
        {
            // Arrange
            var innerError = HttpResultError.New(
                HttpStatusCode.NotFound,
                "Inner not found");
            var error = HttpResultError.New(
                HttpStatusCode.InternalServerError,
                "Outer error",
                error: innerError);

            var mockedLogger = new Mock<ILogger>();

            // Act
            LoggerExtensions.Log(mockedLogger.Object, error);

            // Assert
            VerifyLogMessage("404 : Inner not found", LogLevel.Information);
            VerifyLogMessage("Outer error", LogLevel.Error);

            void VerifyLogMessage(string message, LogLevel logLevel)
            {
                mockedLogger.Verify(
                    x => x.Log(
                        logLevel,
                        It.IsAny<EventId>(),
                        It.Is<It.IsAnyType>((o, t) =>
                            string.Equals(message, o.ToString(), StringComparison.InvariantCultureIgnoreCase)),
                        It.IsAny<Exception>(),
                        It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                    Times.Once);
            }
        }
    }
}