using System;

namespace Profiles.Application.Models;

public class SpecializationModel
{
    public Guid Id { get; set; }
    public string SpecializationName { get; set; } = null!;
    public bool IsActive { get; set; }
}
