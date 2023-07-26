namespace Codehard.Functional.Tests;

public class EnumerableExtTests
{
    [Fact]
    public void WhenCallFirstOrNoneOrFailEffWithTruePredicate_ShouldReturnSome()
    {
        // Arrange
        var emptyList = new [] { 1 };

        // Act
        var item =
            emptyList.FirstOrNone(i => i == 1);

        // Assert
        Assert.Equal(1, item);
    }
    
    [Fact]
    public void WhenCallFirstOrNoneOrFailEffWithFalsePredicate_ShouldReturnNone()
    {
        // Arrange
        var emptyList = new [] { 0 };

        // Act
        var item =
            emptyList.FirstOrNone(i => i == 1);

        // Assert
        Assert.Equal(None, item);
    }
    
    [Fact]
    public void WhenCallFirstOrNoneOrFailEffWithPredicateOnEmptySeq_ShouldReturnNone()
    {
        // Arrange
        var emptySeq = Enumerable.Empty<int>();

        // Act
        var item =  emptySeq.FirstOrNone(i => i == 1);

        // Assert
        Assert.Equal(None, item);
    }
    
    [Fact]
    public void WhenCallSingleOrNoneOrFailEffFromListContainSingleItem_ShouldReturnAnItem()
    {
        // Arrange
        var list = new [] { 1 };

        // Act
        var fin = list.SingleFin();

        // Assert
        Assert.True(fin.IsSucc);
        Assert.Equal(1, fin.ThrowIfFail());
    }
    
    [Fact]
    public void WhenCallSingleOrNoneOrFailEffFromListContainNothing_ShouldReturnNone()
    {
        // Arrange
        var emptySeq = Enumerable.Empty<int>();

        // Act
        var fin = emptySeq.SingleFin();

        // Assert
        Assert.True(fin.IsSucc);
        Assert.Equal(None, fin.ThrowIfFail());
    }
    
    [Fact]
    public void WhenCallSingleOrNoneOrFailEffFromListContainDuplicatedValues_ShouldReturnFail()
    {
        // Arrange
        var list = new [] { 1, 1 };

        // Act
        var fin = list.SingleFin();

        // Assert
        Assert.True(fin.IsFail);
    }
    
    [Fact]
    public void WhenCallSingleOrNoneOrFailEffWithTruePredicate_ShouldReturnItem()
    {
        // Arrange
        var list = new [] { 1 };

        // Act
        var fin = list.SingleFin(i => i == 1);

        // Assert
        Assert.True(fin.IsSucc);
        Assert.Equal(1, fin.ThrowIfFail());
    }
    
    [Fact]
    public void WhenCallSingleOrNoneOrFailEffWithFalsePredicate_ShouldReturnNone()
    {
        // Arrange
        var list = new [] { 0 };

        // Act
        var fin = list.SingleFin(i => i == 1);

        // Assert
        Assert.True(fin.IsSucc);
        Assert.Equal(None, fin.ThrowIfFail());
    }
    
    [Fact]
    public void WhenCallSingleOrNoneOrFailEffWithMultipleMatched_ShouldReturnFail()
    {
        // Arrange
        var list = new [] { 1, 1 };

        // Act
        var fin = list.SingleFin(i => i == 1);

        // Assert
        Assert.True(fin.IsFail);
    }

    [Fact]
    public void WhenRunSingleOrDefaultFromListContainSingleItem_ShouldReturnAnItem()
    {
        // Arrange
        var list = new [] { 1 };

        // Act
        var item = list.SingleOrNone();

        // Assert
        Assert.Equal(1, item);
    }
    
    [Fact]
    public void WhenRunAllIfAnyFromListContainSingleItem_ShouldReturnSomeOfTrue()
    {
        // Arrange
        var list = new [] { 1 };

        // Act
        var res = list.AllIfAny(i => i > 0);

        // Assert
        Assert.Equal(Some(true), res);
    }
    
    [Fact]
    public void WhenRunAllIfAnyWithFalsePredicateFromListContainSingleItem_ShouldReturnSomeOfFalse()
    {
        // Arrange
        var list = new [] { 1 };

        // Act
        var res = list.AllIfAny(i => i > 1);

        // Assert
        Assert.Equal(Some(false), res);
    }
    
    [Fact]
    public void WhenRunAllIfAnyFromListContainNoItem_ShouldReturnNone()
    {
        // Arrange
        var list = new int [] { };

        // Act
        var res = list.AllIfAny(i => i > 1);

        // Assert
        Assert.Equal(None, res);
    }
    
    [Fact]
    public void WhenRunWhereIfTrueFromListWithTrueCondition_ShouldReturnNonEmptyList()
    {
        // Arrange
        var list = new [] { 1 };

        // Act
        var res = list.WhereIfTrue(
            true,
            n => n == 1);

        // Assert
        Assert.Single(res);
        Assert.Equal(1, res.First());
    }
    
    [Fact]
    public void WhenRunWhereIfTrueFromListWithFalseCondition_ShouldReturnEmptyList()
    {
        // Arrange
        var list = new [] { 1 };

        // Act
        var res = list.WhereIfTrue(
            false,
            n => n == 1,
            n => n == 0);

        // Assert
        Assert.Empty(res);
    }
    
    [Fact]
    public void WhenRunWhereOptionalFromListWithSomePredicate_ShouldReturnNonEmptyList()
    {
        // Arrange
        var list = new [] { 1 };

        // Act
        var res = list.WhereOptional(
            Some<Func<int, bool>>(n => n == 1));

        // Assert
        Assert.Single(res);
        Assert.Equal(1, res.First());
    }
    
    [Fact]
    public void WhenRunWhereOptionalFromListWithNonePredicate_ShouldNoneEmptyList()
    {
        // Arrange
        var list = new [] { 1 };

        // Act
        var res = list.WhereOptional(None);

        // Assert
        Assert.Single(res);
        Assert.Equal(1, res.First());
    }
    
    [Fact]
    public void WhenRunSkipOptionalFromListWithSomeCount_ShouldReturnSmallerList()
    {
        // Arrange
        var list = new [] { 1, 2 };

        // Act
        var res = list.SkipOptional(Some(1));

        // Assert
        Assert.Single(res);
        Assert.Equal(2, res.First());
    }
    
    [Fact]
    public void WhenRunSkipOptionalFromListWithNone_ShouldReturnUnchangedList()
    {
        // Arrange
        var list = new [] { 1, 2 };

        // Act
        var res = list.SkipOptional(None);

        // Assert
        Assert.Equal(2, res.Count());
        Assert.Equal(1, res.First());
    }
}