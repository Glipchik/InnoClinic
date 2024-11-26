namespace Offices.API.DTOs
{
    public record UpdateDoctorDto(string Id, string FirstName, string LastName, string MiddleName, string OfficeId, string Status);
}
