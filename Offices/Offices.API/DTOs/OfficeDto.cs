namespace Offices.API.DTOs
{
    public record OfficeDto(string Id, string Address, string PhotoURL, string RegistryPhoneNumber, bool IsActive);
}
