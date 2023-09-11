using Codehard.DomainModel.Generator.Tests.Compilation;

namespace Codehard.DomainModel.Generator.Tests;

public class UnitTest1
{
    [Fact]
    public void WhenGenerate_Func_WhichReturnsBoolean_ShouldGenerateCorrectly()
    {
        // Arrange
        const string source =
            """
            using System;
            using System.Linq.Expressions;
            using Codehard.Common.DomainModel;
            using Codehard.Common.DomainModel.Attributes;

            public class MyEntityRootKey : IEntityKey
            {
                public Guid Value { get; }
            }

            public partial class MyEntityRoot : IAggregateRoot<MyEntityRootKey>, IDisposable
            {
                public MyEntityRootKey Id { get; }
            
                public DateTimeOffset CreatedAt { get; }
            
                [Specification]
                public Func<MyEntityRoot, bool> Func1 = e => true;
            
                [Specification]
                public Func<Guid, int, string, Expression<Func<MyEntityRoot, bool>>> Func2 =
                    (a, b, c) => e => e.Id.Value == a;
            
                public void Dispose()
                {
                }
            }

            public static class Program
            {
                public static void Main(string[] args)
                {
                    var spec1 = new MyEntityRoot.Func1Specification();
                    var spec2 = new MyEntityRoot.Func2Specification(Guid.Empty, 0, string.Empty);
                }
            }
            """;


        // Act
        var result = Compiler.With(new DomainRepositoryGenerator()).Compile(source);

        // Assert
        Assert.Empty(result.CompilationErrors);
    }

    [Fact]
    public void WhenGenerate_Expression_WhichReturnsBoolean_ShouldGenerateCorrectly()
    {
        // Arrange
        const string source =
            """
            using System;
            using System.Linq.Expressions;
            using Codehard.Common.DomainModel;
            using Codehard.Common.DomainModel.Attributes;

            public class MyEntityRootKey : IEntityKey
            {
                public Guid Value { get; }
            }

            public partial class MyEntityRoot : IAggregateRoot<MyEntityRootKey>, IDisposable
            {
                public MyEntityRootKey Id { get; }
            
                public DateTimeOffset CreatedAt { get; }
            
                [SpecificationAttribute]
                public Expression<Func<MyEntityRoot, bool>> Expression1 = e => e.Id.Value == Guid.Empty;
                
                [SpecificationAttribute]
                public Expression<Func<MyEntityRoot, DateTimeOffset, bool>> Expression2 = (e, datetime) => e.CreatedAt == datetime;
            
                public void Dispose()
                {
                }
            }

            public static class Program
            {
                public static void Main(string[] args)
                {
                    var spec1 = new MyEntityRoot.Expression1Specification();
                    var spec2 = new MyEntityRoot.Expression2Specification(DateTimeOffset.UtcNow);
                }
            }
            """;


        // Act
        var result = Compiler.With(new DomainRepositoryGenerator()).Compile(source);

        // Assert
        Assert.Empty(result.CompilationErrors);
    }

    [Fact]
    public void WhenGenerate_Delegate_WhichReturnsBoolean_ShouldGenerateCorrectly()
    {
        // Arrange
        const string source =
            """
            using System;
            using System.Linq.Expressions;
            using Codehard.Common.DomainModel;
            using Codehard.Common.DomainModel.Attributes;

            public delegate bool IsCreatedBefore2022Delegate(MyEntityRoot entity);

            public delegate bool IdComparerDelegate(MyEntityRoot entity, Guid id);

            public class MyEntityRootKey : IEntityKey
            {
                public Guid Value { get; }
            }

            public partial class MyEntityRoot : IAggregateRoot<MyEntityRootKey>, IDisposable
            {
                public MyEntityRootKey Id { get; }
            
                public DateTimeOffset CreatedAt { get; }
            
                [Specification]
                public IsCreatedBefore2022Delegate IsCreatedBefore2022 = (e) => e.CreatedAt.Year < 2022;
            
                [Specification]
                public IdComparerDelegate IsIdEqual = (e, id) => e.Id.Value == id;
            
                public void Dispose()
                {
                }
            }

            public static class Program
            {
                public static void Main(string[] args)
                {
                    var spec1 = new MyEntityRoot.IsCreatedBefore2022Specification();
                    var spec2 = new MyEntityRoot.IsIdEqualSpecification(Guid.Empty);
                }
            }
            """;


        // Act
        var result = Compiler.With(new DomainRepositoryGenerator()).Compile(source);

        // Assert
        Assert.Empty(result.CompilationErrors);
    }
}