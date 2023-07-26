using Xunit.Abstractions;
using static Codehard.Functional.Prelude;

namespace Codehard.Functional.Tests;

public class TaskExtTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public TaskExtTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task WhenRunToUnitAffWithValueTask_ShouldReturnSuccAffOfUnit()
    {
        // Arrange
        ValueTask Task()
        {
            _testOutputHelper.WriteLine("Do something");
            
            return ValueTask.CompletedTask;
        }

        // Act
        var aff = Task().ToAffUnit();
        var result = await aff.Run();

        // Assert
        Assert.Single(result.Succs());
        Assert.Equal(unit, result.Succs().First());
    }
    
    [Fact]
    public async Task WhenRunToUnitAffWithValueTask_ShouldReturnFailAffOfUnit()
    {
        // Arrange
        // Do not remove async keyword!
#pragma warning disable CS1998
        async ValueTask Task()
#pragma warning restore CS1998
        {
            throw new Exception("Some error");
        }

        // Act
        var aff = Task().ToAffUnit();
        var result = await aff.Run();

        // Assert
        Assert.True(result.IsFail);
        Assert.Throws<Exception>(() => result.ThrowIfFail());
    }
    
    [Fact]
    public async Task WhenRunToUnitAffWithTask_ShouldReturnFailAffOfUnit()
    {
        // Arrange
        // Do not remove async keyword!
#pragma warning disable CS1998
        async Task Task()
#pragma warning restore CS1998
        {
            throw new Exception("Some error");
        }

        // Act
        var aff = Task().ToAffUnit();
        var result = await aff.Run();

        // Assert
        Assert.True(result.IsFail);
        Assert.Throws<Exception>(() => result.ThrowIfFail());
    }
    
    [Fact]
    public async Task WhenWarpValueTaskInAffUnit_ShouldReturnSuccAffOfUnit()
    {
        // Arrange
        ValueTask Task()
        {
            _testOutputHelper.WriteLine("Do something");
            
            return ValueTask.CompletedTask;
        }

        // Act
        var aff = AffUnit(async () => await Task());
        var result = await aff.Run();

        // Assert
        Assert.True(result.IsSucc);
    }
    
    [Fact]
    public async Task WhenWarpValueTaskInAffUnit_ShouldReturnFailAffOfUnit()
    {
        // Arrange
        ValueTask Task()
        {
            throw new Exception("Some error");
        }

        // Act
        var aff = AffUnit(async () => await Task());
        var result = await aff.Run();

        // Assert
        Assert.True(result.IsFail);
        Assert.Throws<Exception>(() => result.ThrowIfFail());
    }
    
    [Fact]
    public async Task WhenWrapValueTaskInAffUnit_ShouldReturnSuccAffOfUnit()
    {
        // Arrange
        Task Task()
        {
            throw new Exception("Some error");
        }

        // Act
        var aff = AffUnit(async () => await Task());
        var result = await aff.Run();

        // Assert
        Assert.True(result.IsFail);
        Assert.Throws<Exception>(() => result.ThrowIfFail());
    }
}