namespace Offices.API.DTOs
{
    public record UpdateOfficeDto(string Id, string Address, string PhotoURL, string RegistryPhoneNumber, bool IsActive);
}
