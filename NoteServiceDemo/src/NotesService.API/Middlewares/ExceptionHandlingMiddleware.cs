namespace NotesService.API.Middlewares;

using System.Net;
using NotesService.API.Abstractions.DTO.Response;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(
                RequestDelegate next,
                ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    // TODO: change to ProblemDetails
    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.LogError(exception, "An unexpected error occurred.");

        ErrorResponse response = exception switch
        {
            ApplicationException _ => new ErrorResponse
            {
                StatusCode = HttpStatusCode.BadRequest,
                Message = "Application exception.",
            },
            UnauthorizedAccessException _ => new ErrorResponse
            {
                StatusCode = HttpStatusCode.Unauthorized,
                Message = "Unauthorized.",
            },
            _ => new ErrorResponse
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Message = "Internal server error. Please retry later.",
            },
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)response.StatusCode;

        await context.Response.WriteAsJsonAsync(response);
    }
}
