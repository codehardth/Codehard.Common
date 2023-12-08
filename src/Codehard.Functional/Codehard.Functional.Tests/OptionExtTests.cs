namespace Codehard.Functional.Tests;

public class OptionalExtTests
{
    [Fact]
    public void WhenRunWhereOptionalFromListWithSomeTruePredicate_ShouldReturnNonEmptyList()
    {
        // Arrange
        var list = new [] { 1, 2 };

        // Act
        var predicateOpt =
            Some(1).ToPredicate(val => x => x == val);
        
        var res = list.WhereOptional(predicateOpt);

        // Assert
        Assert.Single(res);
        Assert.Equal(1, res.First());
    }
    
    [Fact]
    public void WhenRunWhereOptionalFromListWithSomeFalsePredicate_ShouldReturnNonEmptyList()
    {
        // Arrange
        var list = new [] { 1, 2 };

        // Act
        var predicateOpt =
            Some(0).ToPredicate(val => x => x == val);
        
        var res = list.WhereOptional(predicateOpt);

        // Assert
        Assert.Empty(res);
    }
    
    [Fact]
    public void WhenRunWhereOptionalFromListWithNonePredicate_ShouldReturnNonEmptyList()
    {
        // Arrange
        var list = new [] { 1 };

        // Act
        var predicateOpt =
            Option<int>.None.ToPredicate(val => x => x == val);
        
        var res = list.WhereOptional(predicateOpt);

        // Assert
        Assert.Single(res);
        Assert.Equal(1, res.First());
    }
    
    [Fact]
    public void WhenRunWhereOptionalFromListWithSomeEqPredExpr_ShouldReturnNonEmptyList()
    {
        // Arrange
        var list = new [] { 1, 2 };

        // Act
        var res = list
            .AsQueryable()
            .WhereOptional(Some(1).ToEqPredExpr());

        // Assert
        Assert.Single(res);
        Assert.Equal(1, res.First());
    }
    
    [Fact]
    public async Task IfSomeAff_ExecutesFunction_WhenOptionContainsValue()
    {
        // Arrange
        var optional = Option<int>.Some(5);
        var valueToChange = 0;

        // Act
        var aff =
            optional.IfSomeAff(
                num =>
                    Eff(() =>
                    {
                        valueToChange = num;
                        return Unit.Default;
                    }).ToAff());
        
        _ = await aff.Run();

        // Assert
        Assert.Equal(5, valueToChange);
    }

    [Fact]
    public void IfSomeAff_ReturnsUnitAff_WhenOptionIsEmpty()
    {
        // Arrange
        var optional = Option<int>.None;

        // Act
        var result = optional.IfSomeAff(_ => throw new Exception("This should not be called"));

        // Assert
        Assert.Equal(unitAff, result);
    }
    
    [Fact]
    public void IfSomeEff_ExecutesFunction_WhenOptionContainsValue()
    {
        // Arrange
        var optional = Option<int>.Some(5);
        var valueToChange = 0;

        // Act
        var eff =
            optional.IfSomeEff(
                num =>
                    Eff(() =>
                    {
                        valueToChange = num;
                        return Unit.Default;
                    }));
        
        _ = eff.Run();

        // Assert
        Assert.Equal(5, valueToChange);
    }

    [Fact]
    public void IfSomeEff_ReturnsUnitEff_WhenOptionIsEmpty()
    {
        // Arrange
        var optional = Option<int>.None;
        var expected = unitEff;

        // Act
        var result = optional.IfSomeEff(_ => throw new Exception("This should not be called"));

        // Assert
        Assert.Equal(expected, result);
    }
}