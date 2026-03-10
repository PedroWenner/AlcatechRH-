using System.Net;
using System.Text.Json;
using DPManagement.Application.Common;
using Microsoft.AspNetCore.Diagnostics;

namespace DPManagement.API.Middleware;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Unhandled exception occurred: {Message}", exception.Message);

        var result = OperationResult.Failure("Ocorreu um erro interno no servidor.", exception.Message);
        
        // If it's a known business exception (optional: create a BusinessException class later)
        // we could set a different status code or message.
        
        httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        httpContext.Response.ContentType = "application/json";

        await httpContext.Response.WriteAsync(JsonSerializer.Serialize(result, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        }), cancellationToken);

        return true;
    }
}
