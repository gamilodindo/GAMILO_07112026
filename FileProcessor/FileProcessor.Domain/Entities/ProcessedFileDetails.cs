using FileProcessor.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileProcessor.Domain.Entities
{
    public class ProcessedFileDetails
    {
        public Guid Id { get; set; }
        public string Filename { get; set; } = string.Empty;
        public FileTypeEnum FileType { get; set; }
        public string ContentType { get; set; } = string.Empty;
        public double FileSize { get; set; }
        public string Content { get; set; }
        public int RecordCount { get; set; }
        public double ProcessingTime { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
    }
}
