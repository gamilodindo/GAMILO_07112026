namespace FileProcessor.API.Middleware
{
    public class FileValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public FileValidationMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!IsProtectedEndpoint(context.Request.Path))
            {
                await _next(context);
                return;
            }

            if (!context.Request.HasFormContentType)
            {
                await _next(context);
                return;
            }

            var form = await context.Request.ReadFormAsync();
            var file = form.Files.FirstOrDefault();

            if (file is null)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;

                await context.Response.WriteAsJsonAsync(new
                {
                    Message = "No file was uploaded."
                });

                return;
            }

            var extension = Path.GetExtension(file.FileName);

            if (!IsAllowedFileType(extension))
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;

                await context.Response.WriteAsJsonAsync(new
                {
                    Message = $"'{extension}' files are not supported."
                });

                return;
            }

            await _next(context);
        }

        #region private methods

        private bool IsProtectedEndpoint(PathString requestPath)
        {
            var protectedEndpoints = _configuration.GetSection("FileProcessingOptions:ProtectedEndpoints").Get<List<string>>();
            return protectedEndpoints.Any(endpoint =>
                requestPath.StartsWithSegments(endpoint, StringComparison.OrdinalIgnoreCase));
        }
        private bool IsAllowedFileType(string extension)
        {
            var allowedFileTypes = _configuration.GetSection("FileProcessingOptions:AllowedFileTypes").Get<List<string>>();
            return allowedFileTypes.Any(ext =>
                ext.Equals(extension, StringComparison.OrdinalIgnoreCase));
        }

        #endregion
    }
}
