
using DistributedSystem.Domain.Exceptions;
using System.Text.Json;

namespace DistributedSystem.API.Middleware;

internal sealed class ExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, exception.Message);

            await HandleExceptionAsync(context, exception);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
        var statusCode = GetStatusCode(exception);

        var response = new
        {
            Title = GetTitle(exception),
            Status = statusCode,
            Detail = exception.Message,
            Errors = GetErrors(exception)
        };

        httpContext.Response.ContentType = "application/json";

        httpContext.Response.StatusCode = statusCode;

        await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));
    }

    private static int GetStatusCode(Exception exception)
        => exception switch
        {
            BadRequestException => StatusCodes.Status400BadRequest,
            NotFoundException => StatusCodes.Status404NotFound,
            FluentValidation.ValidationException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };

    private static string GetTitle(Exception exception)
        => exception switch
        {
            DomainException domainException => domainException.Title,
            _ => "Server Error"
        };

    private static IReadOnlyCollection<Application.Exceptions.ValidationError> GetErrors(Exception exception)
    {
        IReadOnlyCollection<Application.Exceptions.ValidationError> errors = null;

        if(exception is Application.Exceptions.ValidationException validationException)
        {
            errors = validationException.Errors;
        }

        return errors;
    }
}
