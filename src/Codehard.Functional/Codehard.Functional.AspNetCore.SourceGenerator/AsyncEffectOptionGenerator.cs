using System.Text;
using Microsoft.CodeAnalysis;

namespace Codehard.Functional.AspNetCore.SourceGenerator;

[Generator]
public class AsyncEffectOptionGenerator : HttpStatusGeneratorBase
{
    private const string ExpectedClassName = "AsyncEffectExtensions";
    protected override string Namespace => "Codehard.Functional.AspNetCore";
    protected override string ClassName => ExpectedClassName;

    protected override void AppendClassComment(StringBuilder stringBuilder)
    {
        stringBuilder.AppendLine("/// <summary>");
        stringBuilder.AppendLine("/// Provides extension methods for mapping a None state to HTTP status codes.");
        stringBuilder.AppendLine("/// </summary>");
    }

    protected override void AppendMethod(StringBuilder stringBuilder, int code, string statusName)
    {
        var extensionMethodName = $"MapNoneTo{statusName}";

        var overload1 =
            $"    public static Aff<A> {extensionMethodName}<A>(this Aff<Option<A>> ma, string message = \"\") => ma.Bind(opt => opt.ToAffWithFailTo{statusName}(message));";

        var comment = GenerateComment(code, statusName);

        stringBuilder.AppendLine(comment);
        stringBuilder.AppendLine(overload1);

        static string GenerateComment(int code, string status)
        {
            return string.Empty;
        }
    }
}