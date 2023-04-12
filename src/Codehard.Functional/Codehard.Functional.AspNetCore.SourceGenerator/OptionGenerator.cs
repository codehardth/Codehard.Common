using System.Text;
using Microsoft.CodeAnalysis;

namespace Codehard.Functional.AspNetCore.SourceGenerator;

[Generator]
public class OptionGenerator : HttpStatusGeneratorBase
{
    private const string ExpectedClassName = "OptionExtensions";
    protected override string Namespace => "Codehard.Functional.AspNetCore";
    protected override string ClassName => ExpectedClassName;

    protected override void AppendClassComment(StringBuilder stringBuilder)
    {
    }

    protected override void AppendMethod(StringBuilder stringBuilder, int code, string statusName)
    {
        var affMethodName = $"ToAffWithFailTo{statusName}";
        var effMethodName = $"ToEffWithFailTo{statusName}";

        var affImpl =
            $"    public static Aff<A> {affMethodName}<A>(this Option<A> ma, string errorMessage = \"\") => ma.ToAff().MapFailTo{statusName}(errorMessage);";
        var effImpl =
            $"    public static Eff<A> {effMethodName}<A>(this Option<A> ma, string errorMessage = \"\") => ma.ToEff().MapFailTo{statusName}(errorMessage);";

        stringBuilder.AppendLine(affImpl);
        stringBuilder.AppendLine(effImpl);
    }
}