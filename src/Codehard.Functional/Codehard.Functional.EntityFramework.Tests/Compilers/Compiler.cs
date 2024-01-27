using System.Reflection;
using LanguageExt;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;

namespace Codehard.Functional.EntityFramework.Tests.Compilers;

internal sealed class Compiler<TCodeGenerator>
    where TCodeGenerator : IIncrementalGenerator, new()
{
    public static CompilationResult Compile(params string[] sources)
    {
        var baseCompilation = CreateCompilation(sources);
        var (outputCompilation, compilationDiagnostics, generationDiagnostics) = RunGenerator(baseCompilation);

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

    private static GenerationResult RunGenerator(Compilation compilation)
    {
        var codeGenerator = new TCodeGenerator();

        CSharpGeneratorDriver
            .Create(codeGenerator)
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

    private static Compilation CreateCompilation(params string[] sources)
    {
        var systemRuntimePath =
            AppDomain.CurrentDomain.GetAssemblies()
                .Where(asm => !string.IsNullOrWhiteSpace(asm.FullName))
                .FirstOrDefault(asm => asm.FullName!.StartsWith("System.Runtime"))?
                .Location;
        
        if (systemRuntimePath is null)
        {
            throw new InvalidOperationException(
                "Unable to load `System.Runtime` which is required for the compiler to run.");
        }
        
        var netstandard =
            AppDomain.CurrentDomain.GetAssemblies()
                .Where(asm => !string.IsNullOrWhiteSpace(asm.FullName))
                .FirstOrDefault(asm => asm.FullName!.StartsWith("netstandard"))?
                .Location;
        
        if (netstandard is null)
        {
            throw new InvalidOperationException(
                "Unable to load `netstandard` which is required for the compiler to run.");
        }

        var taskExtensions =
            Path.Combine(
                Directory.GetCurrentDirectory(),
                "System.Threading.Tasks.Extensions.dll");
        
        if (taskExtensions is null)
        {
            throw new InvalidOperationException(
                "Unable to load `System.Threading.Tasks.Extensions` which is required for the compiler to run.");
        }
        
        return
            CSharpCompilation.Create(
                "compilation",
                sources.Select(static source => CSharpSyntaxTree.ParseText(source)),
                new[]
                {
                    MetadataReference.CreateFromFile(typeof(object).GetTypeInfo().Assembly.Location),
                    MetadataReference.CreateFromFile(typeof(Aff<>).GetTypeInfo().Assembly.Location),
                    MetadataReference.CreateFromFile(typeof(DbContext).GetTypeInfo().Assembly.Location),
                    MetadataReference.CreateFromFile(taskExtensions),
                    MetadataReference.CreateFromFile(netstandard),
                    MetadataReference.CreateFromFile(systemRuntimePath),
                },
                new CSharpCompilationOptions(OutputKind.ConsoleApplication)
            );
    }
        
}
