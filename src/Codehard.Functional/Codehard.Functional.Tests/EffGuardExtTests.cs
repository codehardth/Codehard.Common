using static Codehard.Functional.Prelude;

namespace Codehard.Functional.Tests;

public class EffGuardExtTests
{
    [Fact]
    public void WhenGuardNoneOnSomeValue_ShouldStayOnSuccCase()
    {
        // Act
        var eff =
            EffOption(() => (int?)1)
                .GuardNotNone("There is something wrong");
            
        var fin = eff.Run();

        // Assert
        var a = fin.ThrowIfFail();

        Assert.Equal(1, a);
    }

    [Fact]
    public void WhenGuardNoneOnNullValue_ShouldGoToFailCase()
    {
        // Act
        var eff =
            EffOption(() => (int?)null)
                .GuardNotNone("There is no value");

        var fin = eff.Run();

        // Assert
        Assert.False(fin.IsSucc);
    }
}