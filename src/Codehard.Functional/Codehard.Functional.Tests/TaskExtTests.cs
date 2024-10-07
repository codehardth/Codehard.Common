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
    public async Task WhenRunToUnitEffWithValueTask_ShouldReturnSuccEffOfUnit()
    {
        // Arrange
        ValueTask Task()
        {
            _testOutputHelper.WriteLine("Do something");
            
            return ValueTask.CompletedTask;
        }

        // Act
        var eff = Task().ToEffUnit();
        var result = await eff.RunAsync();

        // Assert
        Assert.Single(result.ToArray());
        Assert.Equal(unit, result.ToArray().First());
    }
    
    [Fact]
    public async Task WhenRunToUnitEffWithValueTask_ShouldReturnFailEffOfUnit()
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
        var eff = Task().ToEffUnit();
        var result = await eff.RunAsync();

        // Assert
        Assert.True(result.IsFail);
        Assert.Throws<Exception>(() => result.ThrowIfFail());
    }
    
    [Fact]
    public async Task WhenRunToUnitEffWithTask_ShouldReturnFailEffOfUnit()
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
        var eff = Task().ToEffUnit();
        var result = await eff.RunAsync();

        // Assert
        Assert.True(result.IsFail);
        Assert.Throws<Exception>(() => result.ThrowIfFail());
    }
    
    [Fact]
    public async Task WhenWarpValueTaskInEffUnit_ShouldReturnSuccEffOfUnit()
    {
        // Arrange
        ValueTask Task()
        {
            _testOutputHelper.WriteLine("Do something");
            
            return ValueTask.CompletedTask;
        }

        // Act
        var eff = EffUnit(async () => await Task());
        var result = await eff.RunAsync();

        // Assert
        Assert.True(result.IsSucc);
    }
    
    [Fact]
    public async Task WhenWarpValueTaskInEffUnit_ShouldReturnFailEffOfUnit()
    {
        // Arrange
        ValueTask Task()
        {
            throw new Exception("Some error");
        }

        // Act
        var eff = EffUnit(async () => await Task());
        var result = await eff.RunAsync();

        // Assert
        Assert.True(result.IsFail);
        Assert.Throws<Exception>(() => result.ThrowIfFail());
    }
    
    [Fact]
    public async Task WhenWrapValueTaskInEffUnit_ShouldReturnSuccEffOfUnit()
    {
        // Arrange
        Task Task()
        {
            throw new Exception("Some error");
        }

        // Act
        var eff = EffUnit(async () => await Task());
        var result = await eff.RunAsync();

        // Assert
        Assert.True(result.IsFail);
        Assert.Throws<Exception>(() => result.ThrowIfFail());
    }
}