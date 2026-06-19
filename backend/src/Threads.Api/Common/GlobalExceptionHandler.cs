using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Threads.Api.Common;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly IProblemDetailsService _problemDetailsService;
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(
        IProblemDetailsService problemDetailsService,
        ILogger<GlobalExceptionHandler> logger
    )
    {
        _problemDetailsService = problemDetailsService;
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken
    )
    {
        var requestPath = string.Concat(httpContext.Request.Path, httpContext.Request.QueryString);

        _logger.LogError(
            exception,
            "Unhandled exception occurred while processing the request at {RequestPath}.",
            requestPath
        );

        return await _problemDetailsService.TryWriteAsync(
            new ProblemDetailsContext()
            {
                HttpContext = httpContext,
                Exception = exception,
                ProblemDetails = new ProblemDetails()
                {
                    Title = "Internal server error",
                    Detail = "An unexpected error occurred while processing your request.",
                    Status = StatusCodes.Status500InternalServerError,
                },
            }
        );
    }
}
