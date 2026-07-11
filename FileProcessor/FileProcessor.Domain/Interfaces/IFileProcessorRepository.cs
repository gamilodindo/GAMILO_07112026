using FileProcessor.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileProcessor.Domain.Interfaces
{
    public interface IFileProcessorRepository
    {
        Task<ProcessedFileDetails> AddAsync(ProcessedFileDetails file);
        Task<List<ProcessedFileDetails>> GetAllAsync();
        Task<ProcessedFileDetails> GetByIdAsync(Guid id);
    }
}
