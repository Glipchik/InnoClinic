namespace Offices.API.DTOs
{
    public record UpdateReceptionistDto(string Id, string FirstName, string LastName, string? MiddleName, string OfficeId);
}
