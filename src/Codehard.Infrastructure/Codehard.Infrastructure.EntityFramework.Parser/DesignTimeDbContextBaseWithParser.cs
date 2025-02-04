using CommandLine;
using Microsoft.EntityFrameworkCore;

namespace Codehard.Infrastructure.EntityFramework.Parser;

/// <summary>
/// A base class for design time DbContext factory, using command line options to parse necessary values
/// for the migrator to run.
/// </summary>
/// <typeparam name="TContext"></typeparam>
public abstract class DesignTimeDbContextBaseWithParser<TContext> : DesignTimeDbContextBase<TContext>
    where TContext : DbContext
{
    /// <summary>
    /// Represents the parsed migrator options used during design-time DbContext creation.
    /// </summary>
    protected sealed class ParsedMigratorOptions
    {
        /// <summary>
        /// Gets the connection string for the database.
        /// </summary>
        [Option('c', "connection", Required = true)]
        public string ConnectionString { get; init; } = null!;
    }

    /// <summary>
    /// Gets the migration options by parsing command line arguments using CommandLine.Parser.
    /// </summary>
    /// <param name="args">Command line arguments to parse.</param>
    /// <returns>A <see cref="MigrationOptions"/> instance containing the parsed connection string.</returns>
    /// <exception cref="InvalidOperationException">Thrown when unable to parse the command line arguments.</exception>
    protected override MigrationOptions GetMigrationOptions(string[] args)
    {
        var options =
            CommandLine.Parser.Default.ParseArguments<ParsedMigratorOptions>(args)
                .MapResult(
                    res => new MigrationOptions(res.ConnectionString),
                    errs => throw new InvalidOperationException(
                        $"Unable to parse arguments during migration ({string.Join('\n', errs)}"));

        return options;
    }
}