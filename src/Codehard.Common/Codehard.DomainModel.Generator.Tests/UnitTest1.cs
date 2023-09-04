using Codehard.DomainModel.Generator.Tests.Compilation;

namespace Codehard.DomainModel.Generator.Tests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        // Arrange
        const string source =
            """
            using System;
            using System.Linq.Expressions;
            using Codehard.Common.DomainModel;
            using Codehard.Common.DomainModel.Attributes;

            public delegate bool IsCreatedBefore2022Delegate(MyEntityRoot entity, int value);
            public delegate bool TestBoolDelegate(MyEntityRoot entity);
            public delegate string TestStringDelegate(MyEntityRoot entity);

            public partial class MyEntityRoot : IAggregateRoot<GuidKey>, IDisposable
            {
                public GuidKey Id { get; }
                
                public DateTimeOffset CreatedAt { get; }
                            
                [Specification]
                public Func<Guid, Expression<Func<MyEntityRoot, bool>>> MySpec = id => e => e.Id == id;
                            
                [SpecificationAttribute]
                public Expression<Func<MyEntityRoot, bool>> MySpec2 = e => e.Id == Guid.Empty;
                
                [Specification]
                public Func<object, bool> MySpec3 = o => true;
                
                [Specification]
                public Func<Guid, int, string, Expression<Func<MyEntityRoot, bool>>> MySpec4 = (a, b, c) => e => e.Id == a;
                
                [Specification]
                public IsCreatedBefore2022Delegate IsCreatedBefore2022 = (e, v) => e.CreatedAt.Year < 2022 && v > 100;
                
                [Specification]
                public TestBoolDelegate IsSatisfied = e => true;
                
                [Specification]
                public TestBoolDelegate IsSatisfied2 = e => e.CreatedAt == DateTimeOffset.UtcNow;
            
                public void Dispose()
                {
                }
            }

            public static class Program
            {
                public static void Main(string[] args)
                {
                }
            }
            """;


        // Act
        var result = Compiler.With(new DomainRepositoryGenerator()).Compile(source);

        // Assert
        Assert.Empty(result.CompilationErrors);
    }
}