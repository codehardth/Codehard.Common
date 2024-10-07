namespace Codehard.Functional.Tests;

public class EffWithExtTests
{
    [Fact]
    public async Task WhenUseWith_ShouldMatchFinResultSuccessfully()
    {
        // Arrange
        var aEff = SuccessEff(1);
        var bEff = SuccessEff("1");
            
        // Act
        var abEff = aEff.With(bEff);
        var abFin = await abEff.RunAsync();

        // Assert
        var ab = abFin.ThrowIfFail();
        Assert.Equal(1, ab.Item1);
        Assert.Equal("1", ab.Item2);
    }

    [Fact]
    public async Task WhenUseGuardWithTruePredicate_ShouldMatchFinResultSuccessfully()
    {
        // Arrange
        var aEff = SuccessEff(1);
        var bEff = SuccessEff("1");

        // Act
        var abEff = 
            aEff
                .With(bEff)
                .Guard(
                    ab => ab.Item1 == 1 && ab.Item2 == "1",
                    Error.New(1, "Wrong value!"));

        var abFin = await abEff.RunAsync();

        // Assert
        var ab = abFin.ThrowIfFail();
        Assert.Equal(1, ab.Item1);
        Assert.Equal("1", ab.Item2);
    }

    [Fact]
    public async Task WhenUseGuardMultiArgsWithTruePredicate_ShouldMatchFinResultSuccessfully()
    {
        // Arrange
        var aEff = SuccessEff(1);
        var bEff = SuccessEff("1");

        // Act
        var abEff =
            aEff
                .With(bEff)
                .Guard(
                    (a, b) => a == 1 && b == "1",
                    Error.New(1, "Wrong value!"));

        var abFin = await abEff.RunAsync();

        // Assert
        var ab = abFin.ThrowIfFail();
        Assert.Equal(1, ab.Item1);
        Assert.Equal("1", ab.Item2);
    }

    [Fact]
    public async Task WhenUseGuardWithFalsePredicate_ShouldMatchFinResultFail()
    {
        // Arrange
        var aEff = SuccessEff(1);
        var bEff = SuccessEff("1");

        // Act
        var abEff =
            aEff
                .With(bEff)
                .Guard(
                    ab => ab.Item1 == 2 && ab.Item2 == "2",
                    Error.New(1, "Wrong value!"));

        var abFin = await abEff.RunAsync();

        // Assert
        Assert.True(abFin.IsFail);
    }

    [Fact]
    public async Task WhenUseGuardDeconstructTupleWithFalsePredicate_ShouldMatchFinResultFail()
    {
        // Arrange
        var aEff = SuccessEff(1);
        var bEff = SuccessEff("1");

        // Act
        var abEff =
            aEff
                .With(bEff)
                .Guard(
                    (a, b) => a == 2 && b == "2",
                    Error.New(1, "Wrong value!"));

        var abFin = await abEff.RunAsync();

        // Assert
        Assert.True(abFin.IsFail);
    }
}