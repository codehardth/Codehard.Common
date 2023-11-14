using Marten;
using Marten.Services.Json;
using Weasel.Core;

namespace Codehard.Functional.Marten.Tests;

public class QueryableExtensionTests
{
    [Fact]
    public async Task WhenQueryWithSingleOrNoneAff_ShouldReturnResultCorrectly()
    {
        // Arrange
        var store = DocumentStore
            .For("host=localhost;database=marten_testing;password=postgres;username=postgres");
        
        store.Options.AutoCreateSchemaObjects = AutoCreate.All;
        store.Options.DatabaseSchemaName = "sch" + Guid.NewGuid().ToString().Replace("-", string.Empty);
        store.Options.UseDefaultSerialization(
            serializerType: SerializerType.SystemTextJson,
            enumStorage: EnumStorage.AsString);
        
        var session = store.IdentitySession();
        var id = Guid.NewGuid();
        
        const string entityNameValue = "Test";
        
        session.Store(new EntityA
        {
            Id = id,
            Name = entityNameValue
        });

        _ = await session
            .SaveChangesAff()
            .Run();

        // Act
        var fin =
            await
                store.QuerySession()
                     .Query<EntityA>()
                     .Where(a => a.Id == id)
                     .SingleOrNoneAff()
                     .Run();
        
        // Assert
        Assert.True(fin.IsSucc);

        var opt = fin.ThrowIfFail();
        
        Assert.True(opt.IsSome);
        
        var entity = opt.IfNoneUnsafe(() => null);
        
        Assert.Equal(id, entity!.Id);
        Assert.Equal(entityNameValue, entity.Name);
    }
}