using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profiles.Domain.Repositories.Abstractions
{
    public interface IFileRepository
    {
        public Task UploadFileAsync(string fileName, Stream fileStream, string contentType);
        public Task RemoveFileAsync(string fileName);
        public Task<bool> DoesFileExist(string fileName);
        public Task<string> GetFileUrlAsync(string fileName);
    }
}
