namespace Codehard.DomainModel.Generator.Sources;

internal static class SpecificationAttributeSource
{
    public const string Namespace = "Codehard.Common.DomainModel.Attributes";
    public const string Name = "SpecificationAttribute";
    public const string ShortName = "Specification";
    public const string FullyQualifiedName = $"{Namespace}.{Name}";

    public const string SourceCode =
        $@"
using System;

namespace {Namespace};

/// <summary>
/// Represents an attribute used to mark a field as a specification.
/// </summary>
[AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
public sealed class {Name} : Attribute
{{
}}";
}