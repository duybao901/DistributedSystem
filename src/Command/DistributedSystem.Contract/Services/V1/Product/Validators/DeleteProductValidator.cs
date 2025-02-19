using FluentValidation;

namespace DistributedSystem.Contract.Services.V1.Product.Validators;
public class DeleteProductValidator : AbstractValidator<Command.DeleteProductCommand>
{
    public DeleteProductValidator()
    {
        RuleFor(product => product.Id).NotEmpty().NotNull();
    }
}