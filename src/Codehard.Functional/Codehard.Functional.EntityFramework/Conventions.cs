using System.Reflection;

namespace Codehard.Functional.EntityFramework;

internal static class Conventions
{
    public static string GetBackingFieldName(this MemberInfo memberInfo)
    {
        var name = memberInfo.Name;

        return $"{name[0].ToString().ToLowerInvariant()}{name[1..]}";
    }
}