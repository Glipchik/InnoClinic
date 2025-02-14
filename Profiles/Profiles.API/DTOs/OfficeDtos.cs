using System;

namespace Profiles.API.DTOs;

public class OfficeDto
{
    public Guid Id { get; set; }
    public string Address { get; set; } = null!;
    public string RegistryPhoneNumber { get; set; } = null!;
    public bool IsActive { get; set; }
}
