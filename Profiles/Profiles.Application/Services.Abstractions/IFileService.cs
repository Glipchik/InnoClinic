using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profiles.Application.Services.Abstractions
{
    public interface IFileService
    {
        public Task<bool> DoesFileExist(string fileName);
        public Task Upload(string fileName, Stream fileStream, string contentType);
        public Task Remove(string fileName);
        public Task<string> GetFileUrl(string fileName);
    }
}
