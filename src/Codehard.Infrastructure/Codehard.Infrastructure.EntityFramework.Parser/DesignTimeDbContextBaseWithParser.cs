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
    protected sealed class ParsedMigratorOptions
    {
        [Option('c', "connection", Required = true)]
        public string ConnectionString { get; init; } = null!;
    }

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