using FluentValidation;

namespace DistributedSystem.Contract.Services.V1.Product.Validators;

public class CreateProductValidator : AbstractValidator<Command.CreateProductCommand>
{
    public CreateProductValidator()
    {
        RuleFor(p => p.Name).NotEmpty();
        RuleFor(p => p.Price).GreaterThan(0);
        RuleFor(p => p.Description).NotEmpty();
    }
}
