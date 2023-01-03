namespace Codehard.Functional.Tests;

public class AffEitherExtensionTests
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
        Assert.Equal("Something wrong", result.LeftAsEnumerable().First());
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
        Assert.Equal(1, result.RightAsEnumerable().First());
    }
    
    [Fact]
    public async Task WhenCallMapNoneToLeftOnAffOfNone_ShouldGetAffOfLeft()
    {
        // Arrange
        var affOpt = SuccessAff(Option<int>.None);

        // Act
        var result = 
            (await affOpt.MapNoneToLeft(() => "Something wrong")
                         .Run())
            .ThrowIfFail();

        // Assert
        Assert.True(result.IsLeft);
        Assert.Equal("Something wrong", result.LeftAsEnumerable().First());
    }
    
    [Fact]
    public async Task WhenCallMapNoneToLeftOnAffOfSome_ShouldGetAffOfRight()
    {
        // Arrange
        var affOpt = SuccessAff(Option<int>.Some(1));

        // Act
        var result = 
            (await affOpt.MapNoneToLeft(() => "Something wrong")
                         .Run())
            .ThrowIfFail();

        // Assert
        Assert.True(result.IsRight);
        Assert.Equal(1, result.RightAsEnumerable().First());
    }
    
    [Fact]
    public async Task WhenGuardToLeftOnAffOfSomeWithTruePredicate_ShouldGetAffOfRight()
    {
        // Arrange
        var aff = SuccessAff(1);

        // Act
        var result =
            (await aff.GuardToLeft(
                        i => i > 0,
                        () => "Something wrong")
                      .Run())
            .ThrowIfFail();
        // Assert
        Assert.True(result.IsRight);
        Assert.Equal(1, result.RightAsEnumerable().First());
    }
    
    [Fact]
    public async Task WhenGuardToLeftOnAffWithFalsePredicate_ShouldGetAffOfLeft()
    {
        // Arrange
        var aff = SuccessAff(1);

        // Act
        var result =
            (await aff.GuardToLeft(
                    i => i > 2,
                    () => "Something wrong")
                .Run())
            .ThrowIfFail();

        // Assert
        Assert.True(result.IsLeft);
        Assert.Equal("Something wrong", result.LeftAsEnumerable().First());
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
        Assert.Equal(1, result.RightAsEnumerable().First());
    }
    
    [Fact]
    public async Task WhenGuardToLeftOnAffWithTruePredicate_ShouldGetAffOfRight()
    {
        // Arrange
        var aff = SuccessAff(1);

        // Act
        var result =
            (await aff.GuardToLeft(
                    i => i > 0,
                    () => "Something wrong")
                .Run())
            .ThrowIfFail();

        // Assert
        Assert.True(result.IsRight);
        Assert.Equal(1, result.RightAsEnumerable().First());
    }
    
    [Fact]
    public async Task WhenGuardEitherOnAffOfRightWithFalsePredicate_ShouldGetAffOfLeft()
    {
        // Arrange
        var aff = SuccessAff(Right<string, int>(1));

        // Act
        var result =
            (await aff.GuardEither(
                    i => i > 2,
                    () => "Something wrong")
                .Run())
            .ThrowIfFail();

        // Assert
        Assert.True(result.IsLeft);
        Assert.Equal("Something wrong", result.LeftAsEnumerable().First());
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
        Assert.Equal("Something wrong", result.LeftAsEnumerable().First());
    }
    
    [Fact]
    public async Task WhenGuardEitherAsyncOnAffOfRightWithFalsePredicateAsync_ShouldGetAffOfLeft()
    {
        // Arrange
        var aff = SuccessAff(Right<string, int>(1));

        // Act
        var result =
            (await aff.GuardEitherAsync(
                    i => ValueTask.FromResult(i > 2),
                    _ => "Something wrong")
                .Run())
            .ThrowIfFail();

        // Assert
        Assert.True(result.IsLeft);
        Assert.Equal("Something wrong", result.LeftAsEnumerable().First());
    }
    
    [Fact]
    public async Task WhenGuardEitherAsyncOnAffOfRightWithFalsePredicateAndLeftAsync_ShouldGetAffOfLeft()
    {
        // Arrange
        var aff = SuccessAff(Right<string, int>(1));

        // Act
        var result =
            (await aff.GuardEitherAsync(
                    i => i > 2,
                    _ => ValueTask.FromResult("Something wrong"))
                .Run())
            .ThrowIfFail();

        // Assert
        Assert.True(result.IsLeft);
        Assert.Equal("Something wrong", result.LeftAsEnumerable().First());
    }
    
    [Fact]
    public async Task WhenGuardEitherAsyncOnAffOfRightWithFalsePredicateAsyncAndLeftAsync_ShouldGetAffOfLeft()
    {
        // Arrange
        var aff = SuccessAff(Right<string, int>(1));

        // Act
        var result =
            (await aff.GuardEitherAsync(
                    i => ValueTask.FromResult(i > 2),
                    _ => ValueTask.FromResult("Something wrong"))
                .Run())
            .ThrowIfFail();

        // Assert
        Assert.True(result.IsLeft);
        Assert.Equal("Something wrong", result.LeftAsEnumerable().First());
    }

    [Fact]
    public async Task WhenMapRightOnAffOfRight_ShouldMapToNewValue()
    {
        // Arrange
        var aff = SuccessAff(Right<string, int>(1));

        // Act
        var result =
            (await aff.MapRight(i => i + 1)
                      .Run())
            .ThrowIfFail();

        // Assert
        Assert.True(result.IsRight);
        Assert.Equal(2, result.RightAsEnumerable().First());
    }
    
    [Fact]
    public async Task WhenMapRightAsyncOnAffOfRight_ShouldMapToNewValue()
    {
        // Arrange
        var aff = SuccessAff(Right<string, int>(1));

        // Act
        var result =
            (await aff.MapRightAsync(i => ValueTask.FromResult(i + 1))
                .Run())
            .ThrowIfFail();

        // Assert
        Assert.True(result.IsRight);
        Assert.Equal(2, result.RightAsEnumerable().First());
    }
    
    [Fact]
    public async Task WhenMapRightAsyncOnEffOfRight_ShouldMapToNewValue()
    {
        // Arrange
        var eff = SuccessEff(Right<string, int>(1));

        // Act
        var result =
            (await eff.MapRightAsync(i => ValueTask.FromResult(i + 1))
                .Run())
            .ThrowIfFail();

        // Assert
        Assert.True(result.IsRight);
        Assert.Equal(2, result.RightAsEnumerable().First());
    }
    
    [Fact]
    public async Task WhenDoIfRightOnAffOfRight_ShouldDoAction()
    {
        // Arrange
        var aff = SuccessAff(Right<string, int>(1));
        var number = 0;
        var action = new Action<int>(i => number = i);
        
        // Act
        var result =
            (await aff.DoIfRight(action)
                .Run())
            .ThrowIfFail();

        // Assert
        Assert.True(result.IsRight);
        Assert.Equal(1, result.RightAsEnumerable().First());
        Assert.Equal(1, number);
    }
    
    [Fact]
    public async Task WhenDoIfRightOnAffOfLeft_ShouldNotDoAction()
    {
        // Arrange
        var aff = SuccessAff(Left<string, int>("Something wrong"));
        var number = 0;
        var action = new Action<int>(i => number = i);
        
        // Act
        var result =
            (await aff.DoIfRight(action)
                .Run())
            .ThrowIfFail();

        // Assert
        Assert.Equal(0, number);
    }
    
    [Fact]
    public async Task WhenDoIfRightAsyncOnAffOfRight_ShouldDoAction()
    {
        // Arrange
        var aff = SuccessAff(Right<string, int>(1));
        var number = 0;
        var actionFunc = new Func<int, ValueTask<Unit>>(
            i =>
            {
                number = i;

                return ValueTask.FromResult(unit);
            });
        
        // Act
        _ =
            (await aff.DoIfRightAsync(actionFunc)
                .Run())
            .ThrowIfFail();

        // Assert
        Assert.Equal(1, number);
    }
    
    [Fact]
    public async Task WhenDoIfRightAsyncOnAffOfLeft_ShouldNotDoAction()
    {
        // Arrange
        var aff = SuccessAff(Left<string, int>("Something wrong"));
        var number = 0;
        var actionFunc = new Func<int, ValueTask<Unit>>(
            i =>
            {
                number = i;

                return ValueTask.FromResult(unit);
            });
        
        // Act
        _ =
            (await aff.DoIfRightAsync(actionFunc)
                .Run())
            .ThrowIfFail();

        // Assert
        Assert.Equal(0, number);
    }
}