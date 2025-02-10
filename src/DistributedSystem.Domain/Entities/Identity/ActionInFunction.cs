using System.ComponentModel.DataAnnotations.Schema;

namespace DistributedSystem.Domain.Entities.Identity;

public class ActionInFunction
{
    [ForeignKey("ActionId")]
    public string ActionId { get; set; }

    [ForeignKey("FunctionId")]
    public string FunctionId { get; set; }
}