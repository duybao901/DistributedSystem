using DistributedSystem.Domain.Entities.Identity;
using DistributedSystem.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DistributedSystem.Persistence.Configurations;

internal sealed class AppRoleConfiguration : IEntityTypeConfiguration<AppRole>
{
    public void Configure(EntityTypeBuilder<AppRole> builder)
    {
        builder.ToTable(TableNames.AppRoles);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Description)
            .HasMaxLength(250).IsRequired(true);
        builder.Property(x => x.RoleCode).HasMaxLength(50).IsRequired(true);

        // Each User can have many RoleClaims
        builder.HasMany(AppRoles => AppRoles.RoleClaims)
            .WithOne()
            .HasForeignKey(RoleClaims => RoleClaims.RoleId)
            .IsRequired();

        // Each User can have many entries in the UserRole join table
        builder.HasMany(AppRoles => AppRoles.UserRoles)
            .WithOne()
            .HasForeignKey(UserRoles => UserRoles.RoleId)
            .IsRequired();

        // Each User can have many Permission
        builder.HasMany(AppRoles => AppRoles.Permissions)
            .WithOne()
            .HasForeignKey(Permissions => Permissions.RoleId)
            .IsRequired();
    }
}
