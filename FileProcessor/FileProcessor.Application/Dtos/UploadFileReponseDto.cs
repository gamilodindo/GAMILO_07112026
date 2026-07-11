using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileProcessor.Application.Dtos
{
    public class UploadFileReponseDto
    {
        public string Filename { get; set; } = string.Empty;
        public int RecordCount { get; set; }
        public string Result { get; set; } = string.Empty;
        public DateTime ProcessedDateTime { get; set; } = DateTime.UtcNow;
    }
}
