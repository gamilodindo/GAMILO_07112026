using FileProcessor.Domain.Entities;
using FileProcessor.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileProcessor.Infrastructure.Repositories
{
    public class FileProcessorRepository : IFileProcessorRepository
    {
        private readonly FileProcessorDbContext _context;
        public FileProcessorRepository(FileProcessorDbContext context)
        {
            _context = context;
        }

        public async Task<ProcessedFileDetails> AddAsync(ProcessedFileDetails file)
        {
            await _context.ProcessedFileDetails.AddAsync(file);
            await _context.SaveChangesAsync();
            return file;
        }

        public async Task<List<ProcessedFileDetails>> GetAllAsync()
        {
            return await _context.ProcessedFileDetails.ToListAsync();
        }

        public async Task<ProcessedFileDetails> GetByIdAsync(Guid id)
        {
            return await _context.ProcessedFileDetails.FindAsync(id);
        }
    }
}
