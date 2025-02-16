using MassTransit;

namespace DistributedSystem.Contract.Abstractions.Message;

[ExcludeFromTopology] // Prevent create exchange for IDomainEvent
public interface IDomainEvent
{
}
