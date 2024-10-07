using LanguageExt;
using LanguageExt.UnsafeValueAccess;
using Marten;
using Marten.Services.Json;
using Weasel.Core;

namespace Codehard.Functional.Marten.Tests;

public class QueryableExtensionTests
{
    [Fact]
    public async Task WhenQueryWithSingleOrNoneEff_ShouldReturnResultCorrectly()
    {
        // Arrange
        var store = DocumentStore
            .For("host=localhost;database=marten_testing;password=postgres;username=postgres");
        
        store.Options.AutoCreateSchemaObjects = AutoCreate.All;
        store.Options.DatabaseSchemaName = "sch" + Guid.NewGuid().ToString().Replace("-", string.Empty);
        store.Options.UseSystemTextJsonForSerialization(enumStorage: EnumStorage.AsString);
        
        var session = store.IdentitySession();
        var id = Guid.NewGuid();
        
        const string entityNameValue = "Test";
        
        session.Store(new EntityA
        {
            Id = id,
            Name = entityNameValue
        });

        _ = await session
            .SaveChangesEff()
            .RunAsync();

        // Act
        var fin =
            await
                store.QuerySession()
                     .Query<EntityA>()
                     .Where(a => a.Id == id)
                     .SingleOrNoneEff()
                     .RunAsync();
        
        // Assert
        Assert.True(fin.IsSucc);

        var opt = fin.ThrowIfFail();
        
        Assert.True(opt.IsSome);
        
        var entity = opt.ValueUnsafe();
        
        Assert.Equal(id, entity!.Id);
        Assert.Equal(entityNameValue, entity.Name);
    }
}