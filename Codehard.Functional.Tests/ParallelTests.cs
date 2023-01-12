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

        public async Task<Unit> LogAsync(object value)
        {
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
    public async Task WhenRunMultipleEffectInParallel_ShouldSucceed(int count)
    {
        // Arrange
        var logger = new LogifyStub();

        var affs =
            Enumerable.Range(0, count)
                .Map(i => Aff(async () => await logger.LogAsync(i)))
                .ToArray();

        // Act
        _ = await RunParallel(affs).Run();

        // Assert
        Assert.Equal(count, logger.InvokeCount);
    }
}