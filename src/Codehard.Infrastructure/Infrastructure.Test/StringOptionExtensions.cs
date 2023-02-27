using System.Globalization;
using LanguageExt;

namespace Infrastructure.Test;

public static class StringOptionExtensions
{
    public static bool Contains(Option<string> option, string value)
    {
        return option.IfNoneUnsafe((string?)null)?.Contains(value) ?? false;
    }

    public static bool EndsWith(Option<string> option, string value)
    {
        return option.IfNoneUnsafe((string?)null)?.EndsWith(value) ?? false;
    }

    public static bool StartsWith(Option<string> option, string value)
    {
        return option.IfNoneUnsafe((string?)null)?.StartsWith(value) ?? false;
    }

    public static string? ToLower(Option<string> option)
    {
        return option.IfNoneUnsafe((string?)null)?.ToLower();
    }

    public static string? ToUpper(Option<string> option)
    {
        return option.IfNoneUnsafe((string?)null)?.ToUpper();
    }
}