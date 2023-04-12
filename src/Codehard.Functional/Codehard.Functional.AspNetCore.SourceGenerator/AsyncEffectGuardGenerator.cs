using System.Text;
using Microsoft.CodeAnalysis;

namespace Codehard.Functional.AspNetCore.SourceGenerator;

[Generator]
public class AsyncEffectGuardGenerator : HttpStatusGeneratorBase
{
    private const string ExpectedClassName = "AsyncEffectExtensions";
    protected override string Namespace => "Codehard.Functional.AspNetCore";
    protected override string ClassName => "AsyncEffectExtensions";

    protected override void AppendUsing(StringBuilder stringBuilder)
    {
    }

    protected override void AppendClassComment(StringBuilder stringBuilder)
    {
        stringBuilder.AppendLine("/// <summary>");
        stringBuilder.AppendLine(
            "/// Provides extension methods for guarding expected predicate to HTTP status codes.");
        stringBuilder.AppendLine("/// </summary>");
    }

    protected override void AppendMethod(StringBuilder stringBuilder, int code, string statusName)
    {
        var extensionMethodName = $"GuardWith{statusName}";

        var overload1 =
            $"    public static Aff<A> {extensionMethodName}<A>(this Aff<A> ma, Func<A, bool> predicate, string message = \"\") => ma.GuardWithHttpStatus(predicate, HttpStatusCode.{statusName}, message);";

        var overload2 =
            $"    public static Aff<A> {extensionMethodName}<A>(this Aff<A> ma, Func<A, bool> predicate, Func<A, string> messageFunc) => ma.GuardWithHttpStatus(predicate, HttpStatusCode.{statusName}, messageFunc);";

        var comment = GenerateComment(code, statusName);

        stringBuilder.AppendLine(comment);
        stringBuilder.AppendLine(overload1);
        stringBuilder.AppendLine();
        stringBuilder.AppendLine(comment);
        stringBuilder.AppendLine(overload2);

        static string GenerateComment(int code, string status)
        {
            return string.Empty;
        }
    }
}