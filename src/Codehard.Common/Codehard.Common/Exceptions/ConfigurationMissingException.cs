using System.Runtime.Serialization;

namespace Codehard.Common.Exceptions;

/// <summary>
/// The exception that is thrown when a required configuration setting is missing.
/// </summary>
[Serializable]
public sealed class ConfigurationMissingException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurationMissingException"/> class with the specified configuration name.
    /// </summary>
    /// <param name="configurationName">The name of the missing configuration setting.</param>
    public ConfigurationMissingException(string configurationName)
        : base($"Configuration '{configurationName}' is not set")
    {
    }
    
    private ConfigurationMissingException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}