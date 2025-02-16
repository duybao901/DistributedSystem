using DistributedSystem.Contract.Abstractions.Message;
using DistributedSystem.Contract.Abstractions.Shared;
using DistributedSystem.Contract.Services.V1.Product;

namespace DistributedSystem.Infrastructure.Consumer.UseCases.Events;

internal class ProjectProductDetailsWhenProductChangeEventHandler : 
    ICommandHandler<DomainEvent.ProductCreated>,
    ICommandHandler<DomainEvent.ProductUpdated>,
    ICommandHandler<DomainEvent.ProductDeleted>
{
    // Repository working with MongoDB
    public async Task<Result> Handle(DomainEvent.ProductCreated request, CancellationToken cancellationToken)
    {
        // Create new a Product
        await Task.Delay(1000);
        return Result.Success();
    }

    public async Task<Result> Handle(DomainEvent.ProductUpdated request, CancellationToken cancellationToken)
    {
        // Find and update Product
        await Task.Delay(1000);
        return Result.Success();
    }

    public async Task<Result> Handle(DomainEvent.ProductDeleted request, CancellationToken cancellationToken)
    {
        // Find and delete Product
        await Task.Delay(1000);
        return Result.Success();
    }
}
