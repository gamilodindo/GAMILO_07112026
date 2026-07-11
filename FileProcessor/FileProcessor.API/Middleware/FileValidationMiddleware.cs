namespace FileProcessor.API.Middleware
{
    public class FileValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        private readonly ILogger<FileValidationMiddleware> _logger;

        public FileValidationMiddleware(RequestDelegate next, IConfiguration configuration, ILogger<FileValidationMiddleware> logger)
        {
            _next = next;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
                 
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

                _logger.LogWarning("No file was uploaded.");
                return;
            }

            var extension = Path.GetExtension(file.FileName);

            if (!IsAllowedFileType(extension))
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                var message = $"'{extension}' files are not supported.";
                await context.Response.WriteAsJsonAsync(new
                {
                    Message = message
                });

                _logger.LogWarning(message);

                return;
            }

            _logger.LogInformation("Incoming file upload: {FileName}", file.FileName);
            await _next(context);
        }

        #region private methods

        
        private bool IsAllowedFileType(string extension)
        {
            var allowedFileTypes = _configuration.GetSection("FileProcessingOptions:AllowedFileTypes").Get<List<string>>();
            return allowedFileTypes.Any(ext =>
                ext.Equals(extension, StringComparison.OrdinalIgnoreCase));
        }

        #endregion
    }
}
