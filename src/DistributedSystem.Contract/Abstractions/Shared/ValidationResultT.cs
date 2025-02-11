namespace DistributedSystem.Contract.Abstractions.Shared;

public class ValidationResult<T> : Result<T>, IValidationResult
{
    public ValidationResult(Error[] errors) : base(default, false, IValidationResult.ValidationError) =>
        Errors = errors;
    public Error[] Errors { get; }
    public static ValidationResult WithErrors(Error[] errors) => new(errors);
}
