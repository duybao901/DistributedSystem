using DistributedSystem.Persistance.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Action = DistributedSystem.Domain.Entities.Identity.Action;

namespace DistributedSystem.Persistance.Configurations;

internal class ActionConfiguration : IEntityTypeConfiguration<Action>
{

    public void Configure(EntityTypeBuilder<Action> builder)
    {
        builder.ToTable(TableNames.Actions);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasMaxLength(50);
        builder.Property(x => x.Name).HasMaxLength(200).IsRequired(true);
        builder.Property(x => x.IsActive).HasDefaultValue(true);
        builder.Property(x => x.SortOrder).HasDefaultValue(null);

        // Each User can have many Permission
        builder.HasMany(Action => Action.Permissions)
            .WithOne()
            .HasForeignKey(Permissions => Permissions.ActionId)
            .IsRequired();

        // Each User can have many ActionInFunction
        builder.HasMany(Action => Action.ActionInFunctions)
            .WithOne()
            .HasForeignKey(ActionInFunctions => ActionInFunctions.ActionId)
            .IsRequired();
    }
}
