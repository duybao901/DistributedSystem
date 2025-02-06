using FluentValidation;
using MediatR;

namespace DistributedSystem.Application.Behaviors;

public sealed class ValidationPipelineDefaultBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TResponse>> _validators;

    public ValidationPipelineDefaultBehavior(IEnumerable<IValidator<TResponse>> validators) => _validators = validators;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if(!_validators.Any()) return await next();

        var context = new ValidationContext<TRequest>(request);

        var errorsDictionary = _validators
            .Select(validator => validator.Validate(context))
            .SelectMany(result => result.Errors)
            .Where(failure => failure is not null)
            .GroupBy(failure => new { failure.PropertyName, failure.ErrorMessage })
            .Select(x => x.FirstOrDefault())
            .ToList();

        if(errorsDictionary.Any())
        {
            throw new ValidationException(errorsDictionary);
        }

        return await next();
    }
}
