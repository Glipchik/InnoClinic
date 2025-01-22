using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authorization.Application.Models
{
    public class FileModel
    {
        public string FileName { get; }
        public Stream FileStream { get; }
        public string ContentType { get; }

        public FileModel(string fileName, Stream fileStream, string contentType)
        {
            FileName = fileName;
            FileStream = fileStream;
            ContentType = contentType;
        }
    }
}