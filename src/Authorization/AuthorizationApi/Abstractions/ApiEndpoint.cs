using DistributedSystem.Contract.Abstractions.Shared;
using Microsoft.AspNetCore.Mvc;

namespace AuthorizationApi.Abstractions;

public abstract class ApiEndpoint
{
    internal static IResult HandlerFailure(Result result) =>
        result switch
        {
            { IsSuccess: true } => throw new InvalidOperationException(),
            IValidationResult validationResult =>
                Results.BadRequest(
                    CreateProblemDetails(
                        "Valication Error",
                        StatusCodes.Status400BadRequest,
                        result.Error,
                        validationResult.Errors)),
            _ => Results.BadRequest(
                    CreateProblemDetails(
                        "Bad Request",
                        StatusCodes.Status400BadRequest,
                        result.Error))
        };

    private static ProblemDetails CreateProblemDetails(string title, int status, Error error, Error[]? errors = null)
        => new()
        {
            Title = title,
            Type = error.Code,
            Detail = error.Message,
            Status = status,
            Extensions = { { nameof(errors), errors } }
        };

}
