using Microsoft.AspNetCore.Identity;

namespace DistributedSystem.Domain.Entities.Identity;

public class AppUser : IdentityUser<Guid>
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string FullName { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public string UserName { get; set; }

    public DateTime? DayOfBirth { get; set; }

    public bool IsDirector { get; set; }

    public bool IsHeadOfDepartment { get; set; }

    public Guid? ManagerId { get; set; }

    public Guid PositionId { get; set; }

    public int IsReceipient { get; set; }

    public virtual ICollection<IdentityUserClaim<Guid>> UserClaims { get; set; }
    public virtual ICollection<IdentityUserLogin<Guid>> UserLogins { get; set; }
    public virtual ICollection<IdentityUserToken<Guid>> UserTokens { get; set; }
    public virtual ICollection<IdentityUserRole<Guid>> UserRoles { get; set; }
}
