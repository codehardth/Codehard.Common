using static Codehard.Functional.Prelude;

namespace Codehard.Functional.Tests;

public class ParallelTests
{
    public class LogifyStub
    {
        private readonly SemaphoreSlim semaphore;

        public LogifyStub()
        {
            this.semaphore = new SemaphoreSlim(1);
        }

        public int InvokeCount { get; private set; }

        public async Task<Unit> LogAsync(object? value)
        {
            ArgumentNullException.ThrowIfNull(value);

            await this.semaphore.WaitAsync();

            Console.WriteLine(value);

            this.InvokeCount++;

            this.semaphore.Release();

            return unit;
        }
    }

    [Theory]
    [InlineData(10)]
    [InlineData(100)]
    [InlineData(1000)]
    public async Task WhenIterateMultipleEffectsInParallel_ShouldSucceed(int count)
    {
        // Arrange
        var logger = new LogifyStub();

        var affs =
            Enumerable.Range(0, count)
                .Select(i => liftEff(async () => await logger.LogAsync(i)))
                .ToArray();

        // Act
        var fin = await IterParallel(affs).RunAsync();

        // Assert
        Assert.Equal(count, logger.InvokeCount);
        Assert.True(fin.IsSucc);
    }

    [Fact]
    public async Task WhenIterateMultipleEffectsInParallel_ThenExceptionOccurred_ShouldFail()
    {
        // Arrange
        var logger = new LogifyStub();

        var affs =
            Enumerable.Range(0, 3)
                .Select(_ => liftEff(async () => await logger.LogAsync(null)))
                .ToArray();

        // Act
        var fin = await IterParallel(affs).RunAsync();

        // Assert
        Assert.True(fin.IsFail);
    }
}