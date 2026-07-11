using FileProcessor.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FileProcessor.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IFileProcessorService _service;
        public FilesController(IFileProcessorService service)
        {
            _service = service;
        }

        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            var result = await  _service.ProcessFileAsync(file);
            return Ok(result);
        }

        [HttpGet("report")]
        public async Task<IActionResult> Report()
        {
            var result = await _service.GetReportAsync();
            return Ok(result);
        }
    }
}
