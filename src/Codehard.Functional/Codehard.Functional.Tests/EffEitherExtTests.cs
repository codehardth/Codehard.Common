namespace Codehard.Functional.Tests;

public class EffEitherExtTests
{
    [Fact]
    public void WhenCallMapNoneToLeftOnEffOfNone_ShouldGetEffOfLeft()
    {
        // Arrange
        var effOpt = SuccessEff(Option<int>.None);

        // Act
        var result = 
            effOpt.MapNoneToLeft(() => "Something wrong")
                  .Run()
                  .ThrowIfFail();

        // Assert
        Assert.True(result.IsLeft);
        Assert.Equal("Something wrong", result.LeftSpan()[0]);
    }
    
    [Fact]
    public void WhenCallMapNoneToLeftOnEffOfSome_ShouldGetEffOfRight()
    {
        // Arrange
        var effOpt = SuccessEff(Option<int>.Some(1));

        // Act
        var result = 
            effOpt.MapNoneToLeft(() => "Something wrong")
                  .Run()
                  .ThrowIfFail();

        // Assert
        Assert.True(result.IsRight);
        Assert.Equal(1, result.RightSpan()[0]);
    }
    
    [Fact]
    public void WhenGuardToLeftOnEffWithTruePredicate_ShouldGetEffOfRight()
    {
        // Arrange
        var eff = SuccessEff(1);

        // Act
        var result =
            eff.GuardToLeft(
                    i => i > 0,
                    () => "Something wrong")
               .Run()
               .ThrowIfFail();

        // Assert
        Assert.True(result.IsRight);
        Assert.Equal(1, result.RightSpan()[0]);
    }
    
    [Fact]
    public void WhenGuardEitherOnEffOfRightWithFalsePredicate_ShouldGetEffOfLeft()
    {
        // Arrange
        var eff = SuccessEff(Right<string, int>(1));

        // Act
        var result = 
            eff.GuardEither(
                    i => i > 2,
                    () => "Something wrong")
               .Run()
               .ThrowIfFail();

        // Assert
        Assert.True(result.IsLeft);
        Assert.Equal("Something wrong", result.LeftSpan()[0]);
    }
    
    [Fact]
    public async Task WhenGuardEitherAsyncOnEffOfRightWithFalsePredicateAndLeftAsync_ShouldGetEffOfLeft()
    {
        // Arrange
        var eff = SuccessEff(Right<string, int>(1));

        // Act
        var result =
            (await eff.GuardEitherAsync(
                    i => i > 2,
                    _ => ValueTask.FromResult("Something wrong"))
                .RunAsync())
            .ThrowIfFail();

        // Assert
        Assert.True(result.IsLeft);
        Assert.Equal("Something wrong", result.LeftSpan()[0]);
    }
    
    [Fact]
    public async Task WhenGuardEitherAsyncOnEffOfRightWithFalsePredicateAsyncAndLeftAsync_ShouldGetEffOfLeft()
    {
        // Arrange
        var eff = SuccessEff(Right<string, int>(1));

        // Act
        var result =
            (await eff.GuardEitherAsync(
                    i => ValueTask.FromResult(i > 2),
                    _ => ValueTask.FromResult("Something wrong"))
                .RunAsync())
            .ThrowIfFail();

        // Assert
        Assert.True(result.IsLeft);
        Assert.Equal("Something wrong", result.LeftSpan()[0]);
    }

    [Fact]
    public async Task WhenMapRightOnEffOfRight_ShouldMapToNewValue()
    {
        // Arrange
        var eff = SuccessEff(Right<string, int>(1));

        // Act
        var result =
            (await eff.MapRight(i => i + 1)
                      .RunAsync())
            .ThrowIfFail();

        // Assert
        Assert.True(result.IsRight);
        Assert.Equal(2, result.RightSpan()[0]);
    }
    
    [Fact]
    public async Task WhenMapRightAsyncOnEffOfRight_ShouldMapToNewValue()
    {
        // Arrange
        var eff = SuccessEff(Right<string, int>(1));

        // Act
        var result =
            (await eff.MapRightAsync(i => ValueTask.FromResult(i + 1))
                .RunAsync())
            .ThrowIfFail();

        // Assert
        Assert.True(result.IsRight);
        Assert.Equal(2, result.RightSpan()[0]);
    }
    
    [Fact]
    public async Task WhenDoIfRightOnEffOfRight_ShouldDoAction()
    {
        // Arrange
        var eff = SuccessEff(Right<string, int>(1));
        var number = 0;
        var action = new Action<int>(i => number = i);
        
        // Act
        var result =
            (await eff.DoIfRight(action)
                .RunAsync())
            .ThrowIfFail();

        // Assert
        Assert.True(result.IsRight);
        Assert.Equal(1, result.RightSpan()[0]);
        Assert.Equal(1, number);
    }
    
    [Fact]
    public async Task WhenDoIfRightOnEffOfLeft_ShouldNotDoAction()
    {
        // Arrange
        var eff = SuccessEff(Left<string, int>("Something wrong"));
        var number = 0;
        var action = new Action<int>(i => number = i);
        
        // Act
        _ = (await eff.DoIfRight(action)
                .RunAsync())
            .ThrowIfFail();

        // Assert
        Assert.Equal(0, number);
    }
    
    [Fact]
    public async Task WhenDoIfRightAsyncOnEffOfRight_ShouldDoAction()
    {
        // Arrange
        var eff = SuccessEff(Right<string, int>(1));
        var number = 0;
        var actionFunc = new Func<int, ValueTask<Unit>>(
            i =>
            {
                number = i;

                return ValueTask.FromResult(unit);
            });
        
        // Act
        _ =
            (await eff.DoIfRightAsync(actionFunc)
                .RunAsync())
            .ThrowIfFail();

        // Assert
        Assert.Equal(1, number);
    }
    
    [Fact]
    public async Task WhenDoIfRightAsyncOnEffOfLeft_ShouldNotDoAction()
    {
        // Arrange
        var eff = SuccessEff(Left<string, int>("Something wrong"));
        var number = 0;
        var actionFunc = new Func<int, ValueTask<Unit>>(
            i =>
            {
                number = i;

                return ValueTask.FromResult(unit);
            });
        
        // Act
        _ =
            (await eff.DoIfRightAsync(actionFunc)
                .RunAsync())
            .ThrowIfFail();

        // Assert
        Assert.Equal(0, number);
    }
}