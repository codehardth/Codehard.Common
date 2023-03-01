using Codehard.Infrastructure.EntityFramework;
using Infrastructure.Test.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Test;

public class TestDbContext : DbContext
{
    public TestDbContext(DbContextOptions<TestDbContext> options)
        : base(options)
    {
    }

    public DbSet<MyModel> Models { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
    }
}

public class DesignTimeDbContext : DesignTimeDbContextBase<TestDbContext>
{
    protected override MigrationOptions GetMigrationOptions(string[] args)
    {
        return new MigrationOptions(
            "Server=127.0.0.1;Port=5438;Database=TestDatabase;User Id=postgres;Password=postgres;IncludeErrorDetail=true;");
    }

    protected override void ConfigureOptions(DbContextOptionsBuilder<TestDbContext> builder, MigrationOptions options)
    {
        builder.UseNpgsql(options.ConnectionString);
    }
}