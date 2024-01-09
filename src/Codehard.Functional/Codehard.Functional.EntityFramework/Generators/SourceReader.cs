using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Codehard.Functional.EntityFramework.Generators;

internal static class SourceReader
{
    internal static string? GetNamespace(ClassDeclarationSyntax classDeclaration)
    {
        var namespaceDeclaration =
            classDeclaration.AncestorsAndSelf()
                .OfType<NamespaceDeclarationSyntax>()
                .FirstOrDefault();
            
        return namespaceDeclaration?.Name.ToString();
    }
    
    internal static string GetMethodDeclarationParameters(MethodDeclarationSyntax method)
    {
        return
            string.Join(
                ", ",
                method.ParameterList.Parameters
                    .Select(p =>
                    {
                        var defaultValue = p.Default?.Value.ToString();
                        if (defaultValue != null && p.Type?.ToString() == "string")
                        {
                            defaultValue = $"\"{defaultValue}\"";
                        }

                        return $"{p.Type} {p.Identifier.Text}{(defaultValue != null ? $" = {defaultValue}" : "")}";
                    }));
    }
        
    internal static string GetMethodCallParameters(MethodDeclarationSyntax method)
    {
        return
            string.Join(
                ", ",
                method.ParameterList.Parameters.Select(p => p.Identifier.Text));
    }
        
    internal static string? GetMethodGenericParameters(MethodDeclarationSyntax method)
    {
        return
            method.TypeParameterList is null
                ? null
                : $"<{string.Join(", ", method.TypeParameterList.Parameters.Select(p => p.Identifier.Text))}>";
    }
}