using DistributedSystem.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using DistributedSystem.Persistence.Constants;

namespace DistributedSystem.Persistence.Configurations;

internal sealed class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.ToTable(TableNames.AppUsers);

        builder.HasKey(x => x.Id);
        builder.Property(x => x.IsDirector).HasDefaultValue(false);
        builder.Property(x => x.IsHeadOfDepartment).HasDefaultValue(false);
        builder.Property(x => x.ManagerId).HasDefaultValue(null);
        builder.Property(x => x.IsReceipient).HasDefaultValue(-1);

        // Each User can have many UserClaims
        builder.HasMany(AppUsers => AppUsers.UserClaims)
            .WithOne()
            .HasForeignKey(UserClaims => UserClaims.UserId)
            .IsRequired();

        // Each User can have many UserLogins
        builder.HasMany(AppUsers => AppUsers.UserLogins)
            .WithOne()
            .HasForeignKey(UserLogins => UserLogins.UserId)
            .IsRequired();

        // Each User can have many UserTokens
        builder.HasMany(AppUsers => AppUsers.UserTokens)
            .WithOne()
            .HasForeignKey(UserTokens => UserTokens.UserId)
            .IsRequired();

        // Each User can have many entries in the UserRole join table
        builder.HasMany(AppUsers => AppUsers.UserRoles)
            .WithOne()
            .HasForeignKey(UserRoles => UserRoles.UserId)
            .IsRequired();
    }
}