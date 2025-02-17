namespace Profiles.API.DTOs
{
    public record CreateReceptionistDto(
        CreateAccountDto Account,
        string FirstName,
        string LastName,
        string? MiddleName,
        Guid OfficeId);

    public record ReceptionistDto(
        Guid Id,
        string FirstName,
        string LastName,
        string? MiddleName,
        AccountDto Account,
        OfficeDto Office);

    public record UpdateReceptionistDto(
        Guid Id,
        string FirstName,
        string LastName,
        string? MiddleName,
        Guid OfficeId);
}
