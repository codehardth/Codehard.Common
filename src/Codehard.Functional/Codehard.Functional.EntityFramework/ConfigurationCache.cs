namespace Codehard.Functional.EntityFramework;

internal static class ConfigurationCache
{
    // (Entity Type FullName, Property Name) -> Backing Field Name
    public static readonly Dictionary<(string, string), string> BackingField = new();
}