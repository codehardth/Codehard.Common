using Microsoft.CodeAnalysis;

namespace Codehard.DomainModel.Generator.Extensions;

internal static class NamespaceSymbolExtensions
{
    public static string? GetSafeDisplayName(this INamespaceSymbol symbol) =>
        symbol.ToString() switch
        {
            "<global namespace>" => null,
            var @namespace => @namespace,
        };
}