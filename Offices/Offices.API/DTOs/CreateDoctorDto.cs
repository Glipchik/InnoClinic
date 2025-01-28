using Offices.API.DTO.Enums;

namespace Offices.API.DTOs
{
    public record CreateDoctorDto(string FirstName, string LastName, string? MiddleName, string OfficeId, DoctorStatusDto Status);
}
