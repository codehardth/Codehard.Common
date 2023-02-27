using Codehard.Functional.EntityFramework.Interceptors;

namespace Microsoft.EntityFrameworkCore;

public static class CodehardDbContextOptionsBuilderExtensions
{
    public static DbContextOptionsBuilder AddOptionalTranslator(this DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.AddInterceptors(new OptionalTranslatorExpressionInterceptor());
}