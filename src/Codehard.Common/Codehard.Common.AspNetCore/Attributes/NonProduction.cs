using Microsoft.Extensions.Hosting;

namespace Codehard.Common.AspNetCore.Attributes;

/// <summary>
/// A filter attribute that prevent the execution of specific action when running in Production environment
/// </summary>
public sealed class NonProductionAttribute : DisallowOnEnvironmentAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NonProductionAttribute"/> class.
    /// </summary>
    public NonProductionAttribute() : base(Environments.Production)
    {
    }
}