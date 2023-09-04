using System.Linq;
using System.Text;

namespace Codehard.DomainModel.Generator.Extensions;

internal static class DefinitionExtensions
{
    public static void AppendSpecification(
        this StringBuilder sb,
        DomainEntityDefinition entityDefinition,
        SpecificationDefinition specification)
    {
        var specificationName = $"{specification.Name}Specification";

        sb.AppendLine(
            $"    public sealed class {specificationName} : ExpressionSpecification<{entityDefinition.EntityName}>");
        sb.AppendLine("    {");
        sb.Append($"        public {specificationName}(");

        if (specification.Arguments.Count == 0)
        {
            sb.AppendLine(")");
        }
        else if (specification.Arguments.Count == 1)
        {
            var arg = specification.Arguments.Single();
            sb.AppendLine($"{arg.Type} {arg.Name})");
        }
        else
        {
            sb.AppendLine();

            for (var index = 0; index < specification.Arguments.Count; index++)
            {
                var argument = specification.Arguments.ElementAt(index);

                sb.Append($"            {argument.Type} {argument.Name}");

                if (index != specification.Arguments.Count - 1)
                {
                    sb.AppendLine(",");
                }
            }

            sb.AppendLine(")");
        }

        sb.AppendLine($"            : base({specification.ExpressionText})");
        sb.AppendLine("        {");
        sb.AppendLine("        }");

        sb.AppendLine("    }");
        sb.AppendLine();
    }
}