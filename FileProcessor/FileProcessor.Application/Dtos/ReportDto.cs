using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileProcessor.Application.Dtos
{
    public class ReportDto
    {
        public string Filename { get; set; }
        public string FileType { get; set; }
        public string ContentType { get; set; }
        public string FileSize { get; set; }
        public string RecordCount { get; set; }
        public string ProcessingTime { get; set; }
        public string ProcessedDateTime { get; set; }
    }
}
