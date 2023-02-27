using System.Reflection;

namespace Codehard.Functional.EntityFramework;

internal static class EntityOptionMapping
{
    public static readonly Dictionary<(Type, string), PropertyInfo> OptionBackingFieldMapping = new();
}