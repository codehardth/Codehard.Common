using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Text;

[Generator]
public class EffEntityFrameworkQueryableExtensionsGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
    }

    public void Execute(GeneratorExecutionContext context)
    {
        var efQueryableExtensionsSymbol =
            context.Compilation.GetTypeByMetadataName(
                "Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions");

        if (efQueryableExtensionsSymbol == null)
        {
            return;
        }

        var sourceBuilder = new StringBuilder();

        sourceBuilder.AppendLine("namespace Codehard.Functional.EntityFramework.Generators");
        sourceBuilder.AppendLine("{");

        foreach (var member in efQueryableExtensionsSymbol.GetMembers().OfType<IMethodSymbol>())
        {
            sourceBuilder.AppendLine($"public static Eff<{member.ReturnType}> {member.Name}Eff({string.Join(", ", member.Parameters.Select(p => $"{p.Type} {p.Name}"))})");
            sourceBuilder.AppendLine("{");
            sourceBuilder.AppendLine($"    return Eff.From(() => {member.Name}({string.Join(", ", member.Parameters.Select(p => p.Name))}));");
            sourceBuilder.AppendLine("}");
        }

        sourceBuilder.AppendLine("}");

        context.AddSource("EffWrapperGenerator", SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));
    }
}