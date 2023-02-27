using System.Reflection;

namespace Codehard.Functional.EntityFramework;

internal static class Conventions
{
    public static string GetBackingFieldName(this MemberInfo memberInfo)
    {
        return $"_{memberInfo.Name.ToLowerInvariant()}";
    }
}