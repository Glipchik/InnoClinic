using System;

namespace Profiles.API.DTOs;

public class SpecializationDto
{
    public Guid Id { get; set; }
    public string SpecializationName { get; set; } = null!;
    public bool IsActive { get; set; }
}
