using System.Collections.Immutable;
using System.Linq.Expressions;
using System.Reflection;
using Codehard.Common.DomainModel;
using Codehard.Common.DomainModel.Attributes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Codehard.DomainModel.Generator.Tests.Compilation;

internal sealed class Compiler
{
    private readonly IIncrementalGenerator[] generators;

    private Compiler(IIncrementalGenerator[] generators)
    {
        this.generators = generators;
    }

    public CompilationResult Compile(params string[] sources)
    {
        var baseCompilation = CreateCompilation(sources);
        var (outputCompilation, compilationDiagnostics, generationDiagnostics) = this.RunGenerator(baseCompilation);

        using var ms = new MemoryStream();
        Assembly? assembly = null;

        try
        {
            outputCompilation.Emit(ms);
            assembly = Assembly.Load(ms.ToArray());
        }
        catch
        {
            // Do nothing since we want to inspect the diagnostics when compilation fails.
        }

        return new(
            Assembly: assembly,
            CompilationDiagnostics: compilationDiagnostics,
            GenerationDiagnostics: generationDiagnostics
        );
    }

    public static Compiler With(params IIncrementalGenerator[] generators)
    {
        return new Compiler(generators);
    }

    private GenerationResult RunGenerator(Microsoft.CodeAnalysis.Compilation compilation)
    {
        CSharpGeneratorDriver
            .Create(this.generators)
            .RunGeneratorsAndUpdateCompilation(
                compilation,
                out var outputCompilation,
                out var generationDiagnostics
            );

        return new(
            Compilation: outputCompilation,
            CompilationDiagnostics: outputCompilation.GetDiagnostics(),
            GenerationDiagnostics: generationDiagnostics
        );
    }

    private static Microsoft.CodeAnalysis.Compilation CreateCompilation(params string[] sources)
    {
        return CSharpCompilation.Create(
            "compilation",
            sources.Select(static source => CSharpSyntaxTree.ParseText(source)),
            new[]
            {
                AddDllReference(typeof(Binder)),
                AddDllReference(typeof(IEntity)),
                AddDllReference(typeof(Expression<>)),
                AddDllReference(typeof(Infrastructure.EntityFramework.EntityFrameworkRepositoryBase<>)),
                AddDllReference(typeof(Microsoft.EntityFrameworkCore.DbContext)),
                AddDllReference(typeof(SpecificationAttribute)),
                AddDllReferenceAssemblyName("System.Runtime"),
                AddDllReferenceAssemblyName("netstandard"),
            },
            new CSharpCompilationOptions(OutputKind.ConsoleApplication)
        );

        static MetadataReference AddDllReference(Type type)
        {
            var asm = type.GetTypeInfo().Assembly;

            return MetadataReference.CreateFromFile(asm.Location);
        }

        static MetadataReference AddDllReferenceAssemblyName(string assemblyName)
        {
            var path =
                AppDomain.CurrentDomain.GetAssemblies()
                    .Where(asm => !string.IsNullOrWhiteSpace(asm.FullName))
                    .FirstOrDefault(asm => asm.FullName!.StartsWith(assemblyName))?
                    .Location;

            if (path is null)
            {
                throw new InvalidOperationException(
                    $"Unable to load `{assemblyName}` which is required for the compiler to run.");
            }

            return MetadataReference.CreateFromFile(path);
        }
    }
}

internal sealed record CompilationResult(
    Assembly? Assembly,
    ImmutableArray<Diagnostic> CompilationDiagnostics,
    ImmutableArray<Diagnostic> GenerationDiagnostics
)
{
    public ImmutableArray<Diagnostic> CompilationErrors =>
        CompilationDiagnostics
            .Where(static diagnostic => diagnostic.Severity >= DiagnosticSeverity.Error)
            .ToImmutableArray();

    public ImmutableArray<Diagnostic> GenerationErrors =>
        GenerationDiagnostics
            .Where(static diagnostic => diagnostic.Severity >= DiagnosticSeverity.Error)
            .ToImmutableArray();
}

internal sealed record GenerationResult(
    Microsoft.CodeAnalysis.Compilation Compilation,
    ImmutableArray<Diagnostic> CompilationDiagnostics,
    ImmutableArray<Diagnostic> GenerationDiagnostics
)
{
    public ImmutableArray<Diagnostic> CompilationErrors =>
        CompilationDiagnostics
            .Where(diagnostic => diagnostic.Severity >= DiagnosticSeverity.Error)
            .ToImmutableArray();

    public ImmutableArray<Diagnostic> GenerationErrors =>
        GenerationDiagnostics
            .Where(diagnostic => diagnostic.Severity >= DiagnosticSeverity.Error)
            .ToImmutableArray();
}