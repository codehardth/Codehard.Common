using LanguageExt;
using LanguageExt.UnsafeValueAccess;

// ReSharper disable once CheckNamespace
namespace Microsoft.EntityFrameworkCore;

public static class StringOptionDbFunctionsExtensions
{
    public static bool Contains(this DbFunctions _, Option<string> option, string value)
    {
        return option.ValueUnsafe()?.Contains(value) ?? false;
    }

    public static bool EndsWith(this DbFunctions _, Option<string> option, string value)
    {
        return option.ValueUnsafe()?.EndsWith(value) ?? false;
    }

    public static bool StartsWith(this DbFunctions _, Option<string> option, string value)
    {
        return option.ValueUnsafe()?.StartsWith(value) ?? false;
    }

    public static string? ToLower(this DbFunctions _, Option<string> option)
    {
        return option.ValueUnsafe()?.ToLower();
    }

    public static string? ToUpper(this DbFunctions _, Option<string> option)
    {
        return option.ValueUnsafe()?.ToUpper();
    }
}