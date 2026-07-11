using FileProcessor.Application.Dtos;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileProcessor.Application.Interfaces
{
    public interface IFileProcessorService
    {
        Task<UploadFileReponseDto> ProcessFileAsync(IFormFile file);
        Task<IEnumerable<UploadFileReponseDto>> GetReportAsync();
    }
}
