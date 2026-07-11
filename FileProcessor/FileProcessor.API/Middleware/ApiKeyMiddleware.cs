using FileProcessor.API.Configurations;
using Microsoft.Extensions.Options;

namespace FileProcessor.API.Middleware
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly APIKeyOptions _options;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ApiKeyMiddleware> _logger;

        public ApiKeyMiddleware(RequestDelegate next, IOptions<APIKeyOptions> options, IConfiguration configuration, ILogger<ApiKeyMiddleware> logger)
        {
            _next = next;
            _options = options.Value;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context) {

            _logger.LogInformation("Incoming request: {Method} {Path}",context.Request.Method,context.Request.Path);

            if (!context.Request.Headers.TryGetValue(_options.HeaderName, out var apiKey))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;

                await context.Response.WriteAsJsonAsync(new
                {
                    message = "API Key is missing."
                });

                _logger.LogWarning("UNAUTHORIZRED: API Key is missing.");

                return;
            }

            if (!string.Equals(apiKey, _options.Value))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;

                await context.Response.WriteAsJsonAsync(new
                {
                    message = "Invalid API Key."
                });

                _logger.LogWarning("UNAUTHORIZRED: Invalid API Key.");
                return;
            }

            await _next(context);

            _logger.LogInformation("Response Status: {StatusCode}", context.Response.StatusCode);
        }

    }
}
