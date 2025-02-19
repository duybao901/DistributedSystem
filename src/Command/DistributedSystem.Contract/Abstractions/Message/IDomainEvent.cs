using MassTransit;

namespace DistributedSystem.Contract.Abstractions.Message;

[ExcludeFromTopology] // Prevent create exchange for IDomainEvent
public interface IDomainEvent
{
    Guid Id { get; init; }
    Guid IdEvent { get; init; }
}
