namespace Codehard.Functional.Tests;

public class EnumerableExtensionTests
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
        var fin = list.SingleOrNoneOrFailFin();

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
        var fin = emptySeq.SingleOrNoneOrFailFin();

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
        var fin = list.SingleOrNoneOrFailFin();

        // Assert
        Assert.True(fin.IsFail);
    }
    
    [Fact]
    public void WhenCallSingleOrNoneOrFailEffWithTruePredicate_ShouldReturnItem()
    {
        // Arrange
        var list = new [] { 1 };

        // Act
        var fin = list.SingleOrNoneOrFailFin(i => i == 1);

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
        var fin = list.SingleOrNoneOrFailFin(i => i == 1);

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
        var fin = list.SingleOrNoneOrFailFin(i => i == 1);

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
}