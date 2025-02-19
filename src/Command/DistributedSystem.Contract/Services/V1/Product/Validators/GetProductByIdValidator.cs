using FluentValidation;

namespace DistributedSystem.Contract.Services.V1.Product.Validators;
public class GetProductByIdValidator : AbstractValidator<Query.GetProductByIdQuery>
{
    public GetProductByIdValidator()
    {
        RuleFor(product => product.Id).NotEmpty();
    }
}