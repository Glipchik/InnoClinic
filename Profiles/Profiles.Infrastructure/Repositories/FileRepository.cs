using Microsoft.Extensions.Configuration;
using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;
using Profiles.Domain.Repositories.Abstractions;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Profiles.Infrastructure.Repositories
{
    public class FileRepository : IFileRepository
    {
        private readonly IMinioClient _minioClient;
        private readonly string _bucketName;

        public FileRepository(IConfiguration configuration)
        {
            var endpoint = configuration["Minio:Endpoint"];
            var accessKey = configuration["Minio:AccessKey"];
            var secretKey = configuration["Minio:SecretKey"];
            _bucketName = configuration["Minio:BucketName"];

            _minioClient = new MinioClient()
                .WithEndpoint(endpoint)
                .WithCredentials(accessKey, secretKey)
                .Build();
        }

        public async Task<bool> DoesFileExist(string fileName)
        {
            try
            {
                await _minioClient.StatObjectAsync(new StatObjectArgs()
                    .WithBucket(_bucketName)
                    .WithObject(fileName));
                return true;
            }
            catch (MinioException)
            {
                return false;
            }
        }

        public async Task<string> GetFileUrlAsync(string fileName)
        {
            return await _minioClient.PresignedGetObjectAsync(new PresignedGetObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(fileName)
                .WithExpiry(24 * 60 * 60));
        }

        public async Task RemoveFileAsync(string fileName)
        {
            await _minioClient.RemoveObjectAsync(new RemoveObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(fileName));
        }

        public async Task UploadFileAsync(string fileName, Stream fileStream, string contentType)
        {
            var bucketExists = await _minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(_bucketName));
            if (!bucketExists)
            {
                await _minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(_bucketName));
            }

            await _minioClient.PutObjectAsync(new PutObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(fileName)
                .WithStreamData(fileStream)
                .WithObjectSize(fileStream.Length)
                .WithContentType(contentType));
        }
    }
}