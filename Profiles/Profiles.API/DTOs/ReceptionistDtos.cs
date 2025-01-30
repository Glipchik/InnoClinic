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
        Guid AccountId,
        Guid OfficeId);

    public record UpdateReceptionistDto(
        Guid Id,
        string FirstName,
        string LastName,
        string? MiddleName,
        Guid OfficeId);
}
