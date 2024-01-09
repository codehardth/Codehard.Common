using Codehard.Functional.EntityFramework.Generators;
using Codehard.Functional.EntityFramework.Tests.Compilers;

namespace Codehard.Functional.EntityFramework.Tests;

public class EffDbContextGeneratorTests
{
    [Fact]
    public void DiscriminatedUnionGenerator_WithRecord_ShouldGenerateCorrectly()
    {
        // Arrange
        const string source =
            $@"
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Codehard.Functional.EntityFramework.Tests
{{
    public static class Program
    {{
        public static void Main(string[] args)
        {{
        }}
    }}

    public record Record(int Id, string Name);

    public class MyDbContext : DbContext
    {{
        public DbSet<Record> Records {{ get; }} = null!;

        public Task DoSomethingAsync()
        {{
            throw new NotImplementedException();
        }}

        public Task<int> DoSomethingElseAsync()
        {{
            throw new NotImplementedException();
        }}

        public Task DoSomethingAsync1(CancellationToken cancellationToken = default)
        {{
            throw new NotImplementedException();
        }}

        public Task<int> DoSomethingElseAsync1(CancellationToken cancellationToken = default)
        {{
            throw new NotImplementedException();
        }}

        public Task DoSomethingAsync2(string x, CancellationToken cancellationToken = default)
        {{
            throw new NotImplementedException();
        }}

        public Task<int> DoSomethingElseAsync2(string x, CancellationToken cancellationToken = default)
        {{
            throw new NotImplementedException();
        }}
    }}
}}
";
        // Act
        var result = Compiler<EffDbContextGenerator>.Compile(source);

        // Assert
        Assert.Empty(result.CompilationErrors);
        Assert.Empty(result.GenerationDiagnostics);
    }
}