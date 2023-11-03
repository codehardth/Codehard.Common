using System.Text;
using Codehard.Functional.AspNetCore.Sources;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Codehard.Functional.AspNetCore.Generators;

[Generator]
public class FunctionalExtensionGenerator : IIncrementalGenerator
{
    /// <summary>
    /// Called to initialize the generator and register generation steps via callbacks
    /// on the <paramref name="context" />
    /// </summary>
    /// <param name="context">The <see cref="T:Microsoft.CodeAnalysis.IncrementalGeneratorInitializationContext" /> to register callbacks on</param>
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(ctx =>
        {
            ctx.AddSource(
                AffSource.FileName,
                SourceText.From(AffSource.Generate(), Encoding.UTF8));
            
            ctx.AddSource(
                CommonSource.FileName,
                SourceText.From(CommonSource.Generate(), Encoding.UTF8));
            
            ctx.AddSource(
                ControllerSource.FileName,
                SourceText.From(ControllerSource.Generate(), Encoding.UTF8));
            
            ctx.AddSource(
                EffSource.FileName,
                SourceText.From(EffSource.Generate(), Encoding.UTF8));
            
            ctx.AddSource(
                ErrorWrapperActionResultSource.FileName,
                SourceText.From(ErrorWrapperActionResultSource.Generate(), Encoding.UTF8));
            
            ctx.AddSource(
                GuardSource.FileName,
                SourceText.From(GuardSource.Generate(), Encoding.UTF8));
            
            ctx.AddSource(
                HttpResultErrorSource.FileName,
                SourceText.From(HttpResultErrorSource.Generate(), Encoding.UTF8));
            
            ctx.AddSource(
                OptionSource.FileName,
                SourceText.From(OptionSource.Generate(), Encoding.UTF8));
            
            ctx.AddSource(
                TaskSource.FileName,
                SourceText.From(TaskSource.Generate(), Encoding.UTF8));
            
            ctx.AddSource(
                ValidationSource.FileName,
                SourceText.From(ValidationSource.Generate(), Encoding.UTF8));
        });
    }
}