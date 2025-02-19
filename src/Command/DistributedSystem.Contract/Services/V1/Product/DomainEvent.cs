using DistributedSystem.Contract.Abstractions.Message;
using MassTransit;

namespace DistributedSystem.Contract.Services.V1.Product;

[ExcludeFromTopology]
public static class DomainEvent
{
    public record ProductCreated(Guid IdEvent, Guid Id, string Name, decimal Price, string Description) : IDomainEvent, ICommand;
    public record ProductUpdated(Guid IdEvent, Guid Id, string Name, decimal Price, string Description) : IDomainEvent, ICommand;
    public record ProductDeleted(Guid IdEvent, Guid Id) : IDomainEvent, ICommand;
}
