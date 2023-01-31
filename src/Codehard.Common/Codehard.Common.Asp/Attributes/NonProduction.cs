using Microsoft.Extensions.Hosting;

namespace Codehard.Common.Asp.Attributes;

/// <summary>
/// A filter attribute that prevent the execution of specific action when running in Production environment
/// </summary>
public sealed class NonProductionAttribute : DisallowOnEnvironmentAttribute
{
    public NonProductionAttribute() : base(Environments.Production)
    {
    }
}