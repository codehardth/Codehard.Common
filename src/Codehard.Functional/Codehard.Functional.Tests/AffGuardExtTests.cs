using static Codehard.Functional.Prelude;

namespace Codehard.Functional.Tests;

public class AffGuardExtTests
{
    [Fact]
    public async Task WhenGuardNoneOnSomeValue_ShouldStayOnSuccCase()
    {
        // Act
        var aff =
            EffOption(() => ValueTask.FromResult((int?)1))
                .GuardNotNone("There is something wrong");
            
        var fin = await aff.RunAsync();

        // Assert
        var a = fin.ThrowIfFail();

        Assert.Equal(1, a);
    }

    [Fact]
    public async Task WhenGuardNoneOnNullValue_ShouldGoToFailCase()
    {
        // Act
        var aff =
            EffOption(() => ValueTask.FromResult((int?)null))
                .GuardNotNone("There is no value");

        var fin = await aff.RunAsync();

        // Assert
        Assert.False(fin.IsSucc);
    }
}