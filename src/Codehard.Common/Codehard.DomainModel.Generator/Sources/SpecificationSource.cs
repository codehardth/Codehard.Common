using System.Text;
using Codehard.DomainModel.Generator.Extensions;

namespace Codehard.DomainModel.Generator.Sources;

internal static class SpecificationSource
{
    public static string GenerateSpecifications(this DomainEntityDefinition entityDefinition)
    {
        var entityDeclarationType = entityDefinition.IsRecord ? "record" : "class";

        var sb = new StringBuilder();

        sb.AppendAutoGenerated();
        sb.AppendUsings(entityDefinition);
        sb.AppendLine();

        sb.AppendNamespace(entityDefinition);

        sb.AppendLine(
            $"partial {entityDeclarationType} {entityDefinition.EntityName}");
        sb.AppendLine("{");
        sb.AppendSpecifications(entityDefinition);
        sb.AppendLine("}");

        return sb.ToString();
    }
}