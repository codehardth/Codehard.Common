using System.Text;
using Microsoft.CodeAnalysis;

namespace Codehard.Functional.AspNetCore.SourceGenerator;

[Generator]
public class ValidationGenerator : HttpStatusGeneratorBase
{
    private const string ExpectedClassName = "ValidationExtensions";
    protected override string Namespace => "Codehard.Functional.AspNetCore";
    protected override string ClassName => ExpectedClassName;

    protected override void AppendClassComment(StringBuilder stringBuilder)
    {
    }

    protected override void AppendMethod(StringBuilder stringBuilder, int code, string statusName)
    {
        var affMethodName = $"ToAffWithFailTo{statusName}";
        var effMethodName = $"ToEffWithFailTo{statusName}";

        var affOverload1 =
            $"    public static Aff<A> {affMethodName}<A>(this Validation<Error, A> ma, int errorCode, string errorMessage = \"\") => ma.ToAff(se => se.Flatten(errorCode)).MapFailTo{statusName}(_ => errorMessage);";
        var affOverload2 =
            $"    public static Aff<A> {affMethodName}<A>(this Validation<Error, A> ma, int errorCode, Func<Error, string> errorMessageFunc) => ma.ToAff(se => se.Flatten(errorCode)).MapFailTo{statusName}(errorMessageFunc);";
        var effOverload1 =
            $"    public static Eff<A> {effMethodName}<A>(this Validation<Error, A> ma, int errorCode, string errorMessage = \"\") => ma.ToEff(se => se.Flatten(errorCode)).MapFailTo{statusName}(_ => errorMessage);";
        var effOverload2 =
            $"    public static Eff<A> {effMethodName}<A>(this Validation<Error, A> ma, int errorCode, Func<Error, string> errorMessageFunc) => ma.ToEff(se => se.Flatten(errorCode)).MapFailTo{statusName}(errorMessageFunc);";


        stringBuilder.AppendLine(affOverload1);
        stringBuilder.AppendLine(affOverload2);
        stringBuilder.AppendLine(effOverload1);
        stringBuilder.AppendLine(effOverload2);
    }
}