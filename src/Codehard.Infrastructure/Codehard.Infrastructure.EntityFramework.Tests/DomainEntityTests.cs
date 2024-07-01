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
    public async Task WhenSaveChangesAsync_UsingDomainEventDbContext_ShouldPublishAndClearEvents()
    {
        // Arrange
        var assembly = Assembly.GetExecutingAssembly();
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseSqlite(CreateInMemoryDatabase())
            .Options;

        var loggerMock = new Mock<ILogger<TestDbContext>>();
        var logger = loggerMock.Object;
        
        await using var context = new TestDbContext(
            options,
            builder => builder.ApplyConfigurationsFromAssemblyFor<TestDbContext>(assembly),
            logger);
        
        await context.Database.EnsureCreatedAsync();

        // Act
        var entity = EntityA.Create();
        entity.UpdateValue("New Value");

        await context.As.AddAsync(entity);
        await context.SaveChangesAsync();

        // Assert
        loggerMock.Verify(
            logger =>
                logger.Log(
                    It.Is<LogLevel>(logLevel => logLevel == LogLevel.Information),
                    It.Is<EventId>(eventId => true),
                    It.Is<It.IsAnyType>((@object, @type) =>
                        @object.ToString()!.Contains(nameof(EntityCreatedEvent))),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
        loggerMock.Verify(
            logger =>
                logger.Log(
                    It.Is<LogLevel>(logLevel => logLevel == LogLevel.Information),
                    It.Is<EventId>(eventId => true),
                    It.Is<It.IsAnyType>((@object, @type) =>
                        @object.ToString()!.Contains(nameof(ValueChangedEvent))),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
        Assert.Empty(entity.Events);
    }

    [Fact]
    public async Task WhenSaveChangesAsync_UsingGlobalPublisherFunction_ShouldPublishAndClearEvents()
    {
        // Arrange
        var assembly = Assembly.GetExecutingAssembly();
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseSqlite(CreateInMemoryDatabase())
            .Options;

        var loggerMock = new Mock<ILogger<TestDbContext>>();
        var logger = loggerMock.Object;
        await using var context = new TestDbContext(
            options,
            builder => builder.ApplyConfigurationsFromAssemblyFor<TestDbContext>(assembly),
            dm =>
            {
                // We use LogWarning here to distinct
                // between the global and local (within the DbContext) publisher
                logger.LogWarning(dm.ToString());

                return Task.CompletedTask;
            });
        await context.Database.EnsureCreatedAsync();

        // Act
        var entity = EntityA.Create();
        entity.UpdateValue("New Value");

        await context.As.AddAsync(entity);
        await context.SaveChangesAsync();

        // Assert
        loggerMock.Verify(
            logger =>
                logger.Log(
                    It.Is<LogLevel>(logLevel => logLevel == LogLevel.Warning),
                    It.Is<EventId>(eventId => true),
                    It.Is<It.IsAnyType>((@object, @type) =>
                        @object.ToString()!.Contains(nameof(EntityCreatedEvent))),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
        loggerMock.Verify(
            logger =>
                logger.Log(
                    It.Is<LogLevel>(logLevel => logLevel == LogLevel.Warning),
                    It.Is<EventId>(eventId => true),
                    It.Is<It.IsAnyType>((@object, @type) =>
                        @object.ToString()!.Contains(nameof(ValueChangedEvent))),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
        Assert.Empty(entity.Events);
    }

    [Fact]
    public void WhenSaveChanges_UsingDomainEventDbContext_ShouldPublishAndClearEvents()
    {
        // Arrange
        var assembly = Assembly.GetExecutingAssembly();
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseSqlite(CreateInMemoryDatabase())
            .Options;

        var loggerMock = new Mock<ILogger<TestDbContext>>();
        var logger = loggerMock.Object;
        using var context = new TestDbContext(
            options,
            builder => builder.ApplyConfigurationsFromAssemblyFor<TestDbContext>(assembly),
            logger);
        context.Database.EnsureCreated();

        // Act
        var entity = EntityA.Create();
        entity.UpdateValue("New Value");

        context.As.Add(entity);
        context.SaveChanges();

        // Assert
        loggerMock.Verify(
            logger =>
                logger.Log(
                    It.Is<LogLevel>(logLevel => logLevel == LogLevel.Information),
                    It.Is<EventId>(eventId => true),
                    It.Is<It.IsAnyType>((@object, @type) =>
                        @object.ToString()!.Contains(nameof(EntityCreatedEvent))),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
        loggerMock.Verify(
            logger =>
                logger.Log(
                    It.Is<LogLevel>(logLevel => logLevel == LogLevel.Information),
                    It.Is<EventId>(eventId => true),
                    It.Is<It.IsAnyType>((@object, @type) =>
                        @object.ToString()!.Contains(nameof(ValueChangedEvent))),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
        Assert.Empty(entity.Events);
    }

    [Fact]
    public void WhenSaveChanges_UsingGlobalPublisherFunction_ShouldPublishAndClearEvents()
    {
        // Arrange
        var assembly = Assembly.GetExecutingAssembly();
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseSqlite(CreateInMemoryDatabase())
            .Options;

        var loggerMock = new Mock<ILogger<TestDbContext>>();
        var logger = loggerMock.Object;
        using var context = new TestDbContext(
            options,
            builder => builder.ApplyConfigurationsFromAssemblyFor<TestDbContext>(assembly),
            dm =>
            {
                // We use LogWarning here to distinct
                // between the global and local (within the DbContext) publisher
                logger.LogWarning(dm.ToString());

                return Task.CompletedTask;
            });
        context.Database.EnsureCreated();

        // Act
        var entity = EntityA.Create();
        entity.UpdateValue("New Value");

        context.As.Add(entity);
        context.SaveChanges();

        // Assert
        loggerMock.Verify(
            logger =>
                logger.Log(
                    It.Is<LogLevel>(logLevel => logLevel == LogLevel.Warning),
                    It.Is<EventId>(eventId => true),
                    It.Is<It.IsAnyType>((@object, @type) =>
                        @object.ToString()!.Contains(nameof(EntityCreatedEvent))),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
        loggerMock.Verify(
            logger =>
                logger.Log(
                    It.Is<LogLevel>(logLevel => logLevel == LogLevel.Warning),
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