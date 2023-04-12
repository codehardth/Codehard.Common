using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using static LanguageExt.Prelude;

namespace Codehard.Functional.AspNetCore.Tests;

public class ControllerExtensionTests
{
    [Fact]
    public async Task WhenRunToResultWithFail_ShouldLogAndReturnCorrespondingHttpHeader()
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
        var actionResult = await aff.MapFailToInternalServerError("Err001")
            .RunToResultAsync();
        await actionResult.ExecuteResultAsync(actionContextMock.Object);

        // Assert
        Assert.IsType<ErrorWrapperActionResult>(actionResult);
        Assert.Equal("Err001", headerDictionary["x-error-code"]);
    }
}