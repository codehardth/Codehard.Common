using System.Text;
using Microsoft.CodeAnalysis;

namespace Codehard.Functional.AspNetCore.SourceGenerator;

[Generator]
public class AsyncEffectGenerator : HttpStatusGeneratorBase
{
    private const string ExpectedClassName = "AsyncEffectExtensions";
    protected override string Namespace => "Codehard.Functional.AspNetCore";
    protected override string ClassName => ExpectedClassName;

    protected override void AppendClassComment(StringBuilder stringBuilder)
    {
        stringBuilder.AppendLine("/// <summary>");
        stringBuilder.AppendLine("/// Provides extension methods for mapping failures to HTTP status codes.");
        stringBuilder.AppendLine("/// </summary>");
    }

    protected override void AppendMethod(StringBuilder stringBuilder, int code, string statusName)
    {
        var extensionMethodName = $"MapFailTo{statusName}";

        var overload1 =
            $"    public static Aff<A> {extensionMethodName}<A>(this Aff<A> ma, string errorMessage = \"\") => ma.CustomError((int)HttpStatusCode.{statusName}, errorMessage);";

        var overload2 =
            $"    public static Aff<A> {extensionMethodName}<A>(this Aff<A> ma, Func<Error, string> messageFunc) => ma.CustomError((int)HttpStatusCode.{statusName}, messageFunc);";

        var comment = GenerateComment(code, statusName);

        stringBuilder.AppendLine(comment);
        stringBuilder.AppendLine(overload1);
        stringBuilder.AppendLine();
        stringBuilder.AppendLine(comment);
        stringBuilder.AppendLine(overload2);

        static string GenerateComment(int code, string status)
        {
            return
                $@"    /// <summary>
    /// Maps a failed asynchronous effectful computation to a {code} {status} HTTP status code, with an optional error message.
    /// </summary>
    /// <typeparam name=""A"">The type of the computation's result.</typeparam>
    /// <param name=""ma"">The computation to map.</param>
    /// <param name=""errorMessage"">The optional error message to include in the resulting error.</param>
    /// <returns>An asynchronous effectful computation that succeeds with the original result if it succeeds, or fails with a {code} {status} HTTP status code and an error message if it fails.</returns>";
        }
    }
}