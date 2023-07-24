using Codehard.Infrastructure.EntityFramework.Tests.ImmutableEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Codehard.Infrastructure.EntityFramework.Tests.Configurations.ImmutableEntities;

public class EntityBConfiguration : EntityTypeConfigurationBase<ImmutableEntityB, TestDbContext>
{
    protected override void EntityConfigure(EntityTypeBuilder<ImmutableEntityB> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Value)
               .IsRequired();

        builder
            .HasOne(b => b.A)
            .WithMany(a => a.Bs)
            .HasForeignKey(e => e.AId);

        builder.HasMany(e => e.Cs)
               .WithOne()
               .OnDelete(DeleteBehavior.Cascade);
    }
}