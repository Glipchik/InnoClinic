using System;

namespace Profiles.Application.Models;

public record FileModel
(
    string FileName,
    Stream FileStream,
    string ContentType
);
