namespace Codehard.Functional.EntityFramework;

internal static class ConfigurationCache
{
    // (Entity Type, Property Name) -> Backing Field Name
    public static readonly Dictionary<(Type, string), string> BackingField = new();
}