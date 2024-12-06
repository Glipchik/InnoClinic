using Services.Application.Models;

namespace Services.API.DTOs
{
    public record CreateSpecializationDto(string SpecializationName, bool IsActive);
    public record SpecializationDto(Guid Id, string SpecializationName, bool IsActive);
    public record UpdateSpecializationDto(Guid Id, string SpecializationName, bool IsActive);
}
