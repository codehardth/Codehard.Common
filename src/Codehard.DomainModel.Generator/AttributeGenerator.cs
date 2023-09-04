using System.Text;
using Codehard.DomainModel.Generator.Sources;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Codehard.DomainModel.Generator;

[Generator]
public class AttributeGenerator : IIncrementalGenerator
{
    /// <summary>
    /// Called to initialize the generator and register generation steps via callbacks
    /// on the <paramref name="context" />
    /// </summary>
    /// <param name="context">The <see cref="T:Microsoft.CodeAnalysis.IncrementalGeneratorInitializationContext" /> to register callbacks on</param>
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(ctx =>
        {
            ctx.AddSource(
                $"{SpecificationAttributeSource.Name}.g.cs",
                SourceText.From(SpecificationAttributeSource.SourceCode, Encoding.UTF8));
        });
    }
}