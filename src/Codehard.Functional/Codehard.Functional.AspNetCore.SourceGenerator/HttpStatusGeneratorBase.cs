using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Codehard.Functional.AspNetCore.SourceGenerator;

public abstract class HttpStatusGeneratorBase : ISourceGenerator
{
    // Contains some status code that is not present in netstandard2.0
    private readonly (int Code, string Name)[] additionalHttpStatusCodes =
    {
        (422, "UnprocessableEntity"),
        (423, "Locked"),
    };

    protected abstract string Namespace { get; }
    protected abstract string ClassName { get; }

    public void Initialize(GeneratorInitializationContext context)
    {
    }

    public void Execute(GeneratorExecutionContext context)
    {
        if (!ShouldExecute(context))
        {
            return;
        }

        var httpStatusCodeType = typeof(HttpStatusCode);
        var httpStatuses = Enum.GetValues(httpStatusCodeType);

        var stringBuilder = new StringBuilder();

        this.AppendUsing(stringBuilder);
        stringBuilder.AppendLine($"namespace {this.Namespace};");
        stringBuilder.AppendLine();
        this.AppendClassComment(stringBuilder);
        stringBuilder.AppendLine($"public static partial class {this.ClassName}");
        stringBuilder.AppendLine("{");

        var existingStatuses = new HashSet<int>();

        foreach (var @object in httpStatuses)
        {
            var value = (int)@object;

            if (existingStatuses.Contains(value))
            {
                continue;
            }

            existingStatuses.Add(value);

            var status = Enum.GetName(httpStatusCodeType, @object)!;

            stringBuilder.AppendLine();
            this.AppendMethod(stringBuilder, value, status);
        }

        foreach (var pair in additionalHttpStatusCodes)
        {
            if (existingStatuses.Contains(pair.Code))
            {
                continue;
            }

            existingStatuses.Add(pair.Code);

            this.AppendMethod(stringBuilder, pair.Code, pair.Name);
        }

        stringBuilder.AppendLine("}");

        context.AddSource(this.GetType().Name, SourceText.From(stringBuilder.ToString(), Encoding.UTF8));
    }

    protected virtual bool ShouldExecute(GeneratorExecutionContext context)
    {
        // In case we ever implement an ISyntaxReceiver, this will come in handy.
        return true;
    }

    protected virtual void AppendUsing(StringBuilder stringBuilder)
    {
        stringBuilder.AppendLine("using System.Net;");
        stringBuilder.AppendLine("using LanguageExt;");
        stringBuilder.AppendLine("using LanguageExt.Common;");
        stringBuilder.AppendLine("using static LanguageExt.Prelude;");
    }

    protected abstract void AppendClassComment(StringBuilder stringBuilder);

    protected abstract void AppendMethod(StringBuilder stringBuilder, int code, string statusName);
}