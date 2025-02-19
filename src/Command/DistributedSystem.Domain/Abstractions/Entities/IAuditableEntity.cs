namespace DistributedSystem.Domain.Abstractions.Entities;

public interface IAuditableEntity
{
    // Coordinated Universal Time (Thời gian Phối hợp Quốc tế)
    DateTimeOffset CreatedOnUtc { get; set; }
    DateTimeOffset? ModifiedOnUtc { get; set; }
}
