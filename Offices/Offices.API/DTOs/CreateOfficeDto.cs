namespace Offices.API.DTOs
{
    public record CreateOfficeDto(string Address, string PhotoURL, string RegistryPhoneNumber, bool IsActive);
}
