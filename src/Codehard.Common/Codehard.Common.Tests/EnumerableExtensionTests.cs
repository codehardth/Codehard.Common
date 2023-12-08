using Codehard.Common.Extensions;

namespace Codehard.Common.Tests;

public class EnumerableExtensionTests
{
    [Fact]
    public void WhenRunWhereIfFromListWithTrueCondition_ShouldReturnNonEmptyList()
    {
        // Arrange
        var list = new [] { 1 };

        // Act
        var res = list.WhereIf(
            true,
            n => n == 1);

        // Assert
        Assert.Single(res);
        Assert.Equal(1, res.First());
    }
    
    [Fact]
    public void WhenRunWhereIfFromListWithFalseCondition_ShouldReturnEmptyList()
    {
        // Arrange
        var list = new [] { 1 };

        // Act
        var res = list.WhereIf(
            false,
            n => n == 1,
            n => n == 0);

        // Assert
        Assert.Empty(res);
    }
    
    [Fact]
    public void SelectValueTypeOnlyNotNull_ShouldReturnOnlyNonNullValues_WhenValueTypeIsProvided()
    {
        // Arrange
        var source = new List<int?> { 1, null, 2, null, 3 };
        Func<int?, int?> selector = x => x;

        // Act
        var result = EnumerableExtensions.SelectOnlyNotNull(source, selector);

        // Assert
        Assert.Equal(new[] { 1, 2, 3 }, result);
    }

    [Fact]
    public void SelectValueTypeOnlyNotNull_ShouldReturnEmpty_WhenAllValuesAreNull()
    {
        // Arrange
        var source = new List<int?> { null, null, null };
        Func<int?, int?> selector = x => x;

        // Act
        var result = EnumerableExtensions.SelectOnlyNotNull(source, selector);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void SelectValueTypeOnlyNotNull_ShouldReturnEmpty_WhenSourceIsEmpty()
    {
        // Arrange
        var source = new List<int?>();
        Func<int?, int?> selector = x => x;

        // Act
        var result = EnumerableExtensions.SelectOnlyNotNull(source, selector);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void SelectValueTypeOnlyNotNull_ShouldTransformAndFilter_WhenSelectorIsProvided()
    {
        // Arrange
        var source = new List<int?> { 1, null, 2, null, 3 };
        Func<int?, string?> selector = x => x.HasValue ? x.Value.ToString() : null;

        // Act
        var result = EnumerableExtensions.SelectOnlyNotNull(source, selector);

        // Assert
        Assert.Equal(new[] { "1", "2", "3" }, result);
    }
    
    [Fact]
    public void SelectReferenceTypeOnlyNotNull_ShouldReturnOnlyNonNullValues_WhenReferenceTypeIsProvided()
    {
        // Arrange
        var source = new List<string?> { "one", null, "two", null, "three" };
        Func<string?, string?> selector = x => x;

        // Act
        var result = EnumerableExtensions.SelectOnlyNotNull(source, selector);

        // Assert
        Assert.Equal(new[] { "one", "two", "three" }, result);
    }

    [Fact]
    public void SelectReferenceTypeOnlyNotNull_ShouldReturnEmpty_WhenAllValuesAreNull()
    {
        // Arrange
        var source = new List<string?> { null, null, null };
        Func<string?, string?> selector = x => x;

        // Act
        var result = EnumerableExtensions.SelectOnlyNotNull(source, selector);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void SelectReferenceTypeOnlyNotNull_ShouldReturnEmpty_WhenSourceIsEmpty()
    {
        // Arrange
        var source = new List<string?>();
        Func<string?, string?> selector = x => x;

        // Act
        var result = EnumerableExtensions.SelectOnlyNotNull(source, selector);

        // Assert
        Assert.Empty(result);
    }
}