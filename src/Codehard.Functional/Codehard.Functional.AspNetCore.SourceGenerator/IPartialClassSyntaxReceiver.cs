using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Codehard.Functional.AspNetCore.SourceGenerator;

public interface IPartialClassSyntaxReceiver : ISyntaxReceiver
{
    ClassDeclarationSyntax? ClassToAugment { get; }
}