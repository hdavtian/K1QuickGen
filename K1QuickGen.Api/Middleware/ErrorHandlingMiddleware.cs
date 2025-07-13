using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace K1QuickGen.Api.Middleware
{
    /// <summary>
    /// Middleware for global error handling in the ASP.NET Core pipeline.
    /// Catches unhandled exceptions, logs them, and returns a standardized JSON error response.
    /// </summary>
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        /// <summary>
        /// Constructor. Initializes a new instance of the <see cref="ErrorHandlingMiddleware"/> class using Dependency Injection (DI).
        /// </summary>
        /// <param name="next">The next middleware in the pipeline, injected via DI.</param>
        /// <param name="logger">The logger instance for logging errors, injected via DI.</param>
        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Invokes the middleware to handle HTTP requests.
        /// Wraps the request processing in a try-catch block to intercept unhandled exceptions.
        /// If an exception occurs, logs the error and returns a 500 Internal Server Error response with a JSON payload.
        /// </summary>
        /// <param name="context">The current HTTP context.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Proceed to the next middleware/component in the pipeline
                await _next(context);
            }
            catch (Exception ex)
            {
                // Log the unhandled exception
                _logger.LogError(ex, "Unhandled exception occurred.");

                // Set the response status code and content type
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                // Create a standardized error response
                var result = JsonSerializer.Serialize(new { error = "An unexpected error occurred." });

                // Write the error response to the HTTP response body
                await context.Response.WriteAsync(result);
            }
        }
    }
}
