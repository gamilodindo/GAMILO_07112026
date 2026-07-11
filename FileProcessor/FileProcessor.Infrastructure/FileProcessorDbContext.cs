using FileProcessor.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FileProcessor.Infrastructure
{
    public class FileProcessorDbContext : DbContext
    {
        public FileProcessorDbContext(DbContextOptions<FileProcessorDbContext> options) : base(options)
        {
            
        }

        public DbSet<ProcessedFileDetails> ProcessedFileDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
