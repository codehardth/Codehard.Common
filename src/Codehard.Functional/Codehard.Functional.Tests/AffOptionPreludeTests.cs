﻿using static Codehard.Functional.Prelude;

namespace Codehard.Functional.Tests;

public class AffOptionPreludeTests
{
    [Fact]
    public async Task WhenCreateAffOptionOnDefaultNullableInt_ShouldReturnedAffOfOption()
    {
        // Act
        var aff = AffOption(() => ValueTask.FromResult(default(int?)));
        var fin = await aff.Run();

        // Assert
        var a = fin.ThrowIfFail();
        Assert.Equal(None, a);
    }

    [Fact]
    public async Task WhenCreateAffOptionOnNullString_ShouldReturnedAffOfOption()
    {
        // Act
        var aff = AffOption(async () => await Task.FromResult(default(string)));
        var fin = await aff.Run();

        // Assert
        var a = fin.ThrowIfFail();
        Assert.Equal(None, a);
    }

    [Fact]
    public async Task WhenCreateAffOptionOnInt_ShouldReturnedAffOfOption()
    {
        // Act
        var aff = AffOption(() => ValueTask.FromResult((int?)1));
        var fin = await aff.Run();

        // Assert
        var a = fin.ThrowIfFail();
        Assert.Equal(Some(1), a);
    }

    [Fact]
    public async Task WhenCreateAffOptionOnInstance_ShouldReturnedAffOfOption()
    {
        // Act
        var aff = AffOption(async () => await Task.FromResult((string?)"1"));
        var fin = await aff.Run();

        // Assert
        var a = fin.ThrowIfFail();
        Assert.Equal(Some("1"), a);
    }
}