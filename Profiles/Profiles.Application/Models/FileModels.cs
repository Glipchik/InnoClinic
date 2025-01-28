using System;

namespace Profiles.Application.Models;

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
