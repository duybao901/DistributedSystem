using Microsoft.AspNetCore.Identity;

namespace DistributedSystem.Domain.Entities.Identity;

public class AppRole : IdentityRole<Guid>
{
    public string Description { get; set; }
    public string RoleCode { get; set; }

    /// <summary>
    /// The intermediate table (User ↔ Role) helps manage which role a user belongs to.
    /// </summary>
    public virtual ICollection<IdentityUserRole<Guid>> UserRoles { get; set; }

    /// <summary>
    /// "CanViewDashboard" = true
    /// "CanEditUser" = true
    /// </summary>
    public virtual ICollection<IdentityRoleClaim<Guid>> RoleClaims { get; set; }

    /// <summary>
    /// Permissions are more detailed permissions (eg "Manage products", "Delete posts").
    /// </summary>
    public virtual ICollection<Permission> Permissions { get; set; }
}
