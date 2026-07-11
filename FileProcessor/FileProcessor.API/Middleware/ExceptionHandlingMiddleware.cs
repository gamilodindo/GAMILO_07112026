using System.Text.Json;

namespace FileProcessor.API.Middleware
{
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
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, ex.Message);

                await WriteResponse(
                    context,
                    StatusCodes.Status400BadRequest,
                    ex.Message);
            }
            catch (FileNotFoundException ex)
            {
                _logger.LogWarning(ex, ex.Message);

                await WriteResponse(
                    context,
                    StatusCodes.Status404NotFound,
                    ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred. {0}",ex.Message.ToString());

                await WriteResponse(
                    context,
                    StatusCodes.Status500InternalServerError,
                    "An unexpected error occurred.");
            }
        }

        private static async Task WriteResponse(
            HttpContext context,
            int statusCode,
            string message)
        {
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(JsonSerializer.Serialize(new
            {
                StatusCode = statusCode,
                Message = message,
                Timestamp = DateTime.Now.ToString("MMMM dd, yyyy HH:mm:ss 'UTC'")
            }));
        }

    }
}
