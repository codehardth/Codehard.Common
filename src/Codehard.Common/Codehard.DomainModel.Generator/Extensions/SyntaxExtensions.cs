using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Codehard.DomainModel.Generator.Extensions;

internal static class SyntaxExtensions
{
    public static bool HasInterfaces(this SyntaxNode node)
        => node is TypeDeclarationSyntax { BaseList: not null } tds &&
           tds.BaseList.Types.Any();

    public static IEnumerable<InterfaceDeclarationSyntax> GetInterfaceDeclarations(
        this TypeDeclarationSyntax tds,
        SemanticModel semanticModel)
    {
        var classSymbol = semanticModel.GetDeclaredSymbol(tds);

        if (classSymbol is null)
        {
            yield break;
        }

        var interfaceImplementations = classSymbol.AllInterfaces;

        foreach (var interfaceSymbol in interfaceImplementations)
        {
            var interfaceSyntaxNode = interfaceSymbol.DeclaringSyntaxReferences.FirstOrDefault()?.GetSyntax();

            if (interfaceSyntaxNode is not InterfaceDeclarationSyntax interfaceSyntax)
            {
                continue;
            }

            yield return interfaceSyntax;
        }
    }

    public static bool IsDecoratedWith(
        this FieldDeclarationSyntax fds,
        string targetAttributeName,
        SemanticModel semanticModel)
    {
        return fds.AttributeLists
            .SelectMany(static attributeListSyntax => attributeListSyntax.Attributes)
            .Select(GetDecoratedType)
            .Select(static attributeSymbol => attributeSymbol?.ToDisplayString())
            .Any(attributeName => attributeName == targetAttributeName);

        INamedTypeSymbol? GetDecoratedType(AttributeSyntax attributeSyntax)
        {
            var symbolInfo = ModelExtensions.GetSymbolInfo(semanticModel, attributeSyntax);
            var namedTypeSymbol = symbolInfo.Symbol?.ContainingType;

            return namedTypeSymbol;
        }
    }


    public static IEnumerable<UsingDirectiveSyntax> GetAllUsings(this TypeDeclarationSyntax interfaceSyntax)
    {
        var root = interfaceSyntax.SyntaxTree.GetRoot();

        var usingDirectives =
            root.DescendantNodes()
                .OfType<UsingDirectiveSyntax>();

        return usingDirectives;
    }
}