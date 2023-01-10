namespace Codehard.Functional.Tests;

public class OptionalExtensionTests
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
}