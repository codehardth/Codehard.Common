using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        var eff =
            liftEff(() => ValueTask.FromException<int>(new Exception("Something went wrong")));

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
            await eff.MapFailToHttpResultError(expectedStatusCode, expectedData)
                .RunToResultAsync();
        
        await actionResult.ExecuteResultAsync(actionContextMock.Object);

        // Assert
        httpResponseMock.VerifySet(hr => hr.StatusCode = (int)expectedStatusCode, Times.Once());
        Assert.IsType<ErrorWrapperActionResult>(actionResult);
        Assert.Equal(expectedData, headerDictionary["x-error-code"]);
    }
}