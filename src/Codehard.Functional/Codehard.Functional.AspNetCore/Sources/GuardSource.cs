using System.Net;
using System.Text;
using Codehard.Functional.AspNetCore.Shared;

namespace Codehard.Functional.AspNetCore.Sources;

internal static class GuardSource
{
    public static string FileName = "GuardExtensions.g.cs";

    public static string Generate()
    {
        var sb = new StringBuilder();

        sb.AppendLine("// <auto-generated/>");
        sb.AppendLine("using System.Net;");
        sb.AppendLine("using LanguageExt;");
        sb.AppendLine("using LanguageExt.Common;");
        sb.AppendLine();
        sb.AppendLine($"namespace {Constants.Namespace};");
        sb.AppendLine();
        sb.AppendLine("public static class GuardExtensions");
        sb.AppendLine("{");
        sb.AppendLine(
            "    public static Guard<Error> GuardWithHttpStatus(bool flag, HttpStatusCode httpStatusCode, string message = \"\")");
        sb.AppendLine(
            "    {");
        sb.AppendLine(
            "        return new Guard<Error>(flag, Error.New((int)httpStatusCode, message));");
        sb.AppendLine(
            "    }");

        var statusCodes =
            Enum.GetValues(typeof(HttpStatusCode))
                .Cast<HttpStatusCode>()
                .Distinct();

        foreach (var statusCode in statusCodes)
        {
            var name = Enum.GetName(typeof(HttpStatusCode), statusCode);

            sb.AppendLine();
            sb.AppendLine($"    public static Guard<Error> GuardWith{name}(bool flag, string message = \"\")");
            sb.AppendLine($"        => GuardWithHttpStatus(flag, HttpStatusCode.{name}, message);");
        }

        sb.AppendLine("}");

        return sb.ToString();
    }
}