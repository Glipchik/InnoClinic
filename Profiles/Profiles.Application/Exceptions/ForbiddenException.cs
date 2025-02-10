using System;

namespace Profiles.Application.Exceptions;

public class ForbiddenException : BadRequestException
{
    public ForbiddenException()
        : base("Forbidden.")
    {
    }

    public ForbiddenException(string message)
        : base(message)
    {
    }

    public ForbiddenException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
