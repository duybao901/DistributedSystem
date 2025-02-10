using DistributedSystem.Domain.Entities.Identity;
using DistributedSystem.Persistance.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DistributedSystem.Persistance.Configurations;

internal sealed class FunctionConfiguration : IEntityTypeConfiguration<Function>
{
    public void Configure(EntityTypeBuilder<Function> builder)
    {
        builder.ToTable(TableNames.Functions);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasMaxLength(50);
        builder.Property(x => x.Name).HasMaxLength(200).IsRequired(true);
        builder.Property(x => x.ParentId)
            .HasMaxLength(50)
            .HasDefaultValue(null);
        builder.Property(x => x.CssClass).HasMaxLength(50).HasDefaultValue(null);
        builder.Property(x => x.Url).HasMaxLength(50).IsRequired(true);
        builder.Property(x => x.IsActive).HasDefaultValue(true);
        builder.Property(x => x.SortOrder).HasDefaultValue(null);

        builder.HasMany(Functions => Functions.Permissions)
            .WithOne()
            .HasForeignKey(Permissions => Permissions.FunctionId)
            .IsRequired();

        builder.HasMany(Functions => Functions.ActionInFunctions)
            .WithOne()
            .HasForeignKey(ActionInFuntions => ActionInFuntions.FunctionId)
            .IsRequired();
    }
}
