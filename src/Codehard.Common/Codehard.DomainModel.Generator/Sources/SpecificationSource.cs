using System.Linq;
using Scriban;

namespace Codehard.DomainModel.Generator.Sources;

internal static class SpecificationSource
{
    private const string TemplateText =
        """
        // <auto-generated />
        {{~ for using in usings ~}}
        using {{ using }};
        {{~ end ~}}
        {{~ if namespace }}
        namespace {{ namespace }};
        {{ end ~}}

        partial {{ declaration.type }} {{ declaration.name }}
        {
            {{~ for specification in specifications ~}}
            {{~ spec_name = specification.name + "Specification" ~}}
            public partial class {{ spec_name }} : ExpressionSpecification<{{ declaration.name }}>
            {
                public {{ spec_name }}(
                    {{- for arg in specification.args -}}
                    {{ arg.type }} {{ arg.name }}{{ if for.last == false }}, {{ end }}
                    {{- end -}}
                ) : base({{ specification.expression }})
                {
                }
            }
            {{~ if for.last == false ~}}

            {{~ end ~}}
            {{~ end ~}}
        }
        """;

    private static readonly Template Template = Template.Parse(TemplateText);

    public static string GenerateSpecifications(this DomainEntityDefinition entityDefinition)
    {
        var entityDeclarationType = entityDefinition.IsRecord ? "record" : "class";

        var arguments = new
        {
            usings = entityDefinition.Usings,
            @namespace = entityDefinition.Namespace,
            declaration = new
            {
                type = entityDeclarationType,
                name = entityDefinition.EntityName,
            },
            specifications = entityDefinition.SpecificationDefinitions.Select(d => new
            {
                name = d.Name,
                args = d.Arguments.Select(a => new
                {
                    type = a.Type,
                    name = a.Name,
                }),
                expression = d.ExpressionText,
            }),
        };

        var result = Template.Render(arguments);

        return result;
    }
}