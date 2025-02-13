using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
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
            liftEff(async () => await ValueTask.FromException<int>(new Exception("Something went wrong")));

        var actionContextMock = new Mock<ActionContext>();
        var httpContextMock = new Mock<HttpContext>();
        var httpResponseMock = new Mock<HttpResponse>();
        var httpRequestMock = new Mock<HttpRequest>();
        var serviceProviderMock = new Mock<IServiceProvider>();
        var actionResultExecutorMock = new Mock<IActionResultExecutor<ObjectResult>>();
        
        var headerDictionary = new HeaderDictionary(); 
        
        httpContextMock
            .SetupGet(hc => hc.RequestServices)
            .Returns(() => serviceProviderMock.Object);

        httpResponseMock
            .SetupGet(hr => hr.Headers)
            .Returns(() => headerDictionary);

        httpContextMock
            .SetupGet(hc => hc.Response)
            .Returns(() => httpResponseMock.Object);
        
        httpContextMock
            .SetupGet(hc => hc.Request)
            .Returns(() => httpRequestMock.Object);
        
        actionContextMock.Object.HttpContext = httpContextMock.Object;

        serviceProviderMock
            .Setup(sp => sp.GetService(typeof(IActionResultExecutor<ObjectResult>)))
            .Returns(actionResultExecutorMock.Object);
        
        // Act
        var actionResult =
            await eff
                .MapFailToHttpResultError(expectedStatusCode, expectedData)
                .RunToResultAsync();
        
        await actionResult.ExecuteResultAsync(actionContextMock.Object);
        
        httpResponseMock.VerifySet(hr => hr.StatusCode = (int)expectedStatusCode, Times.Once());
        
        Assert.IsType<ErrorWrapperActionResult>(actionResult);
        Assert.Equal(expectedData, headerDictionary["x-error-code"]);
    }
}