namespace DistributedSystem.Contract.Abstractions.Shared;

public interface IValidationResult
{
    public static readonly Error ValidationError = new Error("ValidationError", "Validation problem occurred.");

    Error[] Errors { get; }
}
