using Codehard.Common.DomainModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Codehard.Infrastructure.EntityFramework.Processors;

public interface IDomainEventProcessor
{
    void Process(DbContext dbContext, IEnumerable<IEntity> entities);

    ValueTask ProcessAsync(DbContext dbContext, IEnumerable<IEntity> entities, CancellationToken cancellationToken);
}