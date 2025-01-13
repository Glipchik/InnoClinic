using System;

namespace Profiles.Application.Exceptions;

public class FileAlreadyExistsException : Exception
{
    public FileAlreadyExistsException()
        : base("File with this name already exists.")
    {
    }

    public FileAlreadyExistsException(string message)
        : base(message)
    {
    }

    public FileAlreadyExistsException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
