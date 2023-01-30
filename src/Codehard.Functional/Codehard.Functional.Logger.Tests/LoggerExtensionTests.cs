using LanguageExt.Common;
using Microsoft.Extensions.Logging;
using Moq;

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
                    It.Is<It.IsAnyType>((o, t) => string.Equals(message, o.ToString(), StringComparison.InvariantCultureIgnoreCase)),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
            }
        }
    }
}