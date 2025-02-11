using DistributedSystem.Contract.Abstractions.Message;
using DistributedSystem.Contract.Abstractions.Shared;
using DistributedSystem.Contract.Services.V1.Product;
using DistributedSystem.Domain.Abstractions.Repositories;
namespace DistributedSystem.Application.UserCases.V1.Commands.Product;

public sealed class CreateProductCommandHandler : ICommandHandler<Command.CreateProductCommand>
{
    private readonly IRepositoryBase<DistributedSystem.Domain.Entities.Product, Guid> _productRepository;

    public CreateProductCommandHandler(IRepositoryBase<DistributedSystem.Domain.Entities.Product, Guid> repositoryBase)
    {
        _productRepository = repositoryBase;
    }

    public async Task<Result> Handle(Command.CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = DistributedSystem.Domain.Entities.Product.CreateProduct(
            Guid.NewGuid(),
            request.Name,
            request.Price,
            request.Description);

        _productRepository.Add(product);

        return Result.Success();
    }
}
