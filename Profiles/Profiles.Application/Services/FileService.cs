using Profiles.Application.Services.Abstractions;
using Profiles.Domain.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profiles.Application.Services
{
    public class FileService : IFileService
    {
        private readonly IFileRepository _fileRepository;

        public FileService(IFileRepository fileRepository)
        {
            _fileRepository = fileRepository;
        }

        public async Task<string> GetFileUrl(string fileName)
        {
            return await _fileRepository.GetFileUrlAsync(fileName);
        }

        public async Task Upload(string fileName, Stream fileStream, string contentType)
        {
            await _fileRepository.UploadFileAsync(fileName, fileStream, contentType);
        }
    }
}
