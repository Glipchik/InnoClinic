using System;

namespace Profiles.Application.Models;

public class OfficeModel
{
    public Guid Id { get; set; }
    public string Address { get; set; } = null!;
    public string RegistryPhoneNumber { get; set; } = null!;
    public bool IsActive { get; set; }
}
