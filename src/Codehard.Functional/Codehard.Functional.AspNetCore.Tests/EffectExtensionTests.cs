using System.Net;
using LanguageExt;
using LanguageExt.Common;
using static LanguageExt.Prelude;

namespace Codehard.Functional.AspNetCore.Tests;

public class EffectExtensionTests
{
    [Fact]
    public void WhenMapFailToInternalServerError_ShouldHaveHttpResultErrorWithCorrespondingHttpStatusCode()
    {
        // Arrange
        var eff = FailEff<int>(new Exception("Something went wrong"));

        // Act
        var res =
            eff.MapFailToInternalServerError("Err001")
               .Run();

        // Assert
        Assert.True(res.IsFail);
        
        var error =
            res.Match(
                Succ: _ => Error.New("?"),
                Fail: err => err);
        Assert.IsType<HttpResultError>(error);

        var httpResultErr = error as HttpResultError;
        Assert.Equal(
            HttpStatusCode.InternalServerError,
            httpResultErr!.StatusCode);
        
        Assert.Equal("Err001", httpResultErr.ErrorCode);
        Assert.Equal(
            "Something went wrong",
            httpResultErr.Inner.Map(err => err.ToException().Message).IfNone(string.Empty));
    }
    
    [Fact]
    public void WhenMapFailToInternalServerError_ShouldHaveHttpResultErrorWithCorrespondingErrorMessage()
    {
        // Arrange
        var eff = FailEff<int>(new Exception("Something not right"));

        // Act
        var res =
            eff.MapFailToInternalServerError(message: "Error Message")
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
    public void WhenMapFailToInternalServerError_ShouldHaveHttpResultErrorWithCorrespondingObject()
    {
        // Arrange
        var eff = FailEff<int>(new Exception("Something went crazy"));

        // Act
        var res =
            eff.MapFailToInternalServerError(data: new SomeData(1, "2"))
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
    public void WhenMapFailToInternalServerErrorWithMessageFunc_ShouldHaveHttpResultErrorWithCorrespondingMessage()
    {
        // Arrange
        var aff = FailEff<int>(new Exception("Something went crazy"));

        // Act
        var res =
            aff.MapFailToInternalServerError(
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
    public void WhenMapFailToOkWithAlreadyMapFailInternalServerError_ShouldOverrideToOk()
    {
        // Arrange
        var eff = FailEff<int>(new Exception("Something not right"));

        // Act
        var res =
            eff.MapFailToInternalServerError(message: "Error Message")
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
    public void WhenMapFailToOkWithNotOverrideOptionOnAlreadyMapFailInternalServerError_ShouldNotOverrideInternalServerError()
    {
        // Arrange
        var eff = FailEff<int>(new Exception("Something not right"));

        // Act
        var res = 
            eff.MapFailToInternalServerError(message: "Error Message")
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