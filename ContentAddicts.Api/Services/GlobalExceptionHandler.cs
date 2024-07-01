
using System.Net;

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ContentAddicts.Api.Services;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var problemDetails = new ProblemDetails()
        {
            Status = (int)HttpStatusCode.InternalServerError,
            Title = "An unexpected internal server error occurred",
            Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}"
        };

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        logger.LogError(exception, "An unexpected internal server error occurred");

        return true;
    }
}
