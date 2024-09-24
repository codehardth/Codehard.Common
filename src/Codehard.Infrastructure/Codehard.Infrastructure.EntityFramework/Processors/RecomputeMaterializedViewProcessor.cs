using Codehard.Common.DomainModel;
using Microsoft.EntityFrameworkCore;

namespace Codehard.Infrastructure.EntityFramework.Processors;

public class RecomputeMaterializedViewProcessor : IDomainEventProcessor
{
    protected readonly string MaterializedViewName;

    public RecomputeMaterializedViewProcessor(string materializedViewName)
    {
        this.MaterializedViewName = materializedViewName;
    }

    public void Process(DbContext dbContext, IEnumerable<IEntity> _)
    {
        dbContext.Database.ExecuteSqlRaw(this.GetRefreshMaterializedViewQuery(this.MaterializedViewName));
    }

    public async ValueTask ProcessAsync(DbContext dbContext, IEnumerable<IEntity> _, CancellationToken cancellationToken)
    {
        await dbContext.Database.ExecuteSqlRawAsync(
            this.GetRefreshMaterializedViewQuery(this.MaterializedViewName),
            cancellationToken);
    }

    protected virtual string GetRefreshMaterializedViewQuery(string materializedViewName)
    {
        return $"REFRESH MATERIALIZED VIEW {materializedViewName};";
    }
}