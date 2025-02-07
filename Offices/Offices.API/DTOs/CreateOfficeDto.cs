namespace Offices.API.DTOs
{
    public record CreateOfficeDto(string Address, string RegistryPhoneNumber, bool IsActive);
}
