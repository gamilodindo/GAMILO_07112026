using FileProcessor.API.Configurations;
using Microsoft.Extensions.Options;

namespace FileProcessor.API.Middleware
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly APIKeyOptions _options;

        public ApiKeyMiddleware(RequestDelegate next, IOptions<APIKeyOptions> options)
        {
            _next = next;
            _options = options.Value;
        }

        public async Task Invoke(HttpContext context) {

            if (!context.Request.Headers.TryGetValue(_options.HeaderName, out var apiKey))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;

                await context.Response.WriteAsJsonAsync(new
                {
                    message = "API Key is missing."
                });

                return;
            }

            if (!string.Equals(apiKey, _options.Value))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;

                await context.Response.WriteAsJsonAsync(new
                {
                    message = "Invalid API Key."
                });

                return;
            }

            await _next(context);
        }
    }
}
