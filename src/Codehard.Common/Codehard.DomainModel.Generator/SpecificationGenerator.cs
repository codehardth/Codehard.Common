using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading;
using Codehard.DomainModel.Generator;
using Codehard.DomainModel.Generator.Extensions;
using Codehard.DomainModel.Generator.Sources;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

[Generator]
internal class SpecificationGenerator : IIncrementalGenerator
{
    /// <summary>
    /// Called to initialize the generator and register generation steps via callbacks
    /// on the <paramref name="context" />
    /// </summary>
    /// <param name="context">The <see cref="T:Microsoft.CodeAnalysis.IncrementalGeneratorInitializationContext" /> to register callbacks on</param>
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var targets =
            context.SyntaxProvider
                .CreateSyntaxProvider(
                    predicate: static (node, _) => node.HasInterfaces(),
                    transform: GetGenerationTarget)
                .Where(tds => tds is not null)
                .Collect();

        var compilation = context.CompilationProvider.Combine(targets);

        context.RegisterSourceOutput(
            compilation,
            (spc, source) => this.Execute(source.Left, source.Right, spc));
    }

    protected virtual void Execute(
        Compilation compilation,
        ImmutableArray<TypeDeclarationSyntax?> typeDeclarations,
        SourceProductionContext context)
    {
        if (typeDeclarations.IsDefaultOrEmpty)
        {
            return;
        }

        typeDeclarations
            .Where(t => t is not null)
            .Select(t => t!)
            .AsParallel()
            .Select(ParseType)
            .Where(d => d is not null)
            .ForAll(GenerateSource!);

        DomainEntityDefinition? ParseType(TypeDeclarationSyntax tds)
            => DomainEntityDefinition.Parse(compilation, tds);

        void GenerateSource(DomainEntityDefinition definition)
        {
            var sourceFileName = $"{definition.EntityName}.g.cs";
            var sourceText = SourceText.From(definition.GenerateSpecifications(), Encoding.UTF8);

            context.AddSource(sourceFileName, sourceText);
        }
    }

    private static TypeDeclarationSyntax? GetGenerationTarget(
        GeneratorSyntaxContext context,
        CancellationToken cancellationToken)
    {
        var tds = context.Node as TypeDeclarationSyntax;

        return tds;
    }
}