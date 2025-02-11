namespace DistributedSystem.Contract.Abstractions.Shared;

public interface IValidationResult
{
    public static readonly Error ValidationError = new Error("ValidationError", " validation problem occurred.");

    Error[] Errors { get; }
}
