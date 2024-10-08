using static Codehard.Functional.Prelude;

namespace Codehard.Functional.Tests;

public class EffOptionPreludeTests
{
    [Fact]
    public async Task WhenCreateEffOptionOnDefaultNullableInt_ShouldReturnedEffOfOption()
    {
        // Act
        var eff = EffOption(() => ValueTask.FromResult(default(int?)));
        var fin = await eff.RunAsync();

        // Assert
        var a = fin.ThrowIfFail();
        Assert.Equal(None, a);
    }

    [Fact]
    public async Task WhenCreateEffOptionOnNullString_ShouldReturnedEffOfOption()
    {
        // Act
        var eff = EffOption(async () => await Task.FromResult(default(string)));
        var fin = await eff.RunAsync();

        // Assert
        var a = fin.ThrowIfFail();
        Assert.Equal(None, a);
    }

    [Fact]
    public async Task WhenCreateEffOptionOnInt_ShouldReturnedEffOfOption()
    {
        // Act
        var eff = EffOption(() => ValueTask.FromResult((int?)1));
        var fin = await eff.RunAsync();

        // Assert
        var a = fin.ThrowIfFail();
        Assert.Equal(Some(1), a);
    }

    [Fact]
    public async Task WhenCreateEffOptionOnInstance_ShouldReturnedEffOfOption()
    {
        // Act
        var eff = EffOption(async () => await Task.FromResult((string?)"1"));
        var fin = await eff.RunAsync();

        // Assert
        var a = fin.ThrowIfFail();
        Assert.Equal(Some("1"), a);
    }
}