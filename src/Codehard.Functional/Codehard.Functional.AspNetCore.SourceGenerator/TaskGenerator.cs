using System.Text;
using Microsoft.CodeAnalysis;

namespace Codehard.Functional.AspNetCore.SourceGenerator;

[Generator]
public class TaskGenerator : HttpStatusGeneratorBase
{
    private const string ExpectedClassName = "TaskExtensions";
    protected override string Namespace => "Codehard.Functional.AspNetCore";
    protected override string ClassName => ExpectedClassName;

    protected override void AppendClassComment(StringBuilder stringBuilder)
    {
    }

    protected override void AppendMethod(StringBuilder stringBuilder, int code, string statusName)
    {
        var extensionMethodName = $"ToAffWithFailTo{statusName}";
        var nullableExtensionMethodName = $"ToAffOptionWithFailTo{statusName}";

        var method1 =
            $"    public static Aff<A> {extensionMethodName}<A>(this Task<A> ma, string message = \"\") => ma.ToAff().MapFailTo{statusName}(message);";

        var method2 =
            $"    public static Aff<A> {extensionMethodName}<A>(this Task<A> ma, Func<Error, string> errorMessageFunc) => ma.ToAff().MapFailTo{statusName}(errorMessageFunc);";

        var method3 =
            $"    public static Aff<Option<A>> {nullableExtensionMethodName}<A>(this Task<A?> ma, string message = \"\") => ma.ToAffOption().MapFailTo{statusName}(message);";

        var method4 =
            $"    public static Aff<Option<A>> {nullableExtensionMethodName}<A>(this Task<A?> ma, Func<Error, string> errorMessageFunc) => ma.ToAffOption().MapFailTo{statusName}(errorMessageFunc);";
        var method5 =
            $"    public static Aff<A> {extensionMethodName}<A>(this ValueTask<A> ma, string message = \"\") => ma.ToAff().MapFailTo{statusName}(message);";
        var method6 =
            $"    public static Aff<A> {extensionMethodName}<A>(this ValueTask<A> ma, Func<Error, string> errorMessageFunc) => ma.ToAff().MapFailTo{statusName}(errorMessageFunc);";
        var method7 =
            $"    public static Aff<Option<A>> {nullableExtensionMethodName}<A>(this ValueTask<A?> ma, string message = \"\") => ma.ToAffOption().MapFailTo{statusName}(message);";

        var method8 =
            $"    public static Aff<Option<A>> {nullableExtensionMethodName}<A>(this ValueTask<A?> ma, Func<Error, string> errorMessageFunc) => ma.ToAffOption().MapFailTo{statusName}(errorMessageFunc);";

        stringBuilder.AppendLine(method1);
        stringBuilder.AppendLine(method2);
        stringBuilder.AppendLine(method3);
        stringBuilder.AppendLine(method4);
        stringBuilder.AppendLine(method5);
        stringBuilder.AppendLine(method6);
        stringBuilder.AppendLine(method7);
        stringBuilder.AppendLine(method8);
        
        static string GenerateComment(int code, string status)
        {
            return string.Empty;
        }
    }
}