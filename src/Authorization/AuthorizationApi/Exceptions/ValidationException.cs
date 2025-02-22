namespace AuthorizationApi.Exceptions;

public class ValidationException : DomainException
{
    public ValidationException(IReadOnlyCollection<ValidationError> errors)
        : base("Validation Failure", "One or more validation errors occurred")
    {
        Errors = errors;
    }

    public IReadOnlyCollection<ValidationError> Errors { get;  }
}

public record ValidationError(string Code, string Message);