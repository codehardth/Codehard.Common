using System.Reflection;
using Codehard.Infrastructure.EntityFramework.Extensions;
using Codehard.Infrastructure.EntityFramework.Tests.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Codehard.Infrastructure.EntityFramework.Tests;

public class DomainEntityTests
{
    [Fact]
    public async Task WhenSaveChanges_UsingUnitOfWork_ShouldPublishAndClearEvents()
    {
        // Arrange
        var assembly = Assembly.GetExecutingAssembly();
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseSqlite(CreateInMemoryDatabase())
            .Options;
        await using var context = new TestDbContext(
            options,
            builder => builder.ApplyConfigurationsFromAssemblyFor<TestDbContext>(assembly));
        await context.Database.EnsureCreatedAsync();

        var loggerMock = new Mock<ILogger<TestUnitOfWork>>();
        var unitOfWork = new TestUnitOfWork(loggerMock.Object, context);

        // Act
        var entity = EntityA.Create();
        entity.UpdateValue("New Value");

        await unitOfWork.Entities.AddAsync(entity);
        await unitOfWork.SaveChangesAsync();

        // Assert
        loggerMock.Verify(
            logger =>
                logger.Log(
                    It.Is<LogLevel>(logLevel => true),
                    It.Is<EventId>(eventId => true),
                    It.Is<It.IsAnyType>((@object, @type) =>
                        @object.ToString()!.Contains(nameof(EntityCreatedEvent))),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
        loggerMock.Verify(
            logger =>
                logger.Log(
                    It.Is<LogLevel>(logLevel => true),
                    It.Is<EventId>(eventId => true),
                    It.Is<It.IsAnyType>((@object, @type) =>
                        @object.ToString()!.Contains(nameof(ValueChangedEvent))),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
        Assert.Empty(entity.Events);
    }

    private static SqliteConnection CreateInMemoryDatabase()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();
        return connection;
    }
}