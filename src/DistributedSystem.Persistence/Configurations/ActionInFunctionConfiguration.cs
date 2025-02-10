using DistributedSystem.Domain.Entities.Identity;
using DistributedSystem.Persistance.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DistributedSystem.Persistance.Configurations;

internal class ActionInFunctionConfiguration : IEntityTypeConfiguration<ActionInFunction>
{
    public void Configure(EntityTypeBuilder<ActionInFunction> builder)
    {
        builder.ToTable(TableNames.ActionInFunctions);

        builder.HasKey(ActionInFunctions => new { ActionInFunctions.ActionId, ActionInFunctions.FunctionId });
    }
}
