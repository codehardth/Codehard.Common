using System.Runtime.Serialization;

namespace Codehard.Common.Exceptions;

[Serializable]
public sealed class ConfigurationMissingException : Exception
{
    public ConfigurationMissingException(string configurationName)
        : base($"Configuration '{configurationName}' is not set")
    {
    }

    private ConfigurationMissingException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}