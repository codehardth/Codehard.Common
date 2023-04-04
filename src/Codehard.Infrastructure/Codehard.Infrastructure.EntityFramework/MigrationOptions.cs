namespace Codehard.Infrastructure.EntityFramework;

/// <summary>
/// Represents the options for a database migration.
/// </summary>
public sealed record MigrationOptions
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MigrationOptions"/> class with the specified connection string.
    /// </summary>
    /// <param name="connectionString">The connection string to use for the migration.</param>
    public MigrationOptions(string connectionString)
    {
        ConnectionString = connectionString;
    }

    /// <summary>
    /// Gets the connection string to use for the migration.
    /// </summary>
    public string ConnectionString { get; }
}