using Offices.Application.Models.Enums;

namespace Offices.Application.Models
{
    public record CreateDoctorModel(string FirstName, string LastName, string? MiddleName, Guid OfficeId, DoctorStatusModel Status);
}