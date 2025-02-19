using System.ComponentModel.DataAnnotations.Schema;

namespace DistributedSystem.Domain.Entities.Identity;

public class Permission
{
    [ForeignKey("RoleId")]
    public Guid RoleId { get; set; }

    [ForeignKey("FunctionId")]
    public string FunctionId { get; set; }

    [ForeignKey("ActionId")]
    public string ActionId { get; set; }
}