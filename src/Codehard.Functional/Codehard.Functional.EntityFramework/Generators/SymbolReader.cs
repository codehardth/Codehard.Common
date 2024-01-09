using Microsoft.CodeAnalysis;

namespace Codehard.Functional.EntityFramework.Generators;

internal static class SymbolReader
{
    internal static string GetInnerGenericTypes(INamedTypeSymbol namedTypeSymbol)
    {
        var innerGenericTypes = namedTypeSymbol.Name;
    
        foreach (var typeArgument in namedTypeSymbol.TypeArguments)
        {
            if (typeArgument is INamedTypeSymbol innerNamedTypeSymbol &&
                innerNamedTypeSymbol.TypeArguments.Length > 0)
            {
                innerGenericTypes += 
                    $"<{string.Join(",", GetInnerGenericTypes(innerNamedTypeSymbol))}>";
            }
            else
            {
                innerGenericTypes += $"<{typeArgument.Name}>";
            }
        }
    
        return innerGenericTypes;
    }
    
    internal static string? GetMethodGenericTypeParameters(IMethodSymbol methodSymbol)
    {
        if (!methodSymbol.TypeParameters.Any())
            return null;
        
        return
            "<" + 
            string.Join(", ", methodSymbol.TypeParameters.Select(p => p.Name)) +
            ">";
    }
    
    internal static string? GetMethodConstraints(IMethodSymbol methodSymbol)
    {
        if (!methodSymbol.TypeParameters.Any())
            return null;
        
        var constraints =
            methodSymbol.TypeParameters
                        .Select(tp =>
                        {
                            var constraintTypes =
                                tp.ConstraintTypes.Select(ct => ct.Name).ToList();

                            if (tp.HasReferenceTypeConstraint)
                            {
                                constraintTypes.Add("class");
                            }

                            if (tp.HasValueTypeConstraint)
                            {
                                constraintTypes.Add("struct");
                            }

                            if (tp.HasConstructorConstraint)
                            {
                                constraintTypes.Add("new()");
                            }

                            if (tp.HasNotNullConstraint)
                            {
                                constraintTypes.Add("notnull");
                            }
                            
                            return new
                            {
                                Parameter = tp.Name,
                                Constraints = string.Join(", ", constraintTypes)
                            };
                        })
                        .Where(tp => !string.IsNullOrEmpty(tp.Constraints))
                        .Select(tp => $"where {tp.Parameter} : {tp.Constraints}");

        return $"\t\t{string.Join("\n\t\t", constraints)}\n";
    }

    internal static string GetMethodDeclarationParameters(IMethodSymbol methodSymbol)
    {
        return string.Join(
            ", ",
            methodSymbol.Parameters
                        .Select(p => $"{p.Type} {p.Name}{GetParameterDefaultValue(p)}"));
    }

    internal static string? GetParameterDefaultValue(IParameterSymbol p)
    {
        if (!p.HasExplicitDefaultValue)
        {
            return null;
        }
        
        var defaultValue = p.ExplicitDefaultValue?.ToString();
                                
        if (defaultValue != null && p.Type.ToString() == "string")
        {
            return $" = \"{defaultValue}\"";
        }

        if (p.ExplicitDefaultValue == null)
        {
            return " = default";
        }
        
        return " = " + defaultValue;
    }

    internal static string GetMethodCallParameters(IMethodSymbol methodSymbol)
    {
        return string.Join(", ", methodSymbol.Parameters.Select(p => p.Name));
    }
}