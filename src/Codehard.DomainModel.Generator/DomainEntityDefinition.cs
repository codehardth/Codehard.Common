using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Codehard.DomainModel.Generator.Extensions;
using Codehard.DomainModel.Generator.Sources;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Codehard.DomainModel.Generator;

internal sealed record SpecificationArgument(string Type, string Name);

internal sealed record SpecificationDefinition(
    IReadOnlyCollection<SpecificationArgument> Arguments,
    string Name,
    string ExpressionText);

internal sealed record DomainEntityDefinition(
    TypeDeclarationSyntax DomainEntityDeclaration,
    string? Namespace,
    Accessibility Accessibility,
    string EntityName,
    IReadOnlyCollection<SpecificationDefinition> SpecificationDefinitions)
{
    public bool IsRecord => this.DomainEntityDeclaration is RecordDeclarationSyntax;

    public static DomainEntityDefinition? Parse(
        Compilation compilation,
        TypeDeclarationSyntax declaration)
    {
        var semanticModel = compilation.GetSemanticModel(declaration.SyntaxTree);
        var symbol = ModelExtensions.GetDeclaredSymbol(semanticModel, declaration);

        if (symbol is null)
        {
            return default;
        }

        const string expectedInterface = "IAggregateRoot";

        var interfaces =
            declaration.BaseList?.Types;

        if (interfaces is null)
        {
            return default;
        }

        var entityInterface =
            interfaces
                .Value
                .Select(ts => ts.Type)
                .Where(t => t is GenericNameSyntax)
                .Cast<GenericNameSyntax>()
                .Select(gns => gns.Identifier)
                .SingleOrDefault(id => id.Value is expectedInterface);

        if (entityInterface.IsKind(SyntaxKind.None))
        {
            return default;
        }

        var @namespace = symbol.ContainingNamespace.GetSafeDisplayName();
        var accessibility = symbol.DeclaredAccessibility;
        var entityName = symbol.Name;
        var specificationDefinitions =
            declaration
                .DescendantNodes()
                .OfType<FieldDeclarationSyntax>()
                .Where(f =>
                    f.IsDecoratedWith(SpecificationAttributeSource.FullyQualifiedName, semanticModel))
                .Select(ToSpecificationDefinition)
                .Where(d => d is not null)
                .ToImmutableArray();

        return new DomainEntityDefinition(
            declaration,
            @namespace,
            accessibility,
            entityName,
            specificationDefinitions);

        SpecificationDefinition? ToSpecificationDefinition(FieldDeclarationSyntax field)
        {
            return field.Declaration.Type switch
            {
                GenericNameSyntax gns => ParseGenericNameSyntax(gns),
                IdentifierNameSyntax ins => ParseIdentifierNameSyntax(ins),
                _ => default,
            };

            SpecificationDefinition? ParseIdentifierNameSyntax(IdentifierNameSyntax syntax)
            {
                var specificationName = field.Declaration.Variables.SingleOrDefault()?.Identifier.ToString();

                if (string.IsNullOrWhiteSpace(specificationName))
                {
                    return default;
                }

                var symbolInfo = semanticModel.GetSymbolInfo(syntax);

                if (symbolInfo.Symbol is not INamedTypeSymbol namedTypeSymbol)
                {
                    return default;
                }

                if (namedTypeSymbol.DelegateInvokeMethod is not { } @delegate ||
                    @delegate.ReturnType.SpecialType is not SpecialType.System_Boolean)
                {
                    return default;
                }

                var variable = field.Declaration.Variables.SingleOrDefault();
                var initializer = variable?.Initializer?.Value;

                if (initializer is not LambdaExpressionSyntax lambdaExpressionSyntax)
                {
                    return default;
                }

                var initializerSymbol = semanticModel.GetSymbolInfo(initializer);

                if (initializerSymbol.Symbol is not IMethodSymbol methodSymbol)
                {
                    return default;
                }

                var parameters = methodSymbol.Parameters;

                if (parameters.Length < 1)
                {
                    return default;
                }

                var isFirstArgumentTheSameAsEntity =
                    parameters.First().Type.Equals(symbol, SymbolEqualityComparer.Default);
                var trimmedParameters =
                    isFirstArgumentTheSameAsEntity
                        ? parameters.Skip(1)
                        : parameters;

                var expression = $"{parameters.First().Name} => {lambdaExpressionSyntax.Body.ToString()}";

                return new SpecificationDefinition(
                    trimmedParameters
                        .Select(p => new SpecificationArgument(p.Type.ToString(), p.Name))
                        .ToImmutableArray(),
                    methodSymbol.ContainingSymbol.Name,
                    expression);
            }

            SpecificationDefinition? ParseGenericNameSyntax(GenericNameSyntax syntax)
            {
                var typeName = syntax.Identifier.ToString();

                if (typeName is not ("Func" or "Expression"))
                {
                    return default;
                }

                var variable = field.Declaration.Variables.SingleOrDefault();
                var initializer = variable?.Initializer?.Value;

                if (initializer is null)
                {
                    return default;
                }

                var initializerSymbol = semanticModel.GetSymbolInfo(initializer);

                if (initializerSymbol.Symbol is not IMethodSymbol methodSymbol)
                {
                    return default;
                }

                var parameters = methodSymbol.Parameters;
                var isFirstArgumentTheSameAsEntity =
                    parameters.First().Type.Equals(symbol, SymbolEqualityComparer.Default);
                var trimmedParameters =
                    isFirstArgumentTheSameAsEntity
                        ? parameters.Skip(1)
                        : parameters;
                var expression = typeName switch
                {
                    "Func" when initializer is LambdaExpressionSyntax { Body: LambdaExpressionSyntax } les =>
                        les.Body.ToString(),
                    "Func" when initializer is LambdaExpressionSyntax { Body: LiteralExpressionSyntax } les =>
                        $"{parameters[0].Name} => {les.Body.ToString()}",
                    "Expression" when initializer is LambdaExpressionSyntax les =>
                        $"{parameters[0].Name} => {les.Body.ToString()}",
                    _ when initializer is LambdaExpressionSyntax les =>
                        $"{parameters[0].Name} => {les.Body.ToString()}",
                    _ => string.Empty,
                };

                return new SpecificationDefinition(
                    trimmedParameters
                        .Select(p => new SpecificationArgument(p.Type.ToString(), p.Name))
                        .ToImmutableArray(),
                    methodSymbol.ContainingSymbol.Name,
                    expression);
            }
        }
    }
}