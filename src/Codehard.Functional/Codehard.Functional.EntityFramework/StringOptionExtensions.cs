using LanguageExt;

namespace Microsoft.EntityFrameworkCore;

public static class StringOptionDbFunctionsExtensions
{
    public static bool Contains(this DbFunctions _, Option<string> option, string value)
    {
        return option.IfNoneUnsafe((string?)null)?.Contains(value) ?? false;
    }

    public static bool EndsWith(this DbFunctions _, Option<string> option, string value)
    {
        return option.IfNoneUnsafe((string?)null)?.EndsWith(value) ?? false;
    }

    public static bool StartsWith(this DbFunctions _, Option<string> option, string value)
    {
        return option.IfNoneUnsafe((string?)null)?.StartsWith(value) ?? false;
    }

    public static string? ToLower(this DbFunctions _, Option<string> option)
    {
        return option.IfNoneUnsafe((string?)null)?.ToLower();
    }

    public static string? ToUpper(this DbFunctions _, Option<string> option)
    {
        return option.IfNoneUnsafe((string?)null)?.ToUpper();
    }
}