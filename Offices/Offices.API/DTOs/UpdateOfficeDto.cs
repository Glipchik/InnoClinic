namespace Offices.API.DTOs
{
    public record UpdateOfficeDto(string Id, string Address, string RegistryPhoneNumber, bool IsActive);
}
