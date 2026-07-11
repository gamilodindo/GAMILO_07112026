using FileProcessor.Application.Dtos;
using FileProcessor.Application.Interfaces;
using FileProcessor.Domain.Entities;
using FileProcessor.Domain.Enums;
using FileProcessor.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FileProcessor.Application.Services
{
    public class FileProcessorService : IFileProcessorService
    {
        private readonly IFileProcessorRepository _repository;
        private readonly ILogger<FileProcessorService> _logger;
        public FileProcessorService(IFileProcessorRepository repository, ILogger<FileProcessorService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<UploadFileReponseDto> ProcessFileAsync(IFormFile file)
        {
            var stopwatch = Stopwatch.StartNew();

            var result = await this.ProcessAsync(file);

            var extension = Path.GetExtension(file.FileName);

            var entity = new ProcessedFileDetails
            {
                Filename = file.FileName,
                FileType = GetFileType(file.ContentType),
                ContentType = file.ContentType,
                FileSize = file.Length,
                Content = string.Empty,
                Result = result.Result,
                RecordCount = result.RecordCount,
                ProcessingTime = stopwatch.Elapsed.TotalMilliseconds,
            };

            await _repository.AddAsync(entity);

            _logger.LogInformation("Successfully processed {FileName}. Records: {Count}", file.FileName, result.RecordCount);

            return new UploadFileReponseDto
            {
                Filename = entity.Filename,
                RecordCount = entity.RecordCount,
                Result = entity.Result,
                ProcessingTime = $"{Math.Round(entity.ProcessingTime, 2)} ms",
                ProcessedDateTime = entity.DateCreated.ToString("MMMM dd, yyyy HH:mm:ss 'UTC'")
            };
        }

        public async Task<IEnumerable<ReportDto>> GetReportAsync()
        {
            var uploadedFiles = await _repository.GetAllAsync();

            return uploadedFiles.Select(x => new ReportDto
            { 
            
                Filename = x.Filename,
                FileType = x.FileType.ToString(),
                ContentType = x.ContentType,
                FileSize = $"{Math.Round((x.FileSize / 1024.0),2)} kb",
                RecordCount = $"{x.RecordCount} item(s)",
                ProcessingTime = $"{Math.Round(x.ProcessingTime,2)} ms",
                ProcessedDateTime = x.DateCreated.ToString("MMMM dd, yyyy HH:mm:ss 'UTC'")
            });
        }


        #region  Private Methods

        private async Task<UploadFileReponseDto> ProcessAsync(IFormFile file) {
            
            var contentType = file.ContentType;

            using var stream = file.OpenReadStream();
            using var reader = new StreamReader(stream);

            await reader.ReadLineAsync();

            switch (contentType) {
                case "text/csv":
                    return await ProcessCSV(reader);
                case "application/json":
                    return await ProcessJSON(stream);
                default:
                    throw new InvalidOperationException("File type not supported");
            }
        }

        private async Task<UploadFileReponseDto> ProcessCSV(StreamReader reader) {
            var values = new List<double>();
            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();

                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                var columns = line.Split(',');

                if (columns.Length == 0)
                {
                    continue;
                }

                if (double.TryParse(columns[^1], out var value))
                {
                    values.Add(value);
                }
            }

            if (!values.Any())
            {
                _logger.LogWarning("No numeric values where found");
                throw new InvalidOperationException("No numeric values where found");
            }

            return new UploadFileReponseDto{             
                Filename = reader.BaseStream is FileStream fileStream ? Path.GetFileName(fileStream.Name) : "Unknown",
                RecordCount = values.Count,
                Result = $"The total number of customers is {values.Count}, and the total amount paid is Php{values.Sum().ToString("N2")}. Individual payments range from Php{values.Min().ToString("N2")} to Php{values.Max().ToString("N2")}",
            };
        }
        private async Task<UploadFileReponseDto> ProcessJSON(Stream stream) {
            stream.Position = 0;
            var items = await JsonSerializer.DeserializeAsync<List<JSONFileItemDto>>(stream);

            char randomChar = (char)Random.Shared.Next('A', 'Z' + 1);

            var result = items.Where(x => x.LastName.StartsWith(randomChar)).ToList();

            return new UploadFileReponseDto
            {
                Filename = stream is FileStream fileStream ? Path.GetFileName(fileStream.Name) : "Unknown",
                RecordCount = result.Count,
                Result = $"Filtered {result.Count} records out of {items.Count} where lastname starts with '{randomChar}'. They are {string.Join(',',result.Select(x => x.LastName))}",
            };
        }
        
        private FileTypeEnum GetFileType(string contentType)
        {
            return contentType switch
            {
                "text/csv" => FileTypeEnum.CSV,
                "application/json" => FileTypeEnum.JSON,
                _ => FileTypeEnum.Unknown,
            };
        }

        #endregion
    }
}
