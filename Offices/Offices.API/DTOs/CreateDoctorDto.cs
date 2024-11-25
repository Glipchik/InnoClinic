namespace Offices.API.DTOs
{
    public record CreateDoctorDto(string FirstName, string LastName, string MiddleName, string OfficeId, string Status);
}
