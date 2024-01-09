using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace Codehard.Functional.EntityFramework.Tests.Compilers;

internal sealed record GenerationResult(
    Compilation Compilation,
    ImmutableArray<Diagnostic> CompilationDiagnostics,
    ImmutableArray<Diagnostic> GenerationDiagnostics)
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