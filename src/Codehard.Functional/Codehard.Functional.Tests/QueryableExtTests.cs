﻿using System.Linq.Expressions;

namespace Codehard.Functional.Tests;

public class QueryableExtTests
{
    [Fact]
    public void WhenRunWhereIfTrueFromListWithTrueCondition_ShouldReturnNonEmptyList()
    {
        // Arrange
        var list = new [] { 1 };

        // Act
        var res = list
            .AsQueryable()
                .WhereIfTrue(
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
        var res = list
            .AsQueryable()
            .WhereIfTrue(
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
        var res = list
            .AsQueryable()
            .WhereOptional(
                Some<Expression<Func<int, bool>>>(n => n == 1));

        // Assert
        Assert.Single(res);
        Assert.Equal(1, res.First());
    }
    
    [Fact]
    public void WhenRunWhereOptionalFromListOfDateTimeOffsetWithSomePredicate_ShouldReturnNonEmptyList()
    {
        // Arrange
        var date = new DateTimeOffset(2020, 1, 1, 0, 0, 0, TimeSpan.Zero);
        var list = new [] { date };

        // Act
        var res = list
            .AsQueryable()
            .WhereOptional(
                Some(date).ToPredExpr<DateTimeOffset>(n => d => n == d));

        // Assert
        Assert.Single(res);
        Assert.Equal(date, res.First());
    }
    
    [Fact]
    public void WhenRunWhereOptionalFromListWithNonePredicate_ShouldNoneEmptyList()
    {
        // Arrange
        var list = new [] { 1 };

        // Act
        var res = list
            .AsQueryable()
            .WhereOptional(None);

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
        var res = list
            .AsQueryable()
            .SkipOptional(Some(1));

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
        var res = list
            .AsQueryable()
            .SkipOptional(None);

        // Assert
        Assert.Equal(2, res.Count());
        Assert.Equal(1, res.First());
    }
}