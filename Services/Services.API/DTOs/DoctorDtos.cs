using Services.Application.Models.Enums;
using Services.Application.Models;
using Services.API.DTOs.Enums;

namespace Services.API.DTOs
{
    public record CreateDoctorDto(string FirstName, string LastName, string MiddleName, Guid SpecializationId, DoctorStatusDto Status);
    public record DoctorDto(Guid Id, string FirstName, string LastName, string MiddleName, Guid SpecializationId, DoctorStatusDto Status);
    public record UpdateDoctorDto(Guid Id, string FirstName, string LastName, string MiddleName, Guid SpecializationId, DoctorStatusDto Status);
}
