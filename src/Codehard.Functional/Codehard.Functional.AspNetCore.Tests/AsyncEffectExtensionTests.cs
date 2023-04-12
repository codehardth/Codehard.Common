using System.Net;
using LanguageExt.Common;
using static LanguageExt.Prelude;

namespace Codehard.Functional.AspNetCore.Tests;

public class AsyncEffectExtensionTests
{
    [Fact]
    public async Task WhenMapFailToInternalServerError_ShouldHaveHttpResultErrorWithCorrespondingHttpStatusCode()
    {
        // Arrange
        var aff = Aff(() => ValueTask.FromException<int>(new Exception("Something went wrong")));

        // Act
        var res = await aff.MapFailToInternalServerError("Err001")
            .Run();

        // Assert
        Assert.True(res.IsFail);
        var error = res.Match(
            Succ: _ => Error.New("?"),
            Fail: err => err);
        Assert.IsType<HttpResultError>(error);

        var httpResultErr = error as HttpResultError;
        Assert.Equal(HttpStatusCode.InternalServerError, httpResultErr!.StatusCode);
        Assert.Equal("Err001", httpResultErr.ErrorCode);
        Assert.Equal(
            "Something went wrong",
            httpResultErr.Inner.Map(err => err.ToException().Message).IfNone(string.Empty));
    }
    
    [Fact]
    public async Task WhenMapFailToInternalServerError_ShouldHaveHttpResultErrorWithCorrespondingErrorMessage()
    {
        // Arrange
        var aff = Aff(() => ValueTask.FromException<int>(new Exception("Something not right")));

        // Act
        var res = await aff.MapFailToInternalServerError(message: "Error Message")
            .Run();

        // Assert
        Assert.True(res.IsFail);
        var error = res.Match(
            Succ: _ => Error.New("?"),
            Fail: err => err);
        Assert.IsType<HttpResultError>(error);

        var httpResultErr = error as HttpResultError;
        Assert.Equal("Error Message", httpResultErr!.Message);
    }

    record SomeData(int Number, string Text);

    [Fact]
    public async Task WhenMapFailToInternalServerError_ShouldHaveHttpResultErrorWithCorrespondingObject()
    {
        // Arrange
        var aff = Aff(() => ValueTask.FromException<int>(new Exception("Something went crazy")));

        // Act
        var res = await aff.MapFailToInternalServerError(data: new SomeData(1, "2"))
            .Run();

        // Assert
        Assert.True(res.IsFail);
        var error = res.Match(
            Succ: _ => Error.New("?"),
            Fail: err => err);
        Assert.IsType<HttpResultError>(error);

        var httpResultErr = error as HttpResultError;
        var someData = (SomeData)httpResultErr!.Data;
        Assert.Equal(1, someData.Number);
        Assert.Equal("2", someData.Text);
    }
    
    [Fact]
    public async Task WhenMapFailToInternalServerErrorWithMessageFunc_ShouldHaveHttpResultErrorWithCorrespondingMessage()
    {
        // Arrange
        var aff = Aff(() => ValueTask.FromException<int>(new Exception("Something went crazy")));

        // Act
        var res = await aff.MapFailToInternalServerError(
                err => $"The error message is {err.Message}")
            .Run();

        // Assert
        Assert.True(res.IsFail);
        var error = res.Match(
            Succ: _ => Error.New("?"),
            Fail: err => err);
        Assert.IsType<HttpResultError>(error);

        var httpResultErr = error as HttpResultError;
        Assert.Equal("The error message is Something went crazy", httpResultErr!.Message);
    }
    
    [Fact]
    public async Task WhenMapFailToOkWithAlreadyMapFailInternalServerError_ShouldOverrideToOk()
    {
        // Arrange
        var aff = Aff(() => ValueTask.FromException<int>(new Exception("Something not right")));

        // Act
        var res = await aff.MapFailToInternalServerError(message: "Error Message")
            .MapFailToOK()
            .Run();

        // Assert
        Assert.True(res.IsFail);
        var error = res.Match(
            Succ: _ => Error.New("?"),
            Fail: err => err);
        Assert.IsType<HttpResultError>(error);

        var httpResultErr = error as HttpResultError;
        Assert.Equal(HttpStatusCode.OK, httpResultErr!.StatusCode);
    }
    
    [Fact]
    public async Task WhenMapFailToOkWithNotOverrideOptionOnAlreadyMapFailInternalServerError_ShouldNotOverrideInternalServerError()
    {
        // Arrange
        var aff = Aff(() => ValueTask.FromException<int>(new Exception("Something not right")));

        // Act
        var res = await aff.MapFailToInternalServerError(message: "Error Message")
            .MapFailToOK(@override: false)
            .Run();

        // Assert
        Assert.True(res.IsFail);
        var error = res.Match(
            Succ: _ => Error.New("?"),
            Fail: err => err);
        Assert.IsType<HttpResultError>(error);

        var httpResultErr = error as HttpResultError;
        Assert.Equal(HttpStatusCode.InternalServerError, httpResultErr!.StatusCode);
    }
}