using Offices.API.DTO.Enums;

namespace Offices.API.DTOs
{
    public record DoctorDto(string Id, string FirstName, string LastName, string? MiddleName, string OfficeId, DoctorStatusDto Status);
}
