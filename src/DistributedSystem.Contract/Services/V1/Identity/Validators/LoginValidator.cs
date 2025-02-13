using FluentValidation;

namespace DistributedSystem.Contract.Services.V1.Identity.Validators;
public class LoginValidator : AbstractValidator<Query.Login>
{
    public LoginValidator()
    {
        RuleFor(p => p.Email).NotEmpty().EmailAddress();
        RuleFor(p => p.Password).NotEmpty();
    }
}
